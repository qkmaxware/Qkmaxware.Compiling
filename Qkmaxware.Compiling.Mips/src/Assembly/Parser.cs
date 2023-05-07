using System.IO;
using System.Text;
using System.Collections.Generic;
using Qkmaxware.Compiling.Targets.Mips.Assembly;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public delegate bool TryDecodeAssembly(IdentifierToken opcode, List<Token> args, out IAssemblyInstruction? decoded);

public class Parser {
    public AssemblyProgram Parse(IEnumerable<Token> tokens) {
        var stream = new BufferedTokenStream(tokens);
        return parseProgram(stream);
    }

    private AssemblyProgram parseProgram(BufferedTokenStream tokens) {
        AssemblyProgram program = new AssemblyProgram();

        while (tokens.HasNext()) {
            // Eat newlines
            eatNewlines(tokens);
            if (tokens.HasNext())
                program.Sections.Add(parseSection(tokens));
            else
                break;
        }

        return program;
    }

    private Section parseSection(BufferedTokenStream tokens) {
        if (!tokens.IsLookahead<DirectiveToken>(0)) {
            throw new AssemblyException(tokens.SourcePosition, "Missing section directive.");
        }

        var dir = (DirectiveToken?)tokens.Peek(0);
        return (dir?.Value) switch {
            "globl" => parseGlobalSection(tokens),
            "data" => parseDataSection(tokens),
            "text" => parseTextSection(tokens),
            _ => throw new AssemblyException(tokens.SourcePosition, $".{dir?.Value} isn't a valid section directive. Expecting one of .globl, .data, .text.")
        };
    }

    private static void eol(BufferedTokenStream tokens) {
        if (tokens.HasNext()) {
            // Require 1 newline
            if (tokens.IsLookahead<StatementBreakToken>(0)) {
                tokens.Advance();
            } else {
                throw new AssemblyException(tokens.SourcePosition, "Missing statement break or newline.");
            }

            // Eat extra newlines
            eatNewlines(tokens);
        }
    }

    private static void eatNewlines(BufferedTokenStream tokens) {
        while (tokens.HasNext() && tokens.IsLookahead<StatementBreakToken>(0)) {
            tokens.Advance();
        }
    }

    private static T require<T>(BufferedTokenStream tokens,string type) {
        if (tokens.IsLookahead<T>(0)) {
            var maybeT = tokens.Advance();
            if (maybeT == null)
                throw new AssemblyException(tokens.SourcePosition, "Failed to fetch token from token stream.");
            if (maybeT is T tmaybe) {
                return tmaybe;
            } else {
                throw new AssemblyException(tokens.SourcePosition, "Failed to fetch token from token stream.");
            }
        } else {
            throw new AssemblyException(tokens.SourcePosition, $"Missing required {type}.");
        }
    }

    private static bool maybe<T>(BufferedTokenStream tokens) where T:Token  {
        if (tokens.IsLookahead<T>(0)) {
            var maybeT = tokens.Advance();
            if (maybeT == null || maybeT is not T)
                throw new AssemblyException(tokens.SourcePosition, "Failed to fetch token from token stream.");
            return true;
        } else {
            return false;
        }
    }
    private static bool maybe<T>(BufferedTokenStream tokens, out T token) where T:Token  {
       #nullable disable
        if (tokens.IsLookahead<T>(0)) {
            var maybeT = tokens.Advance();
            if (maybeT == null || maybeT is not T)
                throw new AssemblyException(tokens.SourcePosition, "Failed to fetch token from token stream.");
            token = (T)maybeT;
            return true;
        } else {
            token = default(T);
            return false;
        }
        #nullable restore
    }

    private Section parseGlobalSection (BufferedTokenStream tokens) {
        tokens.Advance(); // Consume the directive

        GlobalSection section = new GlobalSection();

        while (true) {
            if (!tokens.IsLookahead<IdentifierToken>(0)) {
                throw new AssemblyException(tokens.SourcePosition, "Expecting global label but none found.");
            }
            var maybeId = tokens.Advance();
            if (maybeId == null) {
                throw new AssemblyException(tokens.SourcePosition, "Failed to fetch label from token stream.");
            }
            section.Labels.Add((IdentifierToken)maybeId);

            if (tokens.IsLookahead<CommaToken>(0)) {
                continue;
            } else {
                break;
            }
        }

        eol(tokens);

        return section;
    }

    private Section parseDataSection (BufferedTokenStream tokens) {
        tokens.Advance(); // Consume the directive
        eol(tokens);

        DataSection data = new DataSection();

        while (tokens.HasNext() && !tokens.IsLookahead<DirectiveToken>(0)) {
            var @var = parseDataDirective(tokens);
            eol(tokens);
            data.Data.Add(@var);
        }

       return data; 
    }

    private Data parseDataDirective(BufferedTokenStream tokens) {
        var label = require<LabelToken>(tokens, "data label");
        
        var directive = require<DirectiveToken>(tokens, "storage class directive");
        switch (directive.Value) {
            case "byte":
            case "half":
            case "word":
                var integer = require<ScalarConstantToken>(tokens, "scalar quantity");
                if (maybe<ColonToken>(tokens)) {
                    // Is Array
                    var size = require<ScalarConstantToken>(tokens, "data length");
                    return new Data<int>(label, directive, Enumerable.Repeat(integer.IntegerValue, (int)size.IntegerValue).ToArray());
                } else if (maybe<CommaToken>(tokens)) {
                    List<int> values = new List<int>();
                    values.Add(integer.IntegerValue);
                    while (maybe<CommaToken>(tokens)) {
                        var v = require<ScalarConstantToken>(tokens, "value");
                        values.Add(v.IntegerValue);
                    }
                    return new Data<int>(label, directive, values.ToArray());
                } else {
                    // Not Array
                    return new Data<int>(label, directive, integer.IntegerValue);
                }
            case "single":
                var real = require<FloatingPointConstantToken>(tokens, "floating point quantity");
                if (maybe<ColonToken>(tokens)) {
                    // Is Array
                    var size = require<ScalarConstantToken>(tokens, "data length");
                    return new Data<float>(label, directive, Enumerable.Repeat(real.Value, (int)size.IntegerValue).ToArray());
                } else if (maybe<CommaToken>(tokens)) {
                    List<float> values = new List<float>();
                    values.Add(real.Value);
                    while (maybe<CommaToken>(tokens)) {
                        var v = require<FloatingPointConstantToken>(tokens, "value");
                        values.Add(v.Value);
                    }
                    return new Data<float>(label, directive, values.ToArray());
                } else {
                    // Not Array
                    return new Data<float>(label, directive, real.Value);
                }
            case "ascii":
                var str = require<StringConstantToken>(tokens, "ascii string");
                return new Data<byte>(label, directive, System.Text.Encoding.ASCII.GetBytes(str.Value));
            case "asciiz":
                var str2 = require<StringConstantToken>(tokens, "ascii string");
                return new Data<byte>(label, directive, System.Text.Encoding.ASCII.GetBytes(str2.Value + "\0"));
            default: 
                throw new AssemblyException(directive.Position, $"Invalid storage type '{directive.Value}'.");
        }

    }

    private Section parseTextSection (BufferedTokenStream tokens) {
        tokens.Advance(); // Consume the directive
        eol(tokens);

        TextSection section = new TextSection();
        while (tokens.HasNext() && !tokens.IsLookahead<DirectiveToken>(0)) {
            foreach (var instr in parseInstruction(tokens)) {
                section.Code.Add(instr);
            }
        }

        return section;
    }

    /// <summary>
    /// Count of all supported instructions by this parser
    /// </summary>
    /// <returns>count</returns>
    public static int CountSupportedAssemblyInstructions() => decoders.Count;
    /// <summary>
    /// Count of all instructions in this assembly
    /// </summary>
    /// <returns>count</returns>
    public static int CountAllAssemblyInstructions() =>
        typeof(Parser)
        .Assembly
        .GetTypes()
        .Where(type => type.IsClass && !type.IsAbstract && type.IsAssignableTo(typeof(IAssemblyInstruction)))
        .Count();

    private static List<TryDecodeAssembly> decoders = new List<TryDecodeAssembly> {
        #region Arithmetic & Logical
        //Bytecode.AbsS.TryDecodeAssembly,
        Bytecode.Add.TryDecodeAssembly,
        Bytecode.AddS.TryDecodeAssembly,
        Bytecode.Addi.TryDecodeAssembly,
        Bytecode.Addiu.TryDecodeAssembly,
        Bytecode.Addu.TryDecodeAssembly,
        Bytecode.And.TryDecodeAssembly,
        //Bytecode.Andi.TryDecodeAssembly,
        //Bytecode.Div.TryDecodeAssembly,
        //Bytecode.DivS.TryDecodeAssembly,
        //Bytecode.Divu.TryDecodeAssembly,
        //Bytecode.MulS.TryDecodeAssembly,
        //Bytecode.Mult.TryDecodeAssembly,
        //Bytecode.Multu.TryDecodeAssembly,
        Bytecode.Nor.TryDecodeAssembly,
        Bytecode.Or.TryDecodeAssembly,
        //Bytecode.Ori.TryDecodeAssembly,
        //Bytecode.Sllv.TryDecodeAssembly,
        //Bytecode.Srlv.TryDecodeAssembly,
        Bytecode.Sub.TryDecodeAssembly,
        //Bytecode.SubS.TryDecodeAssembly,
        //Bytecode.Subu.TryDecodeAssembly,
        Bytecode.Xor.TryDecodeAssembly,
        //Bytecode.Xori.TryDecodeAssembly,
        #endregion
        #region Branch
        //Bytecode.Beq.TryDecodeAssembly,
        //Bytecode.Bgtz.TryDecodeAssembly,
        //Bytecode.Blez.TryDecodeAssembly,
        //Bytecode.Bne.TryDecodeAssembly,
        #endregion
        #region Comparison
        //Bytecode.Slt.TryDecodeAssembly,
        //Bytecode.Slti.TryDecodeAssembly,
        //Bytecode.Sltiu.TryDecodeAssembly,
        //Bytecode.Sltu.TryDecodeAssembly,
        #endregion
        #region Constant Manipulator
        //Bytecode.Lhi.TryDecodeAssembly,
        //Bytecode.Llo.TryDecodeAssembly,
        #endregion
        #region Data Movement
        //Bytecode.Mfc1.TryDecodeAssembly,
        //Bytecode.Mfhi.TryDecodeAssembly,
        //Bytecode.Mflo.TryDecodeAssembly,
        //Bytecode.Mtc1.TryDecodeAssembly,
        //Bytecode.Mthi.TryDecodeAssembly,
        //Bytecode.Mtlo.TryDecodeAssembly,
        #endregion
        #region Exception and Interrupts
        //Bytecode.Syscall.TryDecodeAssembly,
        #endregion
        #region Jump
        //Bytecode.J.TryDecodeAssembly,
        //Bytecode.Jal.TryDecodeAssembly,
        //Bytecode.Jalr.TryDecodeAssembly,
        //Bytecode.Jr.TryDecodeAssembly,
        #endregion
        #region Load
        //Bytecode.Lb.TryDecodeAssembly,
        //Bytecode.Lbu.TryDecodeAssembly,
        //Bytecode.Lh.TryDecodeAssembly,
        //Bytecode.Lhu.TryDecodeAssembly,
        //Bytecode.Lw.TryDecodeAssembly,
        //Bytecode.Lwc1.TryDecodeAssembly,
        #endregion
        #region  Store
        //Bytecode.Sb.TryDecodeAssembly,
        //Bytecode.Sh.TryDecodeAssembly,
        //Bytecode.Sw.TryDecodeAssembly,
        //Bytecode.Swc1.TryDecodeAssembly,
        #endregion
    };


    private IEnumerable<IAssemblyInstruction> parseInstruction(BufferedTokenStream tokens) {
        // Label (can be on its own line)
        if (tokens.IsLookahead<LabelToken>(0)) {
            var label = require<LabelToken>(tokens, "code label");
            yield return new LabelMarker(label.Value);
        }

        // Operation
        IdentifierToken opcode;
        List<Token> args = new List<Token>();
        if (maybe<IdentifierToken>(tokens, out opcode)) {
            // Got the opcode
    
            // Get the operands
            args.Clear();
            while (tokens.HasNext() && !tokens.IsLookahead<StatementBreakToken>(0)) {
                var tok = tokens.Advance();
                if (tok != null)
                    args.Add(tok);
            }        
            
            // Decode
            IAssemblyInstruction? instr = null;
            foreach (var decoder in decoders) {
                if (decoder(opcode, args, out instr)) {
                    if (instr != null) {
                        break;
                    }
                }
            }
            if (instr == null) {
                // Couldn't find a decoder that worked
                throw new AssemblyException(opcode.Position, $"Unknown operation '{opcode.Value}'."); 
            } 

            // We decoded an instruction successfully
            yield return instr;
            
            // Close
            eol(tokens);
        }
    }

    private static T parseNoResultOp<Lhs,Rhs, T>(BufferedTokenStream tokens, Func<Lhs, Rhs, T> convert) where Lhs:Token where Rhs:Token where T:IAssemblyInstruction {
        var lhs = require<Lhs>(tokens, "left-hand operand");
        require<CommaToken>(tokens, "comma");
        var rhs = require<Rhs>(tokens, "right-hand operand");
        return convert(lhs, rhs);
    }

    private static T parseOp<Lhs,Rhs, T>(BufferedTokenStream tokens, Func<RegisterToken, Lhs, Rhs, T> convert) where Lhs:Token where Rhs:Token where T:IAssemblyInstruction {
        var res = require<RegisterToken>(tokens, "result register");
        require<CommaToken>(tokens, "comma");
        var lhs = require<Lhs>(tokens, "left-hand operand");
        require<CommaToken>(tokens, "comma");
        var rhs = require<Rhs>(tokens, "right-hand operand");
        return convert(res, lhs, rhs);
    }

    private AddSigned parseAdd(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, AddSigned>(
        tokens, 
        (res, lhs, rhs) => new AddSigned {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 

    private SubtractSigned parseSub(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, SubtractSigned>(
        tokens, 
        (res, lhs, rhs) => new SubtractSigned {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 


    private AddSignedImmediate parseAddi(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, AddSignedImmediate>(
        tokens, 
        (res, lhs, rhs) => new AddSignedImmediate {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperand = (int)rhs.IntegerValue,
        }); 

    private SubtractSignedImmediate parseSubi(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, SubtractSignedImmediate>(
        tokens, 
        (res, lhs, rhs) => new SubtractSignedImmediate {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperand = (int)rhs.IntegerValue,
        }); 

    private AddUnsigned parseAddu(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, AddUnsigned>(
        tokens, 
        (res, lhs, rhs) => new AddUnsigned {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 

    private SubtractUnsigned parseSubu(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, SubtractUnsigned>(
        tokens, 
        (res, lhs, rhs) => new SubtractUnsigned {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 

    private AddUnsignedImmediate parseAddiu(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, AddUnsignedImmediate>(
        tokens, 
        (res, lhs, rhs) => new AddUnsignedImmediate {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperand = (uint)rhs.IntegerValue,
        }); 

    private MultiplySignedWithOverflow parseMult(BufferedTokenStream tokens) => parseNoResultOp<RegisterToken, RegisterToken, MultiplySignedWithOverflow>(
        tokens, 
        (lhs, rhs) => new MultiplySignedWithOverflow {
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 
    private MultiplyUnsignedWithOverflow parseMultu(BufferedTokenStream tokens) => parseNoResultOp<RegisterToken, RegisterToken, MultiplyUnsignedWithOverflow>(
        tokens, 
        (lhs, rhs) => new MultiplyUnsignedWithOverflow {
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 

    private DivideSignedWithRemainder parseDiv(BufferedTokenStream tokens) => parseNoResultOp<RegisterToken, RegisterToken, DivideSignedWithRemainder>(
        tokens, 
        (lhs, rhs) => new DivideSignedWithRemainder {
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 

    private DivideUnsignedWithRemainder parseDivu(BufferedTokenStream tokens) => parseNoResultOp<RegisterToken, RegisterToken, DivideUnsignedWithRemainder>(
        tokens, 
        (lhs, rhs) => new DivideUnsignedWithRemainder {
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 
        
    private And parseAnd(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, And>(
        tokens, 
        (res, lhs, rhs) => new And {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 

    private Or parseOr(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, Or>(
        tokens, 
        (res, lhs, rhs) => new Or {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 

    private Nor parseNor(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, Nor>(
        tokens, 
        (res, lhs, rhs) => new Nor {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 

    private Xor parseXor(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, Xor>(
        tokens, 
        (res, lhs, rhs) => new Xor {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        });

    private AndImmediate parseAndi(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, AndImmediate>(
        tokens, 
        (res, lhs, rhs) => new AndImmediate {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperand = (uint)rhs.IntegerValue,
        }); 
        
    private OrImmediate parseOri(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, OrImmediate>(
        tokens, 
        (res, lhs, rhs) => new OrImmediate {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperand = (uint)rhs.IntegerValue,
        }); 

    private XorImmediate parseXori(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, XorImmediate>(
        tokens, 
        (res, lhs, rhs) => new XorImmediate {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperand = (uint)rhs.IntegerValue,
        }); 

    private ShiftLeftLogical parseSllv(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, ShiftLeftLogical>(
        tokens, 
        (res, lhs, rhs) => new ShiftLeftLogical {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 

    private ShiftRightLogical parseSrlv(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, ShiftRightLogical>(
        tokens, 
        (res, lhs, rhs) => new ShiftRightLogical {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        });

    private LoadWord parseLw(BufferedTokenStream tokens) {
        var res = require<RegisterToken>(tokens, "result register");
        require<CommaToken>(tokens, "comma");
        var offset = require<ScalarConstantToken>(tokens, "memory offset");
        require<OpenParenthesisToken>(tokens, "open parenthesis");
        var root = require<RegisterToken>(tokens, "base register");
        require<CloseParenthesisToken>(tokens, "close parenthesis");
        return new LoadWord {
            ResultRegister = res.Value,
            BaseRegister = root.Value,
            Offset = (uint)offset.IntegerValue
        };
    }

    private StoreWord parseSw(BufferedTokenStream tokens) {
        var res = require<RegisterToken>(tokens, "source register");
        require<CommaToken>(tokens, "comma");
        var offset = require<ScalarConstantToken>(tokens, "memory offset");
        require<OpenParenthesisToken>(tokens, "open parenthesis");
        var root = require<RegisterToken>(tokens, "base register");
        require<CloseParenthesisToken>(tokens, "close parenthesis");
        return new StoreWord {
            SourceRegister = res.Value,
            BaseRegister = root.Value,
            Offset = (uint)offset.IntegerValue
        };
    }

    private LoadUpperImmediate parseLui(BufferedTokenStream tokens) {
        var res = require<RegisterToken>(tokens, "result register");
        require<CommaToken>(tokens, "comma");
        var label = require<ScalarConstantToken>(tokens, "immediate value");
        return new LoadUpperImmediate {
            ResultRegister = res.Value,
            Constant = (uint)label.IntegerValue
        };
    }

    private LoadAddress parseLa(BufferedTokenStream tokens) {
        var res = require<RegisterToken>(tokens, "result register");
        require<CommaToken>(tokens, "comma");
        var label = require<IdentifierToken>(tokens, "label identifier");
        return new LoadAddress {
            ResultRegister = res.Value,
            Label = label.Value
        };
    }

    private LoadImmediate parseLi(BufferedTokenStream tokens) {
        var res = require<RegisterToken>(tokens, "result register");
        require<CommaToken>(tokens, "comma");
        var label = require<ScalarConstantToken>(tokens, "immediate value");
        return new LoadImmediate {
            ResultRegister = res.Value,
            Constant = (uint)label.IntegerValue
        };
    }

    private MoveFromHi parseMfhi(BufferedTokenStream tokens) {
        var res = require<RegisterToken>(tokens, "result register");
        return new MoveFromHi {
            ResultRegister = res.Value
        };
    }

    private MoveFromLo parseMflo(BufferedTokenStream tokens) {
        var res = require<RegisterToken>(tokens, "result register");
        return new MoveFromLo {
            ResultRegister = res.Value
        };
    }

    private Move parseMove(BufferedTokenStream tokens) {
        var res = require<RegisterToken>(tokens, "result register");
        require<CommaToken>(tokens, "comma");
        var from = require<RegisterToken>(tokens, "source register");
        return new Move {
            ResultRegister = res.Value,
            SourceRegister = from.Value
        };
    }

    private BranchOnEqual parseBeq(BufferedTokenStream tokens) => parseOp<RegisterToken, AddressLikeToken, BranchOnEqual>(
        tokens, 
        (res, lhs, rhs) => new BranchOnEqual {
            LhsOperandRegister = res.Value,
            RhsOperandRegister = lhs.Value,
            Address = rhs
        });

    private BranchOnNotEqual parseBne(BufferedTokenStream tokens) => parseOp<RegisterToken, AddressLikeToken, BranchOnNotEqual>(
        tokens, 
        (res, lhs, rhs) => new BranchOnNotEqual {
            LhsOperandRegister = res.Value,
            RhsOperandRegister = lhs.Value,
            Address = rhs
        });

    private BranchOnGreater parseBgt(BufferedTokenStream tokens) => parseOp<RegisterToken, AddressLikeToken, BranchOnGreater>(
        tokens, 
        (res, lhs, rhs) => new BranchOnGreater {
            LhsOperandRegister = res.Value,
            RhsOperandRegister = lhs.Value,
            Address = rhs
        });

    private BranchOnGreaterOrEqual parseBge(BufferedTokenStream tokens) => parseOp<RegisterToken, AddressLikeToken, BranchOnGreaterOrEqual>(
        tokens, 
        (res, lhs, rhs) => new BranchOnGreaterOrEqual {
            LhsOperandRegister = res.Value,
            RhsOperandRegister = lhs.Value,
            Address = rhs
        });

    private BranchOnLess parseBlt(BufferedTokenStream tokens) => parseOp<RegisterToken, AddressLikeToken, BranchOnLess>(
        tokens, 
        (res, lhs, rhs) => new BranchOnLess {
            LhsOperandRegister = res.Value,
            RhsOperandRegister = lhs.Value,
            Address = rhs
        });

    private BranchOnLessOrEqual parseBle(BufferedTokenStream tokens) => parseOp<RegisterToken, AddressLikeToken, BranchOnLessOrEqual>(
        tokens, 
        (res, lhs, rhs) => new BranchOnLessOrEqual {
            LhsOperandRegister = res.Value,
            RhsOperandRegister = lhs.Value,
            Address = rhs
        });

    private SetOnLessThan parseSlt(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, SetOnLessThan>(
        tokens, 
        (res, lhs, rhs) => new SetOnLessThan {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value
        });

    private SetOnLessThanImmediate parseSlti(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, SetOnLessThanImmediate>(
        tokens, 
        (res, lhs, rhs) => new SetOnLessThanImmediate {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            Constant = rhs.IntegerValue
        }); 

    private JumpTo parseJ(BufferedTokenStream tokens) {
        var from = require<AddressLikeToken>(tokens, "target address");
        return new JumpTo {
            Address = from
        };
    }

    private JumpRegister parseJr(BufferedTokenStream tokens) {
        var from = require<RegisterToken>(tokens, "target address register");
        return new JumpRegister {
            Register = from.Value
        };
    }

    private JumpAndLink parseJal(BufferedTokenStream tokens) {
        var from = require<AddressLikeToken>(tokens, "target address");
        return new JumpAndLink {
            Address = from
        };
    }

    private BranchGreaterThan0 parseBgtz(BufferedTokenStream tokens) {
        var reg = require<RegisterToken>(tokens, "register");
        require<CommaToken>(tokens, "comma");
        var from = require<AddressLikeToken>(tokens, "target address");
        return new BranchGreaterThan0 {
            LhsOperandRegister = reg.Value,
            Address = from
        };
    }

    private BranchLessThanOrEqual0 parseBlez(BufferedTokenStream tokens) {
        var reg = require<RegisterToken>(tokens, "register");
        require<CommaToken>(tokens, "comma");
        var from = require<AddressLikeToken>(tokens, "target address");
        return new BranchLessThanOrEqual0 {
            LhsOperandRegister = reg.Value,
            Address = from
        };
    }

    private LoadIntoCoprocessor1 parselwc1(BufferedTokenStream tokens) {
        var res = require<RegisterToken>(tokens, "result register");
        require<CommaToken>(tokens, "comma");
        var offset = require<ScalarConstantToken>(tokens, "memory offset");
        require<OpenParenthesisToken>(tokens, "open parenthesis");
        var root = require<RegisterToken>(tokens, "base register");
        require<CloseParenthesisToken>(tokens, "close parenthesis");
        return new LoadIntoCoprocessor1 {
            ResultRegister = res.Value,
            BaseRegister = root.Value,
            Offset = (uint)offset.IntegerValue
        };
    }

    private StoreFromCoprocessor1 parseswc1(BufferedTokenStream tokens) {
        var res = require<RegisterToken>(tokens, "source register");
        require<CommaToken>(tokens, "comma");
        var offset = require<ScalarConstantToken>(tokens, "memory offset");
        require<OpenParenthesisToken>(tokens, "open parenthesis");
        var root = require<RegisterToken>(tokens, "base register");
        require<CloseParenthesisToken>(tokens, "close parenthesis");
        return new StoreFromCoprocessor1 {
            SourceRegister = res.Value,
            BaseRegister = root.Value,
            Offset = (uint)offset.IntegerValue
        };
    }

    private MoveToCoprocessor1 parsemtc1(BufferedTokenStream tokens) {
        var cpu = require<RegisterToken>(tokens, "cpu register");
        require<CommaToken>(tokens, "comma");
        var fpu = require<RegisterToken>(tokens, "fpu register");
        return new MoveToCoprocessor1 {
            CpuRegister = cpu.Value,
            FpuRegister = fpu.Value
        };
    }

    private MoveFromCoprocessor1 parsemfc1(BufferedTokenStream tokens) {
        var cpu = require<RegisterToken>(tokens, "cpu register");
        require<CommaToken>(tokens, "comma");
        var fpu = require<RegisterToken>(tokens, "fpu register");
        return new MoveFromCoprocessor1 {
            CpuRegister = cpu.Value,
            FpuRegister = fpu.Value
        };
    }

    private AbsoluteValueSingle parseabsS(BufferedTokenStream tokens) {
        var dest = require<RegisterToken>(tokens, "result register");
        require<CommaToken>(tokens, "comma");
        var src = require<RegisterToken>(tokens, "source register");
        return new AbsoluteValueSingle {
            ResultRegister = dest.Value,
            SourceRegister = src.Value
        };
    }
    private AddSingle parseaddS(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, AddSingle>(
        tokens, 
        (res, lhs, rhs) => new AddSingle {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 
    private SubtractSingle parsesubS(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, SubtractSingle>(
        tokens, 
        (res, lhs, rhs) => new SubtractSingle {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 
    private MultiplySingle parsemulS(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, MultiplySingle>(
        tokens, 
        (res, lhs, rhs) => new MultiplySingle {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 
    private DivideSingle parsedivS(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, DivideSingle>(
        tokens, 
        (res, lhs, rhs) => new DivideSingle {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 
}
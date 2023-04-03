using System.IO;
using System.Text;
using System.Collections.Generic;
using Qkmaxware.Compiling.Mips.Assembly;

namespace Qkmaxware.Compiling.Mips.Assembly;

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
                // TODO comma separated literal list
                if (maybe<ColonToken>(tokens)) {
                    // Is Array
                    var size = require<ScalarConstantToken>(tokens, "data length");
                    return new Data<int>(label, directive, Enumerable.Repeat(integer.Value, (int)size.Value).ToArray());
                } else if (maybe<CommaToken>(tokens)) {
                    List<int> values = new List<int>();
                    values.Add(integer.Value);
                    while (maybe<CommaToken>(tokens)) {
                        var v = require<ScalarConstantToken>(tokens, "value");
                        values.Add(v.Value);
                    }
                    return new Data<int>(label, directive, values.ToArray());
                } else {
                    // Not Array
                    return new Data<int>(label, directive, integer.Value);
                }
            case "float":
                var real = require<FloatingPointConstantToken>(tokens, "floating point quantity");
                if (maybe<ColonToken>(tokens)) {
                    // Is Array
                    var size = require<ScalarConstantToken>(tokens, "data length");
                    return new Data<float>(label, directive, Enumerable.Repeat(real.Value, (int)size.Value).ToArray());
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

    private IEnumerable<IAssemblyInstruction> parseInstruction(BufferedTokenStream tokens) {
        // Label (can be on its own line)
        if (tokens.IsLookahead<LabelToken>(0)) {
            var label = require<LabelToken>(tokens, "code label");
            yield return new LabelMarker(label.Value);
        }

        // Operation
        IdentifierToken opcode;
        if (maybe<IdentifierToken>(tokens, out opcode)) {

            // Decode
            switch (opcode.Value) {
                // Arithmetic 
                case "add":
                    yield return parseAdd(tokens);
                    break;
                case "sub":
                    yield return parseSub(tokens);
                    break;
                case "addi":
                    yield return parseAddi(tokens);
                    break;
                case "subi":
                    yield return parseSubi(tokens);
                    break;
                case "addu":
                    yield return parseAddu(tokens);
                    break;
                case "subu":
                    yield return parseSubu(tokens);
                    break;
                case "addiu":
                    yield return parseAddiu(tokens);
                    break;
                case "mul":
                    yield return parseMul(tokens);
                    break;
                case "mult":
                    yield return parseMult(tokens);
                    break;
                case "div":
                    yield return parseDiv(tokens);
                    break;

                // Logical
                case "and":
                    yield return parseAnd(tokens);
                    break;
                case "or":
                    yield return parseOr(tokens);
                    break;
                case "andi":
                    yield return parseAndi(tokens);
                    break;
                case "ori":
                    yield return parseOri(tokens);
                    break;
                case "sll":
                    yield return parseSll(tokens);
                    break;
                case "srl":
                    yield return parseSrl(tokens);
                    break;

                // Data Transfer
                case "lw":
                    yield return parseLw(tokens);
                    break;
                case "sw":
                    yield return parseSw(tokens);
                    break;
                case "lui":
                    yield return parseLui(tokens);
                    break;
                case "la":
                    yield return parseLa(tokens);
                    break;
                case "li":
                    yield return parseLi(tokens);
                    break;
                case "mfhi":
                    yield return parseMfhi(tokens);
                    break;
                case "mflo":
                    yield return parseMflo(tokens);
                    break;
                case "move":
                    yield return parseMove(tokens);
                    break;

                // Conditional Branch
                case "beq":
                    yield return parseBeq(tokens);
                    break;
                case "bgtz":
                    yield return parseBgtz(tokens);
                    break;
                case "blez":
                    yield return parseBlez(tokens);
                    break;
                case "bne":
                    yield return parseBne(tokens);
                    break;
                case "bgt":
                    yield return parseBgt(tokens);
                    break;
                case "bge":
                    yield return parseBge(tokens);
                    break;
                case "blt":
                    yield return parseBlt(tokens);
                    break;
                case "ble":
                    yield return parseBle(tokens);
                    break;

                // Comparison
                case "slt":
                    yield return parseSlt(tokens);
                    break;
                case "slti":
                    yield return parseSlti(tokens);
                    break;

                // Unconditional Jump
                case "j":
                    yield return parseJ(tokens);
                    break;
                case "jr":
                    yield return parseJr(tokens);
                    break;
                case "jal":
                    yield return parseJal(tokens);
                    break;

                // System Calls
                case "syscall":
                    // System call behavior changes depending on the value in specific registers, not encoded onto operation
                    yield return new Syscall();
                    break;

                // Else
                default:
                    throw new AssemblyException(opcode.Position, $"Unknown operation '{opcode.Value}'."); 
            }

        }
        eol(tokens);
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
            RhsOperand = (int)rhs.Value,
        }); 

    private SubtractSignedImmediate parseSubi(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, SubtractSignedImmediate>(
        tokens, 
        (res, lhs, rhs) => new SubtractSignedImmediate {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperand = (int)rhs.Value,
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
            RhsOperand = (uint)rhs.Value,
        }); 

    private MultiplyWithoutOverflow parseMul(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, MultiplyWithoutOverflow>(
        tokens, 
        (res, lhs, rhs) => new MultiplyWithoutOverflow {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 
    private MultiplyWithOverflow parseMult(BufferedTokenStream tokens) => parseNoResultOp<RegisterToken, RegisterToken, MultiplyWithOverflow>(
        tokens, 
        (lhs, rhs) => new MultiplyWithOverflow {
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 

    private DivideWithRemainder parseDiv(BufferedTokenStream tokens) => parseNoResultOp<RegisterToken, RegisterToken, DivideWithRemainder>(
        tokens, 
        (lhs, rhs) => new DivideWithRemainder {
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

    private AndImmediate parseAndi(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, AndImmediate>(
        tokens, 
        (res, lhs, rhs) => new AndImmediate {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperand = (uint)rhs.Value,
        }); 
        
    private OrImmediate parseOri(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, OrImmediate>(
        tokens, 
        (res, lhs, rhs) => new OrImmediate {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperand = (uint)rhs.Value,
        }); 

    private ShiftLeftLogical parseSll(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, ShiftLeftLogical>(
        tokens, 
        (res, lhs, rhs) => new ShiftLeftLogical {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperandRegister = rhs.Value,
        }); 

    private ShiftRightLogical parseSrl(BufferedTokenStream tokens) => parseOp<RegisterToken, RegisterToken, ShiftRightLogical>(
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
            Offset = (uint)offset.Value
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
            Offset = (uint)offset.Value
        };
    }

    private LoadUpperImmediate parseLui(BufferedTokenStream tokens) {
        var res = require<RegisterToken>(tokens, "result register");
        require<CommaToken>(tokens, "comma");
        var label = require<ScalarConstantToken>(tokens, "immediate value");
        return new LoadUpperImmediate {
            ResultRegister = res.Value,
            Constant = (uint)label.Value
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
            Constant = (uint)label.Value
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

    private BranchOnEqual parseBeq(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, BranchOnEqual>(
        tokens, 
        (res, lhs, rhs) => new BranchOnEqual {
            LhsOperandRegister = res.Value,
            RhsOperandRegister = lhs.Value,
            Offset = rhs.Value
        });

    private BranchOnNotEqual parseBne(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, BranchOnNotEqual>(
        tokens, 
        (res, lhs, rhs) => new BranchOnNotEqual {
            LhsOperandRegister = res.Value,
            RhsOperandRegister = lhs.Value,
            Offset = rhs.Value
        });

    private BranchOnGreater parseBgt(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, BranchOnGreater>(
        tokens, 
        (res, lhs, rhs) => new BranchOnGreater {
            LhsOperandRegister = res.Value,
            RhsOperandRegister = lhs.Value,
            Offset = rhs.Value
        });

    private BranchOnGreaterOrEqual parseBge(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, BranchOnGreaterOrEqual>(
        tokens, 
        (res, lhs, rhs) => new BranchOnGreaterOrEqual {
            LhsOperandRegister = res.Value,
            RhsOperandRegister = lhs.Value,
            Offset = rhs.Value
        });

    private BranchOnLess parseBlt(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, BranchOnLess>(
        tokens, 
        (res, lhs, rhs) => new BranchOnLess {
            LhsOperandRegister = res.Value,
            RhsOperandRegister = lhs.Value,
            Offset = rhs.Value
        });

    private BranchOnLessOrEqual parseBle(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, BranchOnLessOrEqual>(
        tokens, 
        (res, lhs, rhs) => new BranchOnLessOrEqual {
            LhsOperandRegister = res.Value,
            RhsOperandRegister = lhs.Value,
            Offset = rhs.Value
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
            Constant = rhs.Value
        }); 

    private JumpTo parseJ(BufferedTokenStream tokens) {
        var from = require<IAddressLike>(tokens, "target address");
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
        var from = require<IAddressLike>(tokens, "target address");
        return new JumpAndLink {
            Address = from
        };
    }

    private BranchGreaterThan0 parseBgtz(BufferedTokenStream tokens) {
        var reg = require<RegisterToken>(tokens, "register");
        require<CommaToken>(tokens, "comma");
        var from = require<IAddressLike>(tokens, "target address");
        return new BranchGreaterThan0 {
            LhsOperandRegister = reg.Value,
            Address = from
        };
    }

    private BranchLessThanOrEqual0 parseBlez(BufferedTokenStream tokens) {
        var reg = require<RegisterToken>(tokens, "register");
        require<CommaToken>(tokens, "comma");
        var from = require<IAddressLike>(tokens, "target address");
        return new BranchLessThanOrEqual0 {
            LhsOperandRegister = reg.Value,
            Address = from
        };
    }
}
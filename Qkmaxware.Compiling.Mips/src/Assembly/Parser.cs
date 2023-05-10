using System.IO;
using System.Text;
using System.Collections.Generic;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

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
            var instr = parseInstruction(tokens);
            if (instr != null) {
                section.Code.Add(instr);
            } else {
                break;
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
        #region Pseudo
        Assembly.Instructions.Li.TryDecodeAssembly,
        Assembly.Instructions.La.TryDecodeAssembly,
        Assembly.Instructions.Move.TryDecodeAssembly,
        #endregion
        #region Arithmetic & Logical
        Bytecode.Instructions.AbsS.TryDecodeAssembly,
        Bytecode.Instructions.Add.TryDecodeAssembly,
        Bytecode.Instructions.AddS.TryDecodeAssembly,
        Bytecode.Instructions.Addi.TryDecodeAssembly,
        Bytecode.Instructions.Addiu.TryDecodeAssembly,
        Bytecode.Instructions.Addu.TryDecodeAssembly,
        Bytecode.Instructions.And.TryDecodeAssembly,
        Bytecode.Instructions.Andi.TryDecodeAssembly,
        Bytecode.Instructions.Div.TryDecodeAssembly,
        Bytecode.Instructions.DivS.TryDecodeAssembly,
        Bytecode.Instructions.Divu.TryDecodeAssembly,
        Bytecode.Instructions.MulS.TryDecodeAssembly,
        Bytecode.Instructions.Mult.TryDecodeAssembly,
        Bytecode.Instructions.Multu.TryDecodeAssembly,
        Bytecode.Instructions.Nor.TryDecodeAssembly,
        Bytecode.Instructions.Or.TryDecodeAssembly,
        Bytecode.Instructions.Ori.TryDecodeAssembly,
        Bytecode.Instructions.Sllv.TryDecodeAssembly,
        Bytecode.Instructions.Srlv.TryDecodeAssembly,
        Bytecode.Instructions.Sub.TryDecodeAssembly,
        Bytecode.Instructions.SubS.TryDecodeAssembly,
        Bytecode.Instructions.Subu.TryDecodeAssembly,
        Bytecode.Instructions.Xor.TryDecodeAssembly,
        Bytecode.Instructions.Xori.TryDecodeAssembly,
        #endregion
        #region Branch
        Bytecode.Instructions.Beq.TryDecodeAssembly,
        Bytecode.Instructions.Bgtz.TryDecodeAssembly,
        Bytecode.Instructions.Blez.TryDecodeAssembly,
        Bytecode.Instructions.Bne.TryDecodeAssembly,
        #endregion
        #region Comparison
        Bytecode.Instructions.Slt.TryDecodeAssembly,
        Bytecode.Instructions.Slti.TryDecodeAssembly,
        Bytecode.Instructions.Sltiu.TryDecodeAssembly,
        Bytecode.Instructions.Sltu.TryDecodeAssembly,
        #endregion
        #region Constant Manipulator
        Bytecode.Instructions.Lui.TryDecodeAssembly,
        #endregion
        #region Data Movement
        Bytecode.Instructions.Mfc1.TryDecodeAssembly,
        Bytecode.Instructions.Mfhi.TryDecodeAssembly,
        Bytecode.Instructions.Mflo.TryDecodeAssembly,
        Bytecode.Instructions.Mtc1.TryDecodeAssembly,
        Bytecode.Instructions.Mthi.TryDecodeAssembly,
        Bytecode.Instructions.Mtlo.TryDecodeAssembly,
        #endregion
        #region Exception and Interrupts
        Bytecode.Instructions.Nop.TryDecodeAssembly,
        Bytecode.Instructions.Syscall.TryDecodeAssembly,
        #endregion
        #region Jump
        //Bytecode.Instructions.J.TryDecodeAssembly,
        //Bytecode.Instructions.Jal.TryDecodeAssembly,
        //Bytecode.Instructions.Jalr.TryDecodeAssembly,
        //Bytecode.Instructions.Jr.TryDecodeAssembly,
        #endregion
        #region Load
        Bytecode.Instructions.Lb.TryDecodeAssembly,
        Bytecode.Instructions.Lbu.TryDecodeAssembly,
        Bytecode.Instructions.Lh.TryDecodeAssembly,
        Bytecode.Instructions.Lhu.TryDecodeAssembly,
        Bytecode.Instructions.Lw.TryDecodeAssembly,
        Bytecode.Instructions.Lwc1.TryDecodeAssembly,
        #endregion
        #region  Store
        Bytecode.Instructions.Sb.TryDecodeAssembly,
        Bytecode.Instructions.Sh.TryDecodeAssembly,
        Bytecode.Instructions.Sw.TryDecodeAssembly,
        Bytecode.Instructions.Swc1.TryDecodeAssembly,
        #endregion
    };


    private IAssemblyInstruction? parseInstruction(BufferedTokenStream tokens) {
        eatNewlines(tokens);

        // Label (can be on its own line)
        if (tokens.IsLookahead<LabelToken>(0)) {
            var label = require<LabelToken>(tokens, "code label");
            return new Instructions.Label(label.Value);
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

            // Close
            eol(tokens);

            // We decoded an instruction successfully
            return instr;
        }

        return null;
    } 
}
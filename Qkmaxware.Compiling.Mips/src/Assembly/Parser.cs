using System.IO;
using System.Text;
using System.Collections.Generic;
using Qkmaxware.Compiling.Mips.InstructionSet;

namespace Qkmaxware.Compiling.Mips.Assembly;

public class Parser {
    public AssemblyProgram Parse(IEnumerable<Token> tokens) {
        var stream = new BufferedTokenStream(tokens);
        return parseProgram(stream);
    }

    private AssemblyProgram parseProgram(BufferedTokenStream tokens) {
        AssemblyProgram program = new AssemblyProgram();

        while (tokens.HasNext()) {
            program.Sections.Add(parseSection(tokens));
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
            while (tokens.HasNext() && tokens.IsLookahead<StatementBreakToken>(0)) {
                tokens.Advance();
            }
        }
    }

    private static T require<T>(BufferedTokenStream tokens,string type) where T:Token  {
        if (tokens.IsLookahead<T>(0)) {
            var maybeT = tokens.Advance();
            if (maybeT == null || maybeT is not T)
                throw new AssemblyException(tokens.SourcePosition, "Failed to fetch token from token stream.");
            return (T)maybeT;
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
                    return new Data<uint>(label, directive, Enumerable.Repeat(integer.Value, (int)size.Value).ToArray());
                } else {
                    // Not Array
                    return new Data<uint>(label, directive, integer.Value);
                }
            case "float":
                var real = require<FloatingPointConstantToken>(tokens, "floating point quantity");
                if (maybe<ColonToken>(tokens)) {
                    // Is Array
                    var size = require<ScalarConstantToken>(tokens, "data length");
                    return new Data<float>(label, directive, Enumerable.Repeat(real.Value, (int)size.Value).ToArray());
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

    private IEnumerable<IAssembleable> parseInstruction(BufferedTokenStream tokens) {
        // Label (can be on its own line)
        if (tokens.IsLookahead<LabelToken>(0)) {
            var label = require<LabelToken>(tokens, "code label");
            // TODO return a label instruction
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

                // Data Transfer

                // Conditional Branch

                // Comparison

                // Unconditional Jump

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

    private static T parseNoResultOp<Lhs,Rhs, T>(BufferedTokenStream tokens, Func<Lhs, Rhs, T> convert) where Lhs:Token where Rhs:Token where T:IAssembleable {
        var lhs = require<Lhs>(tokens, "left-hand operand");
        require<CommaToken>(tokens, "comma");
        var rhs = require<Rhs>(tokens, "right-hand operand");
        return convert(lhs, rhs);
    }

    private static T parseOp<Lhs,Rhs, T>(BufferedTokenStream tokens, Func<RegisterToken, Lhs, Rhs, T> convert) where Lhs:Token where Rhs:Token where T:IAssembleable {
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
            RhsOperand = rhs.Value,
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
            RhsOperand = rhs.Value,
        }); 
        
    private OrImmediate parseOri(BufferedTokenStream tokens) => parseOp<RegisterToken, ScalarConstantToken, OrImmediate>(
        tokens, 
        (res, lhs, rhs) => new OrImmediate {
            ResultRegister = res.Value,
            LhsOperandRegister = lhs.Value,
            RhsOperand = rhs.Value,
        }); 
}
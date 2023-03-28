using System.IO;
using System.Text;
using System.Collections.Generic;

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
            if (tokens.IsLookahead<StatementBreakToken>(0)) {
                tokens.Advance();
            } else {
                throw new AssemblyException(tokens.SourcePosition, "Missing statement break or newline.");
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
            case "word":
                var integer = require<ScalarConstantToken>(tokens, "scalar quantity");
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
            case "asciiz":
                var str = require<StringConstantToken>(tokens, "ascii string");
                return new Data<byte>(label, directive, System.Text.Encoding.ASCII.GetBytes(str.Value));
            default: 
                throw new AssemblyException(directive.Position, $"Invalid storage type {directive.Value}.");
        }

    }

    private Section parseTextSection (BufferedTokenStream tokens) {
        tokens.Advance(); // Consume the directive
        eol(tokens);

        TextSection section = new TextSection();
        while (tokens.HasNext() && !tokens.IsLookahead<DirectiveToken>(0)) {
            var instrs = parseInstruction(tokens);
            //section.Code.AddRange(instrs);
        }

        return section;
    }

    private IEnumerable<object> parseInstruction(BufferedTokenStream tokens) {
        // Label (can be on its own line)
        if (tokens.IsLookahead<LabelToken>(0)) {
            var label = require<LabelToken>(tokens, "code label");

        }

        // Operation
        IdentifierToken opcode;
        if (maybe<IdentifierToken>(tokens, out opcode)) {

            // Decode
            switch (opcode.Value) {
                
            }

        }
        eol(tokens);
    }

}
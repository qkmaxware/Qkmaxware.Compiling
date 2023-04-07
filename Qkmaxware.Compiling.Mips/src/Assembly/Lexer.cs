using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Mips.Assembly;

public class Lexer {

    private static bool isIdentifierChar(char c) {
        return char.IsLetterOrDigit(c) || c == '.' || c == '_' || c == '$';
    }

    private static bool isDigit(int c) {
        return c != -1 && char.IsDigit((char)c);
    }

    private static bool isHexChar(int c) {
        if (c == -1)
            return false;
        var c2 = (char)c;
        return 
            c == 'a' || c == 'A'
        ||  c == 'b' || c == 'B'
        ||  c == 'c' || c == 'C'
        ||  c == 'd' || c == 'D'
        ||  c == 'e' || c == 'E'
        ||  c == 'f' || c == 'f'
        ;
    }

    public IEnumerable<Token> Tokenize (string str) {
        var reader = new StringReader(str);
        return Tokenize(reader);
    }
    public IEnumerable<Token> Tokenize (TextReader reader) {
        long pos = 0;

        while (reader.Peek() != -1) {
            var now = pos;
            var next = (char)reader.Peek();

            // Line breaks
            if (next == '\n' || next == ';') {
                reader.Read(); pos++;
                yield return new StatementBreakToken(now);
                continue;
            }

            // Skip whitespace
            if (char.IsWhiteSpace(next)) {
                reader.Read(); pos++;
                continue;
            }

            // Skip comments
            if (next == '#') {
                reader.Read(); pos++;

                // Read until newline
                while (reader.Peek() != -1 && (char)reader.Peek() != '\n') {
                    reader.Read(); pos++;
                }
                continue;
            }

            // Read directives
            if (next == '.') {
                reader.Read(); pos++;
                StringBuilder sb = new StringBuilder();
                while (reader.Peek() != -1 && isIdentifierChar((char)reader.Peek())) {
                    sb.Append((char)reader.Read()); pos++;
                }
                var s = sb.ToString();
                yield return new DirectiveToken(now, s);
                continue;
            }

            // Id or Label
            if (char.IsLetter(next)) {
                StringBuilder sb = new StringBuilder();
                sb.Append((char)reader.Read()); pos++;
                while (reader.Peek() != -1 && isIdentifierChar((char)reader.Peek())) {
                    sb.Append((char)reader.Read()); pos++;
                }
                var s = sb.ToString();

                if (reader.Peek() == ':') {
                    reader.Read(); pos++;
                    yield return new LabelToken (now, s);
                } else {
                    yield return new IdentifierToken (now, s);           
                }
                continue;
            }

            // Commas
            if (next == ',') {
                reader.Read(); pos++;
                yield return new CommaToken(now);
                continue;
            }

            // Colons
            if (next == ':') {
                reader.Read(); pos++;
                yield return new ColonToken(now);
                continue;
            }

            // Parenthesis
            if (next == '(') {
                reader.Read(); pos++;
                yield return new OpenParenthesisToken(now);
                continue;
            }
            if (next == ')') {
                reader.Read(); pos++;
                yield return new CloseParenthesisToken(now);
                continue;
            }

            // Registers
            if (next == '$') {
                reader.Read(); pos++;
                StringBuilder sb = new StringBuilder();
                while (reader.Peek() != -1 && isIdentifierChar((char)reader.Peek())) {
                    sb.Append((char)reader.Read()); pos++;
                }
                var s = sb.ToString();
                var reg = RegisterIndex.Named(s);
                if (!reg.HasValue) {
                    throw new AssemblyException(now, $"Unknown register ${s}.");
                } else {
                    yield return new RegisterToken(now, reg.Value);
                }
                continue;
            }

            // Numbers
            if (char.IsDigit(next) || next == '+' || next == '-') {
                yield return readConstant(now, ref pos, reader);
                continue;
            }

            // Strings
            if (next == '"') {
                reader.Read(); pos++;

                var sb = new StringBuilder();
                while (reader.Peek() != '"') {
                    sb.Append((char)reader.Read()); pos++;
                }
                var s = sb.ToString();

                if (reader.Peek() != '"')
                    throw new AssemblyException(now, "Missing closing \" on string.");
                reader.Read(); pos++;

                yield return new StringConstantToken (now, s);
                continue;
            }
        
            // Else
            throw new AssemblyException(now, $"Unexpected character '{next}'.");
        }
    }

    private static Token readConstant(long startsAt, ref long position, TextReader reader) {
        StringBuilder sb = new StringBuilder();
        var first = (char)reader.Read(); // Read first char
        position++;

        // For binary numbers
        if (first == '0' && reader.Peek() == 'b') {
            reader.Read(); // Ignore first 2 chars
            position++;

            while (reader.Peek() == '0' || reader.Peek() == '1') {
                sb.Append((char)reader.Read()); position++;
            }
            return new ScalarConstantToken(startsAt, Convert.ToInt32(sb.ToString(), 2));
        }
        // For hexadecimal numbers
        else if (reader.Peek() == 'x' || reader.Peek() == 'X') {
            reader.Read(); // Ignore first 2 chars
            position++;

            while (isDigit(reader.Peek()) || isHexChar(reader.Peek())) {
                sb.Append((char)reader.Read()); position++;
            }
            return new ScalarConstantToken(startsAt, Convert.ToInt32(sb.ToString(), 16));
        } 
        // For standard base 10 numbers
        else {
            bool isFractional = false;
            sb.Append(first); // Use the first char

            // Read the rest of the whole numbers
            while (isDigit(reader.Peek())) {
                sb.Append((char)reader.Read());
                position++;
            }

            isFractional = reader.Peek() == '.';
            if (!isFractional) {
                return new ScalarConstantToken(startsAt, int.Parse(sb.ToString(), System.Globalization.CultureInfo.InvariantCulture));
            }

            // Read the rest of the exponent
            sb.Append((char)reader.Read()); position++; // .
            while (isDigit(reader.Peek())) {
                sb.Append((char)reader.Read()); position++;
            }

            // Read the exponential form
            if (!(reader.Peek() == 'e' || reader.Peek() == 'E')) {
                return new FloatingPointConstantToken(startsAt, float.Parse(sb.ToString()));
            }
            sb.Append((char)reader.Read()); position++;
            if (reader.Peek() == '-' || reader.Peek() == '+') {
                sb.Append((char)reader.Read()); position++;
            }
            while (isDigit(reader.Peek())) {
                sb.Append((char)reader.Read()); position++;
            }

            return new FloatingPointConstantToken(startsAt, float.Parse(sb.ToString(), System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}
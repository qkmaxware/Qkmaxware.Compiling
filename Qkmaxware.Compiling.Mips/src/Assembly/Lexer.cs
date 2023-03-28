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
                    sb.Append((char)reader.Read());
                }
                var s = sb.ToString();
                yield return new DirectiveToken(now, s);
                continue;
            }

            // Id or Label
            if (char.IsLetter(next)) {
                reader.Read(); pos++;
                StringBuilder sb = new StringBuilder();
                while (reader.Peek() != -1 && isIdentifierChar((char)reader.Peek())) {
                    sb.Append((char)reader.Read());
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
                    sb.Append((char)reader.Read());
                }
                var s = sb.ToString();
                switch (s) {
                    // Indexed
                    case "0":
                        yield return new RegisterToken (now, new RegisterIndex(0) );
                        break;
                    case "1":
                        yield return new RegisterToken (now, new RegisterIndex(1) );
                        break;
                    case "2":
                        yield return new RegisterToken (now, new RegisterIndex(2) );
                        break;
                    case "3":
                        yield return new RegisterToken (now, new RegisterIndex(3) );
                        break;
                    case "4":
                        yield return new RegisterToken (now, new RegisterIndex(4) );
                        break;
                    case "5":
                        yield return new RegisterToken (now, new RegisterIndex(5) );
                        break;
                    case "6":
                        yield return new RegisterToken (now, new RegisterIndex(6) );
                        break;
                    case "7":
                        yield return new RegisterToken (now, new RegisterIndex(7) );
                        break;
                    case "8":
                        yield return new RegisterToken (now, new RegisterIndex(8) );
                        break;
                    case "9":
                        yield return new RegisterToken (now, new RegisterIndex(9) );
                        break;
                    case "10":
                        yield return new RegisterToken (now, new RegisterIndex(10) );
                        break;
                    case "11":
                        yield return new RegisterToken (now, new RegisterIndex(11) );
                        break;
                    case "12":
                        yield return new RegisterToken (now, new RegisterIndex(12) );
                        break;
                    case "13":
                        yield return new RegisterToken (now, new RegisterIndex(13) );
                        break;
                    case "14":
                        yield return new RegisterToken (now, new RegisterIndex(14) );
                        break;
                    case "15":
                        yield return new RegisterToken (now, new RegisterIndex(15) );
                        break;
                    case "16":
                        yield return new RegisterToken (now, new RegisterIndex(16) );
                        break;
                    case "17":
                        yield return new RegisterToken (now, new RegisterIndex(17) );
                        break;
                    case "18":
                        yield return new RegisterToken (now, new RegisterIndex(18) );
                        break;
                    case "19":
                        yield return new RegisterToken (now, new RegisterIndex(19) );
                        break;
                    case "20":
                        yield return new RegisterToken (now, new RegisterIndex(20) );
                        break;
                    case "21":
                        yield return new RegisterToken (now, new RegisterIndex(21) );
                        break;
                    case "22":
                        yield return new RegisterToken (now, new RegisterIndex(22) );
                        break;
                    case "23":
                        yield return new RegisterToken (now, new RegisterIndex(23) );
                        break;
                    case "24":
                        yield return new RegisterToken (now, new RegisterIndex(24) );
                        break;
                    case "25":
                        yield return new RegisterToken (now, new RegisterIndex(25) );
                        break;
                    case "26":
                        yield return new RegisterToken (now, new RegisterIndex(26) );
                        break;
                    case "27":
                        yield return new RegisterToken (now, new RegisterIndex(27) );
                        break;
                    case "28":
                        yield return new RegisterToken (now, new RegisterIndex(28) );
                        break;
                    case "29":
                        yield return new RegisterToken (now, new RegisterIndex(29) );
                        break;
                    case "30":
                        yield return new RegisterToken (now, new RegisterIndex(30) );
                        break;
                    case "31":
                        yield return new RegisterToken (now, new RegisterIndex(31) );
                        break;
                    // Named
                    case "zero":
                        yield return new RegisterToken (now, new RegisterIndex(0) );
                        break;
                    case "at":
                        yield return new RegisterToken (now, new RegisterIndex(1) );
                        break;
                    case "v0":
                        yield return new RegisterToken (now, new RegisterIndex(2) );
                        break;
                    case "v1":
                        yield return new RegisterToken (now, new RegisterIndex(3) );
                        break;
                    case "a0":
                        yield return new RegisterToken (now, new RegisterIndex(4) );
                        break;
                    case "a1":
                        yield return new RegisterToken (now, new RegisterIndex(5) );
                        break;
                    case "a2":
                        yield return new RegisterToken (now, new RegisterIndex(6) );
                        break;
                    case "a3":
                        yield return new RegisterToken (now, new RegisterIndex(7) );
                        break;
                    case "t0":
                        yield return new RegisterToken (now, new RegisterIndex(8) );
                        break;
                    case "t1":
                        yield return new RegisterToken (now, new RegisterIndex(9) );
                        break;
                    case "t2":
                        yield return new RegisterToken (now, new RegisterIndex(10) );
                        break;
                    case "t3":
                        yield return new RegisterToken (now, new RegisterIndex(11) );
                        break;
                    case "t4":
                        yield return new RegisterToken (now, new RegisterIndex(12) );
                        break;
                    case "t5":
                        yield return new RegisterToken (now, new RegisterIndex(13) );
                        break;
                    case "t6":
                        yield return new RegisterToken (now, new RegisterIndex(14) );
                        break;
                    case "t7":
                        yield return new RegisterToken (now, new RegisterIndex(15) );
                        break;
                    case "s0":
                        yield return new RegisterToken (now, new RegisterIndex(16) );
                        break;
                    case "s1":
                        yield return new RegisterToken (now, new RegisterIndex(17) );
                        break;
                    case "s2":
                        yield return new RegisterToken (now, new RegisterIndex(18) );
                        break;
                    case "s3":
                        yield return new RegisterToken (now, new RegisterIndex(19) );
                        break;
                    case "s4":
                        yield return new RegisterToken (now, new RegisterIndex(20) );
                        break;
                    case "s5":
                        yield return new RegisterToken (now, new RegisterIndex(21) );
                        break;
                    case "s6":
                        yield return new RegisterToken (now, new RegisterIndex(22) );
                        break;
                    case "s7":
                        yield return new RegisterToken (now, new RegisterIndex(23) );
                        break;
                    case "t8":
                        yield return new RegisterToken (now, new RegisterIndex(24) );
                        break;
                    case "t9":
                        yield return new RegisterToken (now, new RegisterIndex(25) );
                        break;
                    case "k0":
                        yield return new RegisterToken (now, new RegisterIndex(26) );
                        break;
                    case "k1":
                        yield return new RegisterToken (now, new RegisterIndex(27) );
                        break;
                    case "gp":
                        yield return new RegisterToken (now, new RegisterIndex(28) );
                        break;
                    case "sp":
                        yield return new RegisterToken (now, new RegisterIndex(29) );
                        break;
                    case "fp":
                        yield return new RegisterToken (now, new RegisterIndex(30) );
                        break;
                    case "s8":
                        yield return new RegisterToken (now, new RegisterIndex(30) );
                        break;
                    case "ra":
                        yield return new RegisterToken (now, new RegisterIndex(31) );
                        break;
                    // IDK
                    default:
                        throw new AssemblyException(now, $"Unknown register ${s}.");
                }
                continue;
            }

            // Numbers
            if (char.IsDigit(next)) {
                yield return readConstant(now, reader);
                continue;
            }

            // Strings
            if (next == '"') {
                reader.Read(); pos++;

                var sb = new StringBuilder();
                while (reader.Peek() != '"') {
                    sb.Append((char)reader.Read());
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

    private static Token readConstant(long startsAt, TextReader reader) {
        StringBuilder sb = new StringBuilder();
        var first = (char)reader.Read(); // Read first char

        // For binary numbers
        if (reader.Peek() == 'b') {
            reader.Read(); // Ignore first 2 chars

            while (reader.Peek() == '0' || reader.Peek() == '1') {
                sb.Append((char)reader.Read());
            }
            return new ScalarConstantToken(startsAt, Convert.ToUInt32(sb.ToString(), 2));
        }
        // For hexadecimal numbers
        else if (reader.Peek() == 'x' || reader.Peek() == 'X') {
            reader.Read(); // Ignore first 2 chars

            while (isDigit(reader.Peek()) || isHexChar(reader.Peek())) {
                sb.Append((char)reader.Read());
            }
            return new ScalarConstantToken(startsAt, Convert.ToUInt32(sb.ToString(), 16));
        } 
        // For standard base 10 numbers
        else {
            bool isFractional = false;
            sb.Append(first); // Use the first char

            // Read the rest of the whole numbers
            while (isDigit(reader.Peek())) {
                sb.Append((char)reader.Read());
            }

            isFractional = reader.Peek() == '.';
            if (!isFractional) {
                return new ScalarConstantToken(startsAt, uint.Parse(sb.ToString(), System.Globalization.CultureInfo.InvariantCulture));
            }

            // Read the rest of the exponent
            sb.Append((char)reader.Read()); // .
            while (isDigit(reader.Peek())) {
                sb.Append((char)reader.Read());
            }

            // Read the exponential form
            if (!(reader.Peek() == 'e' || reader.Peek() == 'E')) {
                return new FloatingPointConstantToken(startsAt, float.Parse(sb.ToString()));
            }
            sb.Append((char)reader.Read());
            if (reader.Peek() == '-' || reader.Peek() == '+') {
                sb.Append((char)reader.Read());
            }
            while (isDigit(reader.Peek())) {
                sb.Append((char)reader.Read());
            }

            return new FloatingPointConstantToken(startsAt, float.Parse(sb.ToString(), System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Mips.Assembly;

public enum StorageType {
    word,
    asciiz,
}

public class AsmVariable {
    public string? Name;
    public StorageType StorageType;
}

public class AsmVariable<T> : AsmVariable {
    public T? Value;
}

public class Token {}
public class CommaToken : Token {}
public class DataHeaderToken : Token {}
public class CodeHeaderToken : Token {}
public class AsciiStorageTypeToken : Token {}
public class WordStorageTypeToken : Token {}
public class LabelToken : Token {
    public string? Text;
}
public class NameToken : Token {
    public string? Text;
}
public class StringToken : Token {
    public string? Text;
}
public class IntegerToken : Token {
    public uint Value;
}
public class RegisterToken : Token {
    public RegisterIndex Register;
}

public class AsmLexer {
    public IEnumerable<Token> Tokenize (string str) {
        var reader = new StringReader(str);
        return Tokenize(reader);
    }
    public IEnumerable<Token> Tokenize (TextReader reader) {
        while (reader.Peek() != -1) {
            var next = (char)reader.Peek();

            // Skip whitespace
            if (char.IsWhiteSpace(next)) {
                reader.Read();
                continue;
            }

            // Skip comments
            if (next == '#') {
                reader.Read();

                // Read until newline
                while (reader.Peek() != -1 && (char)reader.Peek() != '\n') {
                    reader.Read();
                }
                continue;
            }

            // Read section directives
            if (next == '.') {
                reader.Read();
                StringBuilder sb = new StringBuilder();
                while (reader.Peek() != -1 && char.IsLetterOrDigit((char)reader.Peek())) {
                    sb.Append((char)reader.Read());
                }
                var s = sb.ToString();
                switch (s) {
                    case "data":
                        yield return new DataHeaderToken();
                        break;
                    case "text":
                        yield return new CodeHeaderToken();
                        break;
                    case "word":
                        yield return new WordStorageTypeToken();
                        break;
                    case "asciiz":
                        yield return new AsciiStorageTypeToken();
                        break;
                    default: 
                        throw new System.ArgumentException($"Unknown section directive .{s}");
                }
                continue;
            }

            // Id or Label
            if (char.IsLetter(next)) {
                reader.Read();
                StringBuilder sb = new StringBuilder();
                while (reader.Peek() != -1 && char.IsLetterOrDigit((char)reader.Peek())) {
                    sb.Append((char)reader.Read());
                }
                var s = sb.ToString();

                if (reader.Peek() == ':') {
                    reader.Read();
                    yield return new LabelToken { Text = s };
                } else {
                    yield return new NameToken { Text = s };           
                }
                continue;
            }

            // Commas
            if (next == ',') {
                reader.Read();
                yield return new CommaToken();
                continue;
            }

            // Registers
            if (next == '$') {
                reader.Read();
                StringBuilder sb = new StringBuilder();
                while (reader.Peek() != -1 && char.IsLetterOrDigit((char)reader.Peek())) {
                    sb.Append((char)reader.Read());
                }
                var s = sb.ToString();
                switch (s) {
                    // Indexed
                    case "0":
                        yield return new RegisterToken { Register = new RegisterIndex(0) };
                        break;
                    case "1":
                        yield return new RegisterToken { Register = new RegisterIndex(1) };
                        break;
                    case "2":
                        yield return new RegisterToken { Register = new RegisterIndex(2) };
                        break;
                    case "3":
                        yield return new RegisterToken { Register = new RegisterIndex(3) };
                        break;
                    case "4":
                        yield return new RegisterToken { Register = new RegisterIndex(4) };
                        break;
                    case "5":
                        yield return new RegisterToken { Register = new RegisterIndex(5) };
                        break;
                    case "6":
                        yield return new RegisterToken { Register = new RegisterIndex(6) };
                        break;
                    case "7":
                        yield return new RegisterToken { Register = new RegisterIndex(7) };
                        break;
                    case "8":
                        yield return new RegisterToken { Register = new RegisterIndex(8) };
                        break;
                    case "9":
                        yield return new RegisterToken { Register = new RegisterIndex(9) };
                        break;
                    case "10":
                        yield return new RegisterToken { Register = new RegisterIndex(10) };
                        break;
                    case "11":
                        yield return new RegisterToken { Register = new RegisterIndex(11) };
                        break;
                    case "12":
                        yield return new RegisterToken { Register = new RegisterIndex(12) };
                        break;
                    case "13":
                        yield return new RegisterToken { Register = new RegisterIndex(13) };
                        break;
                    case "14":
                        yield return new RegisterToken { Register = new RegisterIndex(14) };
                        break;
                    case "15":
                        yield return new RegisterToken { Register = new RegisterIndex(15) };
                        break;
                    case "16":
                        yield return new RegisterToken { Register = new RegisterIndex(16) };
                        break;
                    case "17":
                        yield return new RegisterToken { Register = new RegisterIndex(17) };
                        break;
                    case "18":
                        yield return new RegisterToken { Register = new RegisterIndex(18) };
                        break;
                    case "19":
                        yield return new RegisterToken { Register = new RegisterIndex(19) };
                        break;
                    case "20":
                        yield return new RegisterToken { Register = new RegisterIndex(20) };
                        break;
                    case "21":
                        yield return new RegisterToken { Register = new RegisterIndex(21) };
                        break;
                    case "22":
                        yield return new RegisterToken { Register = new RegisterIndex(22) };
                        break;
                    case "23":
                        yield return new RegisterToken { Register = new RegisterIndex(23) };
                        break;
                    case "24":
                        yield return new RegisterToken { Register = new RegisterIndex(24) };
                        break;
                    case "25":
                        yield return new RegisterToken { Register = new RegisterIndex(25) };
                        break;
                    case "26":
                        yield return new RegisterToken { Register = new RegisterIndex(26) };
                        break;
                    case "27":
                        yield return new RegisterToken { Register = new RegisterIndex(27) };
                        break;
                    case "28":
                        yield return new RegisterToken { Register = new RegisterIndex(28) };
                        break;
                    case "29":
                        yield return new RegisterToken { Register = new RegisterIndex(29) };
                        break;
                    case "30":
                        yield return new RegisterToken { Register = new RegisterIndex(30) };
                        break;
                    case "31":
                        yield return new RegisterToken { Register = new RegisterIndex(31) };
                        break;
                    // Named
                    case "zero":
                        yield return new RegisterToken { Register = new RegisterIndex(0) };
                        break;
                    case "at":
                        yield return new RegisterToken { Register = new RegisterIndex(1) };
                        break;
                    case "v0":
                        yield return new RegisterToken { Register = new RegisterIndex(2) };
                        break;
                    case "v1":
                        yield return new RegisterToken { Register = new RegisterIndex(3) };
                        break;
                    case "a0":
                        yield return new RegisterToken { Register = new RegisterIndex(4) };
                        break;
                    case "a1":
                        yield return new RegisterToken { Register = new RegisterIndex(5) };
                        break;
                    case "a2":
                        yield return new RegisterToken { Register = new RegisterIndex(6) };
                        break;
                    case "a3":
                        yield return new RegisterToken { Register = new RegisterIndex(7) };
                        break;
                    case "t0":
                        yield return new RegisterToken { Register = new RegisterIndex(8) };
                        break;
                    case "t1":
                        yield return new RegisterToken { Register = new RegisterIndex(9) };
                        break;
                    case "t2":
                        yield return new RegisterToken { Register = new RegisterIndex(10) };
                        break;
                    case "t3":
                        yield return new RegisterToken { Register = new RegisterIndex(11) };
                        break;
                    case "t4":
                        yield return new RegisterToken { Register = new RegisterIndex(12) };
                        break;
                    case "t5":
                        yield return new RegisterToken { Register = new RegisterIndex(13) };
                        break;
                    case "t6":
                        yield return new RegisterToken { Register = new RegisterIndex(14) };
                        break;
                    case "t7":
                        yield return new RegisterToken { Register = new RegisterIndex(15) };
                        break;
                    case "s0":
                        yield return new RegisterToken { Register = new RegisterIndex(16) };
                        break;
                    case "s1":
                        yield return new RegisterToken { Register = new RegisterIndex(17) };
                        break;
                    case "s2":
                        yield return new RegisterToken { Register = new RegisterIndex(18) };
                        break;
                    case "s3":
                        yield return new RegisterToken { Register = new RegisterIndex(19) };
                        break;
                    case "s4":
                        yield return new RegisterToken { Register = new RegisterIndex(20) };
                        break;
                    case "s5":
                        yield return new RegisterToken { Register = new RegisterIndex(21) };
                        break;
                    case "s6":
                        yield return new RegisterToken { Register = new RegisterIndex(22) };
                        break;
                    case "s7":
                        yield return new RegisterToken { Register = new RegisterIndex(23) };
                        break;
                    case "t8":
                        yield return new RegisterToken { Register = new RegisterIndex(24) };
                        break;
                    case "t9":
                        yield return new RegisterToken { Register = new RegisterIndex(25) };
                        break;
                    case "k0":
                        yield return new RegisterToken { Register = new RegisterIndex(26) };
                        break;
                    case "k1":
                        yield return new RegisterToken { Register = new RegisterIndex(27) };
                        break;
                    case "gp":
                        yield return new RegisterToken { Register = new RegisterIndex(28) };
                        break;
                    case "sp":
                        yield return new RegisterToken { Register = new RegisterIndex(29) };
                        break;
                    case "fp":
                        yield return new RegisterToken { Register = new RegisterIndex(30) };
                        break;
                    case "s8":
                        yield return new RegisterToken { Register = new RegisterIndex(30) };
                        break;
                    case "ra":
                        yield return new RegisterToken { Register = new RegisterIndex(31) };
                        break;
                    // IDK
                    default:
                        throw new System.ArgumentException($"Unknown register ${s}");
                }
                continue;
            }

            // Integers
            if (char.IsDigit(next)) {
                var sb = new StringBuilder();
                while (reader.Peek() != -1 && char.IsDigit((char)reader.Peek())) {
                    sb.Append((char)reader.Read());
                }
                var s = sb.ToString();
                var i = uint.Parse(s);
                
                yield return new IntegerToken { Value = i };
                continue;
            }

            // Strings
            if (next == '"') {
                reader.Read();

                var sb = new StringBuilder();
                while (reader.Peek() != '"') {
                    sb.Append((char)reader.Read());
                }
                var s = sb.ToString();

                if (reader.Peek() != '"')
                    throw new System.ArgumentException("Missing closing \" on string");
                reader.Read();

                yield return new StringToken { Text = s };
                continue;
            }
        
            // Else
            throw new System.ArgumentException($"Unexpected character '{next}'");
        }
    }
}

public class DataSection {
    private List<AsmVariable> vars = new List<AsmVariable>();
}

public class AsmParser {
    public void Parse(BufferedTokenStream tokens) {
        while (tokens.HasNext()) {
            if (tokens.Peek(0) is DataHeaderToken) {

            } else if (tokens.Peek(0) is CodeHeaderToken) {

            } else {
                throw new System.ArgumentException("Expecting data section or code section but neither found");
            }
        }
    }
}
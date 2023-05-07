using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public class CommaToken : Token {
    public CommaToken(long pos) : base(pos) {}
}

public class ColonToken : Token {
    public ColonToken(long pos) : base(pos) {}
}

public class StatementBreakToken: Token {
    public StatementBreakToken(long pos) : base(pos) {}
}

public class OpenParenthesisToken : Token {
    public OpenParenthesisToken(long pos) : base(pos) {}
}

public class CloseParenthesisToken : Token {
    public CloseParenthesisToken(long pos) : base(pos) {}
}
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Mips.Assembly;

public class IdentifierToken : Token<string> {
    public IdentifierToken(long pos, string directive) : base(pos, directive) {}
}
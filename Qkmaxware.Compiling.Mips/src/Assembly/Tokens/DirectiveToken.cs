using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public class DirectiveToken : Token<string> {
    public DirectiveToken(long pos, string directive) : base(pos, directive) {}
}
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Mips.Assembly;

public class LabelToken : Token<string> {
    public LabelToken(long pos, string directive) : base(pos, directive) {}
}
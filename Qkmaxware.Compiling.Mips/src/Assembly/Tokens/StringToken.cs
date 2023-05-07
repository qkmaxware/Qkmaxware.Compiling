using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public class StringConstantToken : Token<string> {
    public StringConstantToken(long pos, string value) : base(pos, value) {}
}
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Mips.Assembly;

public class ScalarConstantToken : Token<uint> {
    public ScalarConstantToken(long pos, uint value) : base(pos, value) {}
}
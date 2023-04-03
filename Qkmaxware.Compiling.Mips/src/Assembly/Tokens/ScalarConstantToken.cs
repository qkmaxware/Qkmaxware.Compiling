using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Mips.Assembly;

public class ScalarConstantToken : Token<int>, IAddressLike {
    public ScalarConstantToken(long pos, int value) : base(pos, value) {}
}
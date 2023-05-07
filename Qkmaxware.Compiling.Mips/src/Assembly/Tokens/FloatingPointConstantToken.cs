using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public class FloatingPointConstantToken : Token<float> {
    public FloatingPointConstantToken(long pos, float value) : base(pos, value) {}
}
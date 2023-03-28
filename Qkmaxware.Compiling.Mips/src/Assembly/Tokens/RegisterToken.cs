using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Mips.Assembly;

public class RegisterToken : Token<RegisterIndex> {
    public RegisterToken(long pos, RegisterIndex index) : base(pos, index) {}
}
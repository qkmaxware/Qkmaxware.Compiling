using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public class IdentifierToken : AddressLikeToken {
    public IdentifierToken(long pos, string directive) : base(pos, directive) {}

    public override string ToString() {
        return this.Value;
    }
}
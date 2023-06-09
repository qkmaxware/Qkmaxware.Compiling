using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public class LabelToken : AddressLikeToken {
    public LabelToken(long pos, string directive) : base(pos, directive) {}

    public override string ToString() {
        return this.Value + ":";
    }

    public override AddressLikeValue GetAddress() => new LabelAddress(this.Value);
}
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public class ScalarConstantToken : AddressLikeToken {
    public int IntegerValue {get; private set;}
    public ScalarConstantToken(long pos, int value) : base(pos, value.ToString()) {
        this.IntegerValue = value;
    }

    public override AddressLikeValue GetAddress() => new IntegerAddress(this.IntegerValue);
}
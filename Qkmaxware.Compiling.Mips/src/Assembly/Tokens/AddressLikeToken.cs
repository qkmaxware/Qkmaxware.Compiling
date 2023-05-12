namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public abstract class AddressLikeToken : Token<string> {
    public AddressLikeToken(long pos, string value) : base(pos, value){ }

    public abstract AddressLikeValue GetAddress();
}

public class AddressLikeValue {}

public class LabelAddress : AddressLikeValue {
    public string Value;

    public LabelAddress (string val) {
        this.Value = val;
    }

    public override string ToString() => this.Value;
}

public class IntegerAddress : AddressLikeValue {
    public uint Value;

    public IntegerAddress (uint val) {
        this.Value = val;
    }

    public override string ToString() => this.Value.ToString();
}
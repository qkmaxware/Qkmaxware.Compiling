namespace Qkmaxware.Compiling.Mips.Assembly;

public class AddressLikeToken : Token<string> {
    public AddressLikeToken(long pos, string value) : base(pos, value){ }
}
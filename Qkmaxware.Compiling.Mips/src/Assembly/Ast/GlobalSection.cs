namespace Qkmaxware.Compiling.Mips.Assembly;

public class GlobalSection : Section {
    public List<IdentifierToken> Labels {get; private set;} = new List<IdentifierToken>();

    public override string ToString() {
        return $".globl {string.Join(',', Labels)}";
    }
}
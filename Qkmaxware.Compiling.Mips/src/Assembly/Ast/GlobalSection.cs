namespace Qkmaxware.Compiling.Mips.Assembly;

public class GlobalSection : Section {
    public List<IdentifierToken> Labels {get; private set;} = new List<IdentifierToken>();
}
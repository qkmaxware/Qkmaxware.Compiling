using Qkmaxware.Compiling.Mips.Assembly;

namespace Qkmaxware.Compiling.Mips.Assembly;

public class TextSection : Section {
    public List<IAssemblyInstruction> Code {get; private set;} = new List<IAssemblyInstruction>();

    public override string ToString() {
        return ".text";
    }
}
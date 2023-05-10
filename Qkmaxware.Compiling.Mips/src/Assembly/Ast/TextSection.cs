using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public class TextSection : Section {
    public List<IAssemblyInstruction> Code {get; private set;} = new List<IAssemblyInstruction>();

    public override string ToString() {
        return ".text";
    }
}
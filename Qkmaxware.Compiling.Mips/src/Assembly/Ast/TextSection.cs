using Qkmaxware.Compiling.Mips.InstructionSet;

namespace Qkmaxware.Compiling.Mips.Assembly;

public class TextSection : Section {
    public List<IAssembleable> Code {get; private set;} = new List<IAssembleable>();
}
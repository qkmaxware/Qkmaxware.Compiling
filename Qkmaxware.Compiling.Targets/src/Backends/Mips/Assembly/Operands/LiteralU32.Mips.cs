using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Targets.Mips.Assembly;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Ir;

public partial class LiteralU32 : IMipsValueOperand {
    public IEnumerable<IAssemblyInstruction> MipsInstructionsToLoadValueInto(RegisterIndex index) {
        yield return new Li{
            Destination = index,
            Value = this.Value,
        };
    }
}
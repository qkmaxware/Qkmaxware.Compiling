using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Targets.Mips.Assembly;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Ir;

public partial class LiteralF32 : IMipsValueOperand {
    public IEnumerable<IAssemblyInstruction> MipsInstructionsToLoadValueInto(RegisterIndex index) {
        yield return new Li {
            Destination = index,
            Value = BitConverter.ToUInt32(BitConverter.GetBytes(this.Value)),
        };
    }
}
using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Targets.Mips.Assembly;

namespace Qkmaxware.Compiling.Targets.Ir;

public partial class LiteralF32 : IMipsValueOperand {
    public IEnumerable<IAssemblyInstruction> MipsInstructionsToLoadValueInto(RegisterIndex index) {
        yield return new LoadImmediate{
            ResultRegister = index,
            Constant = BitConverter.ToUInt32(BitConverter.GetBytes(this.Value)),
        };
    }
}

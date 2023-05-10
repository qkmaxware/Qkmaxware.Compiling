using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Targets.Mips.Assembly;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Ir;

public abstract partial class Declaration : IMipsValueOperand {
    public abstract IEnumerable<IAssemblyInstruction> MipsInstructionsToLoadValueInto(RegisterIndex index);
    /*public abstract IEnumerable<IAssemblyInstruction> MipsInstructionsToStoreValueFrom(RegisterIndex index);*/
}

public partial class Global : IMipsValueOperand {
    public string MipsMemoryLabel => "global_" + this.Name;
    public override IEnumerable<IAssemblyInstruction> MipsInstructionsToLoadValueInto(RegisterIndex index) {
        yield return new La {
            Destination = index,
            Label = this.MipsMemoryLabel
        };
    }
}

public partial class Local : IMipsValueOperand {
    public override IEnumerable<IAssemblyInstruction> MipsInstructionsToLoadValueInto(RegisterIndex index) {
        yield return new Lw {
            Target = index,
            Source = RegisterIndex.FP,
            Immediate = this.VariableIndex // FP + local index
        };
    }
}
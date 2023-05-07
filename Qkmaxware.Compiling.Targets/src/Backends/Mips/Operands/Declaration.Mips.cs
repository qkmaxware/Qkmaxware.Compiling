using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Targets.Mips.Assembly;

namespace Qkmaxware.Compiling.Targets.Ir;

public abstract partial class Declaration : IMipsValueOperand {
    public abstract IEnumerable<IAssemblyInstruction> MipsInstructionsToLoadValueInto(RegisterIndex index);
    /*public abstract IEnumerable<IAssemblyInstruction> MipsInstructionsToStoreValueFrom(RegisterIndex index);*/
}

public partial class Global : IMipsValueOperand {
    public string MipsMemoryLabel => "global_" + this.Name;
    public override IEnumerable<IAssemblyInstruction> MipsInstructionsToLoadValueInto(RegisterIndex index) {
        yield return new LoadAddress {
            ResultRegister = index,
            Label = this.MipsMemoryLabel
        };
    }
}

public partial class Local : IMipsValueOperand {
    public override IEnumerable<IAssemblyInstruction> MipsInstructionsToLoadValueInto(RegisterIndex index) {
        yield return new LoadWord {
            ResultRegister = index,
            BaseRegister = RegisterIndex.FP,
            Offset = this.VariableIndex // FP + local index
        };
    }
}
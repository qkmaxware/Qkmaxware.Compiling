using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Targets.Mips.Assembly;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Ir;

public abstract partial class Declaration : IMipsValueOperand {
    public abstract IEnumerable<IAssemblyInstruction> MipsInstructionsToLoadValueInto(RegisterIndex index);
    public abstract IEnumerable<IAssemblyInstruction> MipsInstructionsToStoreValue(RegisterIndex from);
}

public partial class Global : IMipsValueOperand {
    public string MipsMemoryLabel => "global_" + this.Name;
    public override IEnumerable<IAssemblyInstruction> MipsInstructionsToLoadValueInto(RegisterIndex index) {
        yield return new La {
            Destination = index,
            Label = this.MipsMemoryLabel
        };
        yield return new Lw {
            Target = index,
            Source = index,
            Immediate = 0
        };
    }
    public override IEnumerable<IAssemblyInstruction> MipsInstructionsToStoreValue(RegisterIndex from) {
        yield return new La {
            Destination = RegisterIndex.At,
            Label = this.MipsMemoryLabel
        };
        yield return new Sw {
            Target = from,
            Source = RegisterIndex.At,
            Immediate = 0
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

    public override IEnumerable<IAssemblyInstruction> MipsInstructionsToStoreValue(RegisterIndex from) {
        yield return new Sw {
            Target = from,
            Source = RegisterIndex.FP,
            Immediate = this.VariableIndex // FP + local index
        };
    }
}
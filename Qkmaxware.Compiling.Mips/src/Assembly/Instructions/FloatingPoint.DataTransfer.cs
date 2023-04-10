namespace Qkmaxware.Compiling.Mips.Assembly;

public class LoadIntoCoprocessor1 : IAssemblyInstruction {
    public RegisterIndex ResultRegister;
    public RegisterIndex BaseRegister;
    public uint Offset;

    public string InstrName() => "lwc1";
    public string InstrFormat() => InstrName() + " $rDest,offset($rBase)";
    public string InstrDescription() =>  "Load 32 bit word from memory address $rBase + offset into $rDest on the FPU.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class StoreFromCoprocessor1 : IAssemblyInstruction {
    public RegisterIndex SourceRegister;
    public RegisterIndex BaseRegister;
    public uint Offset;

    public string InstrName() => "swc1";
    public string InstrFormat() => InstrName() + " $rSrc,offset($rBase)";
    public string InstrDescription() =>  "Stores 32 bit word from $rSrc on the FPU to memory address $rBase + offset.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class MoveToCoprocessor1 : IAssemblyInstruction {
    public RegisterIndex CpuRegister;
    public RegisterIndex FpuRegister;

    public string InstrName() => "mtc1";
    public string InstrFormat() => InstrName() + " $rFrom, $rTo";
    public string InstrDescription() =>  "Move the value stored in CPU register $rFrom into FPU register $rTo.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string? ToString() {
        return $"move {this.CpuRegister},{this.FpuRegister}";
    }
}

public class MoveFromCoprocessor1 : IAssemblyInstruction {
    public RegisterIndex CpuRegister;
    public RegisterIndex FpuRegister;

    public string InstrName() => "mfc1";
    public string InstrFormat() => InstrName() + " $rTo, $rFrom";
    public string InstrDescription() =>  "Move the value stored in FPU register $rTo into CPU register $rFrom.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string? ToString() {
        return $"move {this.CpuRegister},{this.FpuRegister}";
    }
}
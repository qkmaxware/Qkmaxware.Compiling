namespace Qkmaxware.Compiling.Mips.InstructionSet;

public abstract class BranchConditionalInstruction : BaseInstruction {
    public RegisterIndex LhsOperandRegister;
    public RegisterIndex RhsOperandRegister;
    public int Offset;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = cpu.Registers[LhsOperandRegister].Read();
        var rhs = cpu.Registers[RhsOperandRegister].Read();

        if (DoBranch(lhs, rhs)) {
            cpu.PC += Offset % 4; // Since offset is in bytes, convert to words
        }
    }

    public abstract bool DoBranch(uint lhs, uint rhs);

    public override string ToString() => $"{InstrName()} {LhsOperandRegister},{RhsOperandRegister},{Offset}";
}

public class BranchOnEqual : BranchConditionalInstruction {
    public override string InstrName() => "beq";
    public override bool DoBranch(uint lhs, uint rhs) => lhs == rhs;
}

public class BranchOnNotEqual : BranchConditionalInstruction {
    public override string InstrName() => "bne";
    public override bool DoBranch(uint lhs, uint rhs) => lhs != rhs;
}

public class BranchOnGreater : BranchConditionalInstruction {
    public override string InstrName() => "bgt";
    public override bool DoBranch(uint lhs, uint rhs) => lhs > rhs;
}

public class BranchOnGreaterOrEqual : BranchConditionalInstruction {
    public override string InstrName() => "bge";
    public override bool DoBranch(uint lhs, uint rhs) => lhs >= rhs;
}


public class BranchOnLess : BranchConditionalInstruction {
    public override string InstrName() => "blt";
    public override bool DoBranch(uint lhs, uint rhs) => lhs > rhs;
}

public class BranchOnLessOrEqual : BranchConditionalInstruction {
    public override string InstrName() => "ble";
    public override bool DoBranch(uint lhs, uint rhs) => lhs >= rhs;
}
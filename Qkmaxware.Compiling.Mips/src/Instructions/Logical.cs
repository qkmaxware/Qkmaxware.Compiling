namespace Qkmaxware.Compiling.Mips.InstructionSet;

public class And : ThreeAddressInstruction {
    public override string InstrName() => "and";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = u32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = u32(cpu.Registers[this.RhsOperandRegister]);
        cpu.Registers[this.ResultRegister].Write(u32(lhs & rhs));
    }
}

public class Or : ThreeAddressInstruction {
    public override string InstrName() => "or";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = u32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = u32(cpu.Registers[this.RhsOperandRegister]);
        cpu.Registers[this.ResultRegister].Write(u32(lhs | rhs));
    }
}

public class AndImmediate : TwoAddressImmediateInstruction<uint> {
    public override string InstrName() => "andi";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = u32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = this.RhsOperand;
        cpu.Registers[this.ResultRegister].Write(u32(lhs & rhs));
    }
}

public class OrImmediate : TwoAddressImmediateInstruction<uint> {
    public override string InstrName() => "ori";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = u32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = this.RhsOperand;
        cpu.Registers[this.ResultRegister].Write(u32(lhs | rhs));
    }
}

public class ShiftLeftLogical : ThreeAddressInstruction {
    public override string InstrName() => "sll";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = u32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = u32(cpu.Registers[this.RhsOperandRegister]);
        cpu.Registers[this.ResultRegister].Write(u32(lhs << (int)rhs));
    }
}

public class ShiftRightLogical : ThreeAddressInstruction {
    public override string InstrName() => "srl";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = u32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = u32(cpu.Registers[this.RhsOperandRegister]);
        cpu.Registers[this.ResultRegister].Write(u32(lhs >> (int)rhs));
    }
}
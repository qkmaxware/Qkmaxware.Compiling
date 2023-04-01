namespace Qkmaxware.Compiling.Mips.InstructionSet;

public class AddSigned : ThreeAddressInstruction {
    public override string InstrName() => "add";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = i32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = i32(cpu.Registers[this.RhsOperandRegister]);
        cpu.Registers[this.ResultRegister].Write(u32(lhs + rhs));
    }
}

public class SubtractSigned : ThreeAddressInstruction {
    public override string InstrName() => "sub";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = i32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = i32(cpu.Registers[this.RhsOperandRegister]);
        cpu.Registers[this.ResultRegister].Write(u32(lhs - rhs));
    }
}

public class AddSignedImmediate : TwoAddressImmediateInstruction<int> {
    public override string InstrName() => "addi";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = i32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = this.RhsOperand;
        cpu.Registers[this.ResultRegister].Write(u32(lhs + rhs));
    }
}

public class SubtractSignedImmediate : TwoAddressImmediateInstruction<int> {
    public override string InstrName() => "subi";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = i32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = this.RhsOperand;
        cpu.Registers[this.ResultRegister].Write(u32(lhs - rhs));
    }
}

public class AddUnsigned : ThreeAddressInstruction {
    public override string InstrName() => "addu";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = u32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = u32(cpu.Registers[this.RhsOperandRegister]);
        cpu.Registers[this.ResultRegister].Write(u32(lhs + rhs));
    }
}

public class SubtractUnsigned : ThreeAddressInstruction {
    public override string InstrName() => "subu";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = u32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = u32(cpu.Registers[this.RhsOperandRegister]);
        cpu.Registers[this.ResultRegister].Write(u32(lhs - rhs));
    }
}

public class AddUnsignedImmediate : TwoAddressImmediateInstruction<uint> {
    public override string InstrName() => "addiu";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = u32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = this.RhsOperand;
        cpu.Registers[this.ResultRegister].Write(u32(lhs + rhs));
    }
}

public class SubtractUnsignedImmediate : TwoAddressImmediateInstruction<uint> {
    public override string InstrName() => "subiu";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = u32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = this.RhsOperand;
        cpu.Registers[this.ResultRegister].Write(u32(lhs - rhs));
    }
}

public class MultiplyWithoutOverflow : ThreeAddressInstruction {
    public override string InstrName() => "mul";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = i32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = i32(cpu.Registers[this.RhsOperandRegister]);
        cpu.Registers[this.ResultRegister].Write(u32(lhs * rhs));
    }
}

public class MultiplyWithOverflow : TwoAddressBinaryInstruction {
    public override string InstrName() => "mult";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = i32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = i32(cpu.Registers[this.RhsOperandRegister]);
        var result = (long)lhs * (long)rhs;

        var hi = (int)(result >> 32);
        var lo = (int)(result & 0xFFFFFFFF);

        cpu.Registers.HI.Write(u32(hi));
        cpu.Registers.LO.Write(u32(lo));
    }
}

public class DivideWithRemainder : TwoAddressBinaryInstruction {
    public override string InstrName() => "div";
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = i32(cpu.Registers[this.LhsOperandRegister]);
        var rhs = i32(cpu.Registers[this.RhsOperandRegister]);
        var quotient = lhs / rhs;
        var remainder = lhs & rhs;

        cpu.Registers.HI.Write(u32(remainder));
        cpu.Registers.LO.Write(u32(quotient));
    }
}
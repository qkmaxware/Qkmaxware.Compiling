namespace Qkmaxware.Compiling.Mips.InstructionSet;


public class SetOnLessThan : BaseInstruction {
    public override string InstrName() => "slt";

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = cpu.Registers[LhsOperandRegister].Read();
        var rhs = cpu.Registers[RhsOperandRegister].Read();

        cpu.Registers[ResultRegister].Write((uint)(lhs < rhs ? 1 : 0));
    }

    public RegisterIndex ResultRegister;
    public RegisterIndex LhsOperandRegister;
    public RegisterIndex RhsOperandRegister;

    public override string ToString() {
        return $"${InstrName()} ${this.ResultRegister},${this.LhsOperandRegister},${this.RhsOperandRegister}";
    }
}


public class SetOnLessThanImmediate : BaseInstruction {
    public override string InstrName() => "slti";

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = cpu.Registers[LhsOperandRegister].Read();
        var rhs = this.Constant;

        cpu.Registers[ResultRegister].Write((uint)(lhs < rhs ? 1 : 0));
    }

    public RegisterIndex ResultRegister;
    public RegisterIndex LhsOperandRegister;
    public int Constant;

    public override string ToString() {
        return $"${InstrName()} ${this.ResultRegister},${this.LhsOperandRegister},${this.Constant}";
    }
}
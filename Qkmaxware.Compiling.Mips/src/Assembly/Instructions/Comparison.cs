namespace Qkmaxware.Compiling.Mips.Assembly;


public class SetOnLessThan : BaseAssemblyInstruction {
    public override string InstrName() => "slt";

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = cpu.Registers[LhsOperandRegister].Read();
        var rhs = cpu.Registers[RhsOperandRegister].Read();

        cpu.Registers[ResultRegister].Write((uint)(lhs < rhs ? 1 : 0));
    }

    public RegisterIndex ResultRegister;
    public RegisterIndex LhsOperandRegister;
    public RegisterIndex RhsOperandRegister;

    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string ToString() {
        return $"{InstrName()} {this.ResultRegister},{this.LhsOperandRegister},{this.RhsOperandRegister}";
    }
}


public class SetOnLessThanImmediate : BaseAssemblyInstruction {
    public override string InstrName() => "slti";

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = cpu.Registers[LhsOperandRegister].Read();
        var rhs = this.Constant;

        cpu.Registers[ResultRegister].Write((uint)(lhs < rhs ? 1 : 0));
    }

    public RegisterIndex ResultRegister;
    public RegisterIndex LhsOperandRegister;
    public int Constant;

    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string ToString() {
        return $"{InstrName()} {this.ResultRegister},{this.LhsOperandRegister},{this.Constant}";
    }
}
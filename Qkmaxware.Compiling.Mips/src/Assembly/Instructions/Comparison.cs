namespace Qkmaxware.Compiling.Targets.Mips.Assembly;


public class SetOnLessThan : BaseAssemblyInstruction {
    public override string InstrName() => "slt";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public override string InstrDescription() =>  "Stores a value of 1 in rDest if the value in rLhs < rRhs.";

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
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, value";
    public override string InstrDescription() =>  "Stores a value of 1 in rDest if the value in rLhs < immediate value.";

    public RegisterIndex ResultRegister;
    public RegisterIndex LhsOperandRegister;
    public int Constant;

    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string ToString() {
        return $"{InstrName()} {this.ResultRegister},{this.LhsOperandRegister},{this.Constant}";
    }
}
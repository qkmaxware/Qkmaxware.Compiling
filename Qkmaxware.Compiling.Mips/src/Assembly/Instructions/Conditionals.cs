using Qkmaxware.Compiling.Targets.Mips.Assembly;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public abstract class BranchConditionalInstruction : BaseAssemblyInstruction {
    public RegisterIndex LhsOperandRegister;
    public RegisterIndex RhsOperandRegister;
    public AddressLikeToken Address;

    public abstract bool DoBranch(uint lhs, uint rhs);

    public override string ToString() => $"{InstrName()} {LhsOperandRegister},{RhsOperandRegister},{Address}";
}

public class BranchOnEqual : BranchConditionalInstruction {
    public override string InstrName() => "beq";
    public override string InstrFormat() => InstrName() + " $rLhs, $rRhs, label";
    public override string InstrDescription() =>  "If the value stored in rLhs == rRhs then goto the address provided.";
    public override bool DoBranch(uint lhs, uint rhs) => lhs == rhs;
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class BranchGreaterThan0 : BaseAssemblyInstruction {

    public RegisterIndex LhsOperandRegister;
    public AddressLikeToken Address;

    public override string InstrName() => "bgtz";
    public override string InstrFormat() => InstrName() + " $rLhs, label";
    public override string InstrDescription() =>  "If the value stored in rLhs > 0 then goto the address provided.";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class BranchLessThanOrEqual0 : BaseAssemblyInstruction {

    public RegisterIndex LhsOperandRegister;
    public AddressLikeToken Address;

    public override string InstrName() => "blez";
    public override string InstrFormat() => InstrName() + " $rLhs, label";
    public override string InstrDescription() =>  "If the value stored in rLhs <= 0 then goto the address provided.";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class BranchOnNotEqual : BranchConditionalInstruction {
    public override string InstrName() => "bne";
    public override string InstrFormat() => InstrName() + " $rLhs, $rRhs, label";
    public override string InstrDescription() =>  "If the value stored in rLhs != rRhs then goto the address provided.";
    public override bool DoBranch(uint lhs, uint rhs) => lhs != rhs;
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class BranchOnGreater : BranchConditionalInstruction {
    public override string InstrName() => "bgt";
    public override string InstrFormat() => InstrName() + " $rLhs, $rRhs, label";
    public override string InstrDescription() =>  "If the value stored in rLhs > rRhs then goto the address provided.";
    public override bool DoBranch(uint lhs, uint rhs) => lhs > rhs;
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class BranchOnGreaterOrEqual : BranchConditionalInstruction {
    public override string InstrName() => "bge";
    public override string InstrFormat() => InstrName() + " $rLhs, $rRhs, label";
    public override string InstrDescription() =>  "If the value stored in rLhs >= rRhs then goto the address provided.";
    public override bool DoBranch(uint lhs, uint rhs) => lhs >= rhs;
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}


public class BranchOnLess : BranchConditionalInstruction {
    public override string InstrName() => "blt";
    public override string InstrFormat() => InstrName() + " $rLhs, $rRhs, label";
    public override string InstrDescription() =>  "If the value stored in rLhs < rRhs then goto the address provided.";
    public override bool DoBranch(uint lhs, uint rhs) => lhs > rhs;
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class BranchOnLessOrEqual : BranchConditionalInstruction {
    public override string InstrName() => "ble";
    public override string InstrFormat() => InstrName() + " $rLhs, $rRhs, label";
    public override string InstrDescription() =>  "If the value stored in rLhs <= rRhs then goto the address provided.";
    public override bool DoBranch(uint lhs, uint rhs) => lhs >= rhs;
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}
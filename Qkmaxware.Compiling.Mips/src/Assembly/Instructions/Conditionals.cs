using Qkmaxware.Compiling.Mips.Assembly;

namespace Qkmaxware.Compiling.Mips.Assembly;

public abstract class BranchConditionalInstruction : BaseAssemblyInstruction {
    public RegisterIndex LhsOperandRegister;
    public RegisterIndex RhsOperandRegister;
    public AddressLikeToken Address;

    public abstract bool DoBranch(uint lhs, uint rhs);

    public override string ToString() => $"{InstrName()} {LhsOperandRegister},{RhsOperandRegister},{Address}";
}

public class BranchOnEqual : BranchConditionalInstruction {
    public override string InstrName() => "beq";
    public override bool DoBranch(uint lhs, uint rhs) => lhs == rhs;
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class BranchGreaterThan0 : BaseAssemblyInstruction {

    public RegisterIndex LhsOperandRegister;
    public AddressLikeToken Address;

    public override string InstrName() => "bgtz";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class BranchLessThanOrEqual0 : BaseAssemblyInstruction {

    public RegisterIndex LhsOperandRegister;
    public AddressLikeToken Address;

    public override string InstrName() => "blez";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class BranchOnNotEqual : BranchConditionalInstruction {
    public override string InstrName() => "bne";
    public override bool DoBranch(uint lhs, uint rhs) => lhs != rhs;
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class BranchOnGreater : BranchConditionalInstruction {
    public override string InstrName() => "bgt";
    public override bool DoBranch(uint lhs, uint rhs) => lhs > rhs;
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class BranchOnGreaterOrEqual : BranchConditionalInstruction {
    public override string InstrName() => "bge";
    public override bool DoBranch(uint lhs, uint rhs) => lhs >= rhs;
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}


public class BranchOnLess : BranchConditionalInstruction {
    public override string InstrName() => "blt";
    public override bool DoBranch(uint lhs, uint rhs) => lhs > rhs;
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class BranchOnLessOrEqual : BranchConditionalInstruction {
    public override string InstrName() => "ble";
    public override bool DoBranch(uint lhs, uint rhs) => lhs >= rhs;
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}
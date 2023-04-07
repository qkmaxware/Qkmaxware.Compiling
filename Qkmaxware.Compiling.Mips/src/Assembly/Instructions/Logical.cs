namespace Qkmaxware.Compiling.Mips.Assembly;

public class And : ThreeAddressInstruction {
    public override string InstrName() => "and";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class Or : ThreeAddressInstruction {
    public override string InstrName() => "or";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class AndImmediate : TwoAddressImmediateInstruction<uint> {
    public override string InstrName() => "andi";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class OrImmediate : TwoAddressImmediateInstruction<uint> {
    public override string InstrName() => "ori";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class ShiftLeftLogical : ThreeAddressInstruction {
    public override string InstrName() => "sll";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class ShiftRightLogical : ThreeAddressInstruction {
    public override string InstrName() => "srl";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}
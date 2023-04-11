namespace Qkmaxware.Compiling.Mips.Assembly;

public class And : ThreeAddressInstruction {
    public override string InstrName() => "and";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public override string InstrDescription() =>  "Bitwise AND of rLhs and rRhs. The result is stored in rDest.";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class Or : ThreeAddressInstruction {
    public override string InstrName() => "or";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public override string InstrDescription() =>  "Bitwise OR of rLhs and rRhs. The result is stored in rDest.";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class Nor : ThreeAddressInstruction {
    public override string InstrName() => "nor";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public override string InstrDescription() =>  "Bitwise NOR of rLhs and rRhs. The result is stored in rDest.";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class Xor : ThreeAddressInstruction {
    public override string InstrName() => "xor";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public override string InstrDescription() =>  "Bitwise XOR of rLhs and rRhs. The result is stored in rDest.";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class AndImmediate : TwoAddressImmediateInstruction<uint> {
    public override string InstrName() => "andi";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, value";
    public override string InstrDescription() =>  "Bitwise AND of rLhs and an immediate value. The result is stored in rDest.";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class OrImmediate : TwoAddressImmediateInstruction<uint> {
    public override string InstrName() => "ori";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, value";
    public override string InstrDescription() =>  "Bitwise Or of rLhs and and immediate value. The result is stored in rDest.";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class XorImmediate : TwoAddressImmediateInstruction<uint> {
    public override string InstrName() => "xori";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, value";
    public override string InstrDescription() =>  "Bitwise XOR of rLhs and an immediate value. The result is stored in rDest.";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class ShiftLeftLogical : ThreeAddressInstruction {
    public override string InstrName() => "sllv";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public override string InstrDescription() =>  "Shifts the value stored in rLhs to the left by $rRhs. The result is stored in rDest.";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class ShiftRightLogical : ThreeAddressInstruction {
    public override string InstrName() => "srlv";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public override string InstrDescription() =>  "Shifts the value stored in rLhs to the right by $rRhs. The result is stored in rDest.";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}
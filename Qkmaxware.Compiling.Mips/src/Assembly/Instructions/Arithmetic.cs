namespace Qkmaxware.Compiling.Mips.Assembly;

public class AddSigned : ThreeAddressInstruction {
    public override string InstrName() => "add";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class SubtractSigned : ThreeAddressInstruction {
    public override string InstrName() => "sub";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class AddSignedImmediate : TwoAddressImmediateInstruction<int> {
    public override string InstrName() => "addi";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class SubtractSignedImmediate : TwoAddressImmediateInstruction<int> {
    public override string InstrName() => "subi";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class AddUnsigned : ThreeAddressInstruction {
    public override string InstrName() => "addu";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class SubtractUnsigned : ThreeAddressInstruction {
    public override string InstrName() => "subu";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class AddUnsignedImmediate : TwoAddressImmediateInstruction<uint> {
    public override string InstrName() => "addiu";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class SubtractUnsignedImmediate : TwoAddressImmediateInstruction<uint> {
    public override string InstrName() => "subiu";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class MultiplySignedWithOverflow : TwoAddressBinaryInstruction {
    public override string InstrName() => "mult";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class MultiplyUnsignedWithOverflow : TwoAddressBinaryInstruction {
    public override string InstrName() => "multu";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class DivideSignedWithRemainder : TwoAddressBinaryInstruction {
    public override string InstrName() => "div";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class DivideUnsignedWithRemainder : TwoAddressBinaryInstruction {
    public override string InstrName() => "divu";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}
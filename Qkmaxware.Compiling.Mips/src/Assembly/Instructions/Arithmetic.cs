namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public class AddSigned : ThreeAddressInstruction {
    public override string InstrName() => "add";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public override string InstrDescription() =>  "Adds the value stored in rLhs and rRhs and stores the result in rDest. Values are treated as signed integers";

    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class SubtractSigned : ThreeAddressInstruction {
    public override string InstrName() => "sub";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public override string InstrDescription() =>  "Subtracts the value stored in rLhs and rRhs and stores the result in rDest. Values are treated as signed integers";

    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class AddSignedImmediate : TwoAddressImmediateInstruction<int> {
    public override string InstrName() => "addi";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, value";
    public override string InstrDescription() =>  "Adds the value stored in rLhs and a provided value and stores the result in rDest. Values are treated as signed integers";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class SubtractSignedImmediate : TwoAddressImmediateInstruction<int> {
    public override string InstrName() => "subi";
     public override string InstrFormat() => InstrName() + " $rDest, $rLhs, value";
    public override string InstrDescription() =>  "Subtracts the value stored in rLhs and a provided value and stores the result in rDest. Values are treated as signed integers";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class AddUnsigned : ThreeAddressInstruction {
    public override string InstrName() => "addu";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public override string InstrDescription() =>  "Adds the values stored in rLhs and in rRhs and stores the result in rDest. Values are treated as unsigned integers";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class SubtractUnsigned : ThreeAddressInstruction {
    public override string InstrName() => "subu";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public override string InstrDescription() =>  "Subtracts the values stored in rLhs and in rRhs and stores the result in rDest. Values are treated as unsigned integers";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class AddUnsignedImmediate : TwoAddressImmediateInstruction<uint> {
    public override string InstrName() => "addiu";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, value";
    public override string InstrDescription() =>  "Adds the values stored in rLhs and an immediate value and stores the result in rDest. Values are treated as unsigned integers";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class SubtractUnsignedImmediate : TwoAddressImmediateInstruction<uint> {
    public override string InstrName() => "subiu";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, value";
    public override string InstrDescription() =>  "Subtracts the values stored in rLhs and an immediate value and stores the result in rDest. Values are treated as unsigned integers";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class MultiplySignedWithOverflow : TwoAddressBinaryInstruction {
    public override string InstrName() => "mult";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public override string InstrDescription() =>  "Multiplies the value stored in rLhs and rRhs and stores the result in rDest. Values are treated as signed integers";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class MultiplyUnsignedWithOverflow : TwoAddressBinaryInstruction {
    public override string InstrName() => "multu";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public override string InstrDescription() =>  "Multiplies the value stored in rLhs and rRhs and stores the result in rDest. Values are treated as unsigned integers";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class DivideSignedWithRemainder : TwoAddressBinaryInstruction {
    public override string InstrName() => "div";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public override string InstrDescription() =>  "Divides the value stored in rLhs and rRhs and stores the result in rDest. Values are treated as signed integers";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class DivideUnsignedWithRemainder : TwoAddressBinaryInstruction {
    public override string InstrName() => "divu";
    public override string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public override string InstrDescription() =>  "Divides the value stored in rLhs and rRhs and stores the result in rDest. Values are treated as unsigned integers";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}
namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public class AbsoluteValueSingle : IAssemblyInstruction {
    public RegisterIndex ResultRegister;
    public RegisterIndex SourceRegister;

    public string InstrName() => "abs.s";
    public string InstrFormat() => InstrName() + " $rDest, $rSrc";
    public string InstrDescription() =>  "Compute the absolute value of the single in register $rSrc and store the result in $rDest";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class AddSingle : IAssemblyInstruction {
    public RegisterIndex ResultRegister;
    public RegisterIndex LhsOperandRegister;
    public RegisterIndex RhsOperandRegister;

    public string InstrName() => "add.s";
    public string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public string InstrDescription() =>  "Add the single values from $rLhs and $rRhs in the FPU and store the value in $rDest.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class SubtractSingle : IAssemblyInstruction {
    public RegisterIndex ResultRegister;
    public RegisterIndex LhsOperandRegister;
    public RegisterIndex RhsOperandRegister;

    public string InstrName() => "sub.s";
    public string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public string InstrDescription() =>  "Subtract the single values from $rLhs and $rRhs in the FPU and store the value in $rDest.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class MultiplySingle : IAssemblyInstruction {
    public RegisterIndex ResultRegister;
    public RegisterIndex LhsOperandRegister;
    public RegisterIndex RhsOperandRegister;

    public string InstrName() => "mul.s";
    public string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public string InstrDescription() =>  "Multiply the single values from $rLhs and $rRhs in the FPU and store the value in $rDest.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}

public class DivideSingle : IAssemblyInstruction {
    public RegisterIndex ResultRegister;
    public RegisterIndex LhsOperandRegister;
    public RegisterIndex RhsOperandRegister;

    public string InstrName() => "div.s";
    public string InstrFormat() => InstrName() + " $rDest, $rLhs, $rRhs";
    public string InstrDescription() =>  "Divide the single values from $rLhs and $rRhs in the FPU and store the value in $rDest.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
}
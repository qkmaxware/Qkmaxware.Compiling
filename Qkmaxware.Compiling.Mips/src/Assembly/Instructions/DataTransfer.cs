namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

// https://www.dsi.unive.it/~gasparetto/materials/MIPS_Instruction_Set.pdf

public class LoadWord : IAssemblyInstruction {
    public RegisterIndex ResultRegister;
    public RegisterIndex BaseRegister;
    public uint Offset;

    public string InstrName() => "lw";
    public string InstrFormat() => InstrName() + " $rDest,offset($rBase)";
    public string InstrDescription() =>  "Load 32 bit word from memory address $rBase + offset into $rDest.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string? ToString() {
        return $"lw {this.ResultRegister},{this.Offset}({this.BaseRegister})";
    }
}

public class StoreWord : IAssemblyInstruction {
    public RegisterIndex SourceRegister;
    public RegisterIndex BaseRegister;
    public uint Offset;

    public string InstrName() => "sw";
    public string InstrFormat() => InstrName() + " $rSrc,offset($rBase)";
    public string InstrDescription() =>  "Stores the value in $rSrc into memory at address $rBase + offset.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
    public override string? ToString() {
        return $"sw {this.SourceRegister},{this.Offset}({this.BaseRegister})";
    }
}

public class LoadUpperImmediate : IAssemblyInstruction {
    public RegisterIndex ResultRegister;
    public uint Constant;

    public string InstrName() => "lui";
    public string InstrFormat() => InstrName() + " $rDest, value";
    public string InstrDescription() =>  "Loads the lower 16 bits of an immediate value into the upper 16bits of the register $rDest.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string? ToString() {
        return $"lui {this.ResultRegister},{Constant}";
    }
}

public class LoadAddress : IAssemblyInstruction, IPseudoInstruction {
    public RegisterIndex ResultRegister;
    public string? Label;

    public string InstrName() => "la";
    public string InstrFormat() => InstrName() + " $rDest, label";
    public string InstrDescription() =>  "Loads the computed address of the label into $rDest.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string? ToString() {
        return $"la {this.ResultRegister},{this.Label}";
    }
}

public class LoadImmediate : IAssemblyInstruction, IPseudoInstruction {
    public RegisterIndex ResultRegister;
    public uint Constant;

    public string InstrName() => "li";
    public string InstrFormat() => InstrName() + " $rDest, value";
    public string InstrDescription() =>  "Loads the 32bit immediate value into $rDest.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string? ToString() {
        return $"li {this.ResultRegister},{this.Constant}";
    }
}

public class MoveFromHi : IAssemblyInstruction {
    public RegisterIndex ResultRegister;

    public string InstrName() => "mfhi";
    public string InstrFormat() => InstrName() + " $rDest";
    public string InstrDescription() =>  "Move the value from the special HI register into $rDest.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string? ToString() {
        return $"mfhi {this.ResultRegister}";
    }
}

public class MoveFromLo : IAssemblyInstruction {
    public RegisterIndex ResultRegister;

    public string InstrName() => "mflo";
    public string InstrFormat() => InstrName() + " $rDest";
    public string InstrDescription() =>  "Move the value from the special LO register into $rDest.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string? ToString() {
        return $"mflo {this.ResultRegister}";
    }
}

public class Move : IAssemblyInstruction, IPseudoInstruction {
    public RegisterIndex ResultRegister;
    public RegisterIndex SourceRegister;

    public string InstrName() => "move";
    public string InstrFormat() => InstrName() + " $rDest, $rSrc";
    public string InstrDescription() =>  "Move the value stored in $rSrc into $rDest.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string? ToString() {
        return $"move {this.ResultRegister},{this.SourceRegister}";
    }
}
namespace Qkmaxware.Compiling.Mips.InstructionSet;

// https://www.dsi.unive.it/~gasparetto/materials/MIPS_Instruction_Set.pdf

public class LoadWord : IInstruction {
    public RegisterIndex ResultRegister;
    public RegisterIndex BaseRegister;
    public uint Offset;

    public void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var word = memory.LoadWord(cpu.Registers[this.BaseRegister].Read() + this.Offset);
        cpu.Registers[this.ResultRegister].Write(word);
    }

    public override string? ToString() {
        return $"lw ${this.ResultRegister},${this.Offset}(${this.BaseRegister})";
    }
}

public class StoreWord : IInstruction {
    public RegisterIndex SourceRegister;
    public RegisterIndex BaseRegister;
    public uint Offset;
    public void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        memory.StoreWord(cpu.Registers[BaseRegister].Read() + Offset, cpu.Registers[SourceRegister].Read());
    }

    public override string? ToString() {
        return $"lw ${this.SourceRegister},${this.Offset}(${this.BaseRegister})";
    }
}

public class LoadUpperImmediate : IInstruction {
    public RegisterIndex ResultRegister;
    public uint Constant;

    public void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var upper = this.Constant << 16;
        cpu.Registers[this.ResultRegister].Write(upper);
    }

    public override string? ToString() {
        return $"lui ${this.ResultRegister},${Constant}";
    }
}

public class LoadAddress : IPseudoInstruction {
    public RegisterIndex ResultRegister;
    public string? Label;

    public override string? ToString() {
        return $"la ${this.ResultRegister},${this.Label}";
    }
}

public class LoadImmediate : IPseudoInstruction {
    public RegisterIndex ResultRegister;
    public uint Constant;

    public override string? ToString() {
        return $"li ${this.ResultRegister},${this.Constant}";
    }
}

public class MoveFromHi : IInstruction {
    public RegisterIndex ResultRegister;

    public void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        cpu.Registers[this.ResultRegister].Write(cpu.Registers.HI.Read());
    }

    public override string? ToString() {
        return $"mfhi ${this.ResultRegister}";
    }
}

public class MoveFromLo : IInstruction {
    public RegisterIndex ResultRegister;

    public void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        cpu.Registers[this.ResultRegister].Write(cpu.Registers.LO.Read());
    }

    public override string? ToString() {
        return $"mfhi ${this.ResultRegister}";
    }
}

public class Move : IPseudoInstruction {
    public RegisterIndex ResultRegister;
    public RegisterIndex SourceRegister;

    public override string? ToString() {
        return $"li ${this.ResultRegister},${this.SourceRegister}";
    }
}
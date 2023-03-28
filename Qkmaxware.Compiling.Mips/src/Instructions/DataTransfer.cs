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

public class LoadAddress : IPseudoInstruction {
    
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
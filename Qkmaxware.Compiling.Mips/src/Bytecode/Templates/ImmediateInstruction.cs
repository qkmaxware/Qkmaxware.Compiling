using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Base class for instructions with Immediate type encoding
/// </summary>
public abstract class ImmediateEncodedInstruction : IBytecodeInstruction {
    public abstract uint Opcode {get;}
    public abstract void Invoke(Cpu cpu, Fpu fpu, IMemory memory);
    public abstract uint Encode32();

    protected uint Encode32(uint opcode, uint source, uint target, uint imm) {
        // Encoding
        // ooooooss sssttttt iiiiiiii iiiiiiii
        uint encoded = 0;
        encoded |= (opcode  & 0b111111U) << 26;
        encoded |= (source  & 0b11111U)  << 21;
        encoded |= (target  & 0b11111U)  << 16;
        encoded |= (imm     & 0b11111111_11111111U);
        return encoded;
    }

    public static void Decode32(uint instruction, out uint opcode, out uint source, out uint target, out uint imm) {
        opcode = (instruction >> 26) & 0b111111U;
        source = (instruction >> 21) & 0b11111U;
        target = (instruction >> 16) & 0b11111U;
        imm    = (instruction & 0b11111111_11111111U);
    }
}

/// <summary>
/// Base class for instructions with format o $t, $s, i
/// </summary>
public abstract class ArithLogIInstruction : ImmediateEncodedInstruction {
    public RegisterIndex Source;
    public RegisterIndex Target;
    public uint Immediate;

    public override uint Encode32() {
        return Encode32(this.Opcode, (uint)this.Source, (uint)this.Target, this.Immediate);
    }
}

/// <summary>
/// Base class for instructions with format o $t, i
/// </summary>
public abstract class LoadIInstruction : ImmediateEncodedInstruction {
    public RegisterIndex Target;
    public uint Immediate;

    public override uint Encode32() {
        return Encode32(this.Opcode, 0, (uint)this.Target, this.Immediate);
    }
}

/// <summary>
/// Base class for instructions with format o $s, $t, label
/// </summary>
public abstract class BranchInstruction : ImmediateEncodedInstruction {
    public RegisterIndex Source;
    public RegisterIndex Target;
    public uint Immediate;

    public override uint Encode32() {
        return Encode32(this.Opcode, (uint)this.Source, (uint)this.Target, this.Immediate);
    }
}

/// <summary>
/// Base class for instructions with format o $s, label
/// </summary>
public abstract class BranchZInstruction : ImmediateEncodedInstruction {
    public RegisterIndex Source;
    public uint Immediate;

    public override uint Encode32() {
        return Encode32(this.Opcode, (uint)this.Source, 0, this.Immediate);
    }
}

/// <summary>
/// Base class for instructions with format o $t, i($s)
/// </summary>
public abstract class LoadStoreInstruction : ImmediateEncodedInstruction {
    public RegisterIndex Target;
    public RegisterIndex Source;
    public uint Immediate;

    public override uint Encode32() {
        return Encode32(this.Opcode, (uint)this.Source, (uint)this.Target, this.Immediate);
    }
}
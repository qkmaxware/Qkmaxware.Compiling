namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Base class for instructions with Jump type encoding
/// </summary>
public abstract class JumpEncodedInstruction : IBytecodeInstruction {
    public abstract uint Opcode {get;}
    public abstract void Invoke(Cpu cpu, Fpu fpu, IMemory memory);
    public abstract uint Encode32();

    protected uint Encode32(uint opcode, uint imm) {
        // Register Encoding
        // ooooooii iiiiiiii iiiiiiii iiiiiiii
        uint encoded = 0;
        encoded |= (opcode  & 0b111111U) << 16;
        encoded |= (imm     & 0b00000011_11111111_11111111_11111111U);
        return encoded;
    }
}

/// <summary>
/// Base class for instructions with format o label
/// </summary>
public abstract class JumpInstruction : JumpEncodedInstruction {
    public uint Immediate;

    public override uint Encode32() {
        return Encode32(this.Opcode, this.Immediate);
    }
}

/// <summary>
/// Base class for instructions with format o i
/// </summary>
public abstract class TrapInstruction : JumpEncodedInstruction {
    public uint Immediate;

    public override uint Encode32() {
        return Encode32(this.Opcode, this.Immediate);
    }
}
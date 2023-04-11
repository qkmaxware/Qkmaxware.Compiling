using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Base class for instructions with Jump type encoding
/// </summary>
public abstract class JumpEncodedInstruction : IBytecodeInstruction {
    public abstract uint Opcode {get;}
    public abstract void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io);
    public abstract uint Encode32();

    public abstract IEnumerable<uint> GetOperands();

    protected uint Encode32(uint opcode, uint imm) {
        // Encoding
        // ooooooii iiiiiiii iiiiiiii iiiiiiii
        uint encoded = 0;
        encoded |= (opcode  & 0b111111U) << 26;
        encoded |= (imm     & 0b00000011_11111111_11111111_11111111U);
        return encoded;
    }

    public static void Decode32(uint instruction, out uint opcode, out uint imm) {
        opcode = (instruction >> 26) & 0b111111U;
        imm    = (instruction & 0b00000011_11111111_11111111_11111111U);
    }

    protected static bool TryDecodeBytecode(uint bytecode, uint opcode, out uint immediate) {
        var word = new WordEncoder(bytecode);
        var opcode_check = word.Decode(26..32);
        immediate = 0;
        if (opcode_check != opcode) {
            return false;
        }
        
        immediate = word.Decode(0..26);
        return true;
    }
}

/// <summary>
/// Base class for instructions with format o label
/// </summary>
public abstract class JumpInstruction : JumpEncodedInstruction {
    public uint Immediate;

    public override IEnumerable<uint> GetOperands() {
        yield return Immediate;
    }

    public override uint Encode32() {
        return Encode32(this.Opcode, this.Immediate);
    }
}

/// <summary>
/// Base class for instructions with format o i
/// </summary>
public abstract class TrapInstruction : JumpEncodedInstruction {
    public uint Immediate;

    public override IEnumerable<uint> GetOperands() {
        yield return Immediate;
    }

    public override uint Encode32() {
        return Encode32(this.Opcode, this.Immediate);
    }
}
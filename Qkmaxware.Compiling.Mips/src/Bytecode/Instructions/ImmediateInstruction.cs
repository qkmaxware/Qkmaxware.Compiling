using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Base class for instructions with Immediate type encoding
/// </summary>
public abstract class ImmediateEncodedInstruction : BaseBytecodeInstruction {
    public abstract uint Opcode {get;}

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

    protected static bool TryDecodeBytecode(uint bytecode, uint opcode, out uint source, out uint target, out uint immediate) {
        var word = new WordEncoder(bytecode);
        var opcode_check = word.Decode(26..32);
        source = 0;
        target = 0;
        immediate = 0;
        if (opcode_check != opcode) {
            return false;
        }

        source = word.Decode(21..26);
        target = word.Decode(16..21);
        immediate = word.Decode(0..16);
        return true;
    }
}

/// <summary>
/// Base class for instructions with format o $t, $s, i
/// </summary>
public abstract class ArithLogIInstruction : ImmediateEncodedInstruction {
    public RegisterIndex Source;
    public RegisterIndex Target;
    public uint Immediate;

    public override IEnumerable<uint> GetOperands() {
        yield return (uint)Target;
        yield return (uint)Source;
        yield return Immediate;
    }

    public override uint Encode32() {
        return Encode32(this.Opcode, (uint)this.Source, (uint)this.Target, this.Immediate);
    }
    /// <summary>
    /// Print this instruction as MIPS assembly code
    /// </summary>
    /// <returns>assembly string</returns>
    public override string ToAssemblyString() => $"{this.InstructionName()} {this.Target}, {this.Source}, {this.Immediate}";
}

/// <summary>
/// Base class for instructions with format o $t, i
/// </summary>
public abstract class LoadIInstruction : ImmediateEncodedInstruction {
    public RegisterIndex Target;
    public uint Immediate;

    public override IEnumerable<uint> GetOperands() {
        yield return (uint)Target;
        yield return Immediate;
    }

    public override uint Encode32() {
        return Encode32(this.Opcode, 0, (uint)this.Target, this.Immediate);
    }

    /// <summary>
    /// Print this instruction as MIPS assembly code
    /// </summary>
    /// <returns>assembly string</returns>
    public override string ToAssemblyString() => $"{this.InstructionName()} {this.Target}, {this.Immediate}";
}

/// <summary>
/// Base class for instructions with format o $s, $t, label
/// </summary>
public abstract class BranchInstruction : ImmediateEncodedInstruction {
    public RegisterIndex Source;
    public RegisterIndex Target;
    public uint Immediate;

    public override IEnumerable<uint> GetOperands() {
        yield return (uint)Source;
        yield return (uint)Target;
        yield return Immediate;
    }

    public override uint Encode32() {
        return Encode32(this.Opcode, (uint)this.Source, (uint)this.Target, this.Immediate);
    }

    /// <summary>
    /// Print this instruction as MIPS assembly code
    /// </summary>
    /// <returns>assembly string</returns>
    public override string ToAssemblyString() => $"{this.InstructionName()} {this.Source}, {this.Target}, {this.Immediate}";
}

/// <summary>
/// Base class for instructions with format o $s, label
/// </summary>
public abstract class BranchZInstruction : ImmediateEncodedInstruction {
    public RegisterIndex Source;
    public uint Immediate;

    public override IEnumerable<uint> GetOperands() {
        yield return (uint)Source;
        yield return Immediate;
    }

    public override uint Encode32() {
        return Encode32(this.Opcode, (uint)this.Source, 0, this.Immediate);
    }

    /// <summary>
    /// Print this instruction as MIPS assembly code
    /// </summary>
    /// <returns>assembly string</returns>
    public override string ToAssemblyString() => $"{this.InstructionName()} {this.Source}, {this.Immediate}";
}

/// <summary>
/// Base class for instructions with format o $t, i($s)
/// </summary>
public abstract class LoadStoreInstruction : ImmediateEncodedInstruction {
    public RegisterIndex Target;
    public RegisterIndex Source;
    public uint Immediate;

    public override IEnumerable<uint> GetOperands() {
        yield return (uint)Target;
        yield return Immediate;
        yield return (uint)Source;
    }

    public override uint Encode32() {
        return Encode32(this.Opcode, (uint)this.Source, (uint)this.Target, this.Immediate);
    }

    /// <summary>
    /// Print this instruction as MIPS assembly code
    /// </summary>
    /// <returns>assembly string</returns>
    public override string ToAssemblyString() => $"{this.InstructionName()} {this.Target}, {this.Immediate}({this.Source})";
}
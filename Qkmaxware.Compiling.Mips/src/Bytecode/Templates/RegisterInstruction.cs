using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Base class for instructions with Register type encoding
/// </summary>
public abstract class RegisterEncodedInstruction : IBytecodeInstruction {
    public abstract uint Opcode {get;}
    public abstract void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io);
    public abstract uint Encode32();

    public abstract IEnumerable<uint> GetOperands();

    protected uint Encode32(uint opcode, uint source, uint target, uint dest, uint amount, uint function) {
        // Encoding
        // 000000ss sssttttt dddddaaa aaffffff
        return new WordEncoder()
            .Encode(opcode, 26..32)
            .Encode(source, 21..26)
            .Encode(target, 16..21)
            .Encode(dest, 11..16)
            .Encode(amount, 6..11)
            .Encode(function, 0..6)
            .Encoded;
    }

    protected static bool TryDecodeBytecode(uint bytecode, out uint opcode, out uint source, out uint target, out uint dest, out uint amount, uint function) {
        var word = new WordEncoder(bytecode);
        opcode = word.Decode(26..32);
        uint function_check = word.Decode(0..6);
        source = 0;
        target = 0;
        dest = 0;
        amount = 0;
        if (opcode != 0 || function_check != function) {
            return false;
        }

        source = word.Decode(21..26);
        target = word.Decode(16..21);
        dest = word.Decode(11..16);
        amount = word.Decode(6..11);
        return true;
    }

}

/// <summary>
/// Base class for instructions with format f $d, $s, $t
/// </summary>
public abstract class ArithLogInstruction : RegisterEncodedInstruction {
    public RegisterIndex Destination;
    public RegisterIndex Source;
    public RegisterIndex Target;

    public override IEnumerable<uint> GetOperands() {
        yield return (uint)Destination;
        yield return (uint)Source;
        yield return (uint)Target;
    }

    public override uint Encode32() {
        return Encode32(0, (uint)Source, (uint)Target, (uint)Destination, 0, this.Opcode);
    }
}

/// <summary>
/// Base class for instructions with format f $s, $t
/// </summary>
public abstract class DivMultInstruction : RegisterEncodedInstruction {
    public RegisterIndex Source;
    public RegisterIndex Target;

    public override IEnumerable<uint> GetOperands() {
        yield return (uint)Source;
        yield return (uint)Target;
    }

    public override uint Encode32() {
        return Encode32(0, (uint)Source, (uint)Target, 0, 0, this.Opcode);
    }
}

/// <summary>
/// Base class for instructions with format f $d, $t, a
/// </summary>
public abstract class ShiftInstruction : RegisterEncodedInstruction {
    public RegisterIndex Destination;
    public RegisterIndex Target;
    public uint Amount;

    public override IEnumerable<uint> GetOperands() {
        yield return (uint)Destination;
        yield return (uint)Target;
        yield return (uint)Amount;
    }

    public override uint Encode32() {
        return Encode32(0, 0, (uint)Target, (uint)Destination, this.Amount, this.Opcode);
    }
}

/// <summary>
/// Base class for instructions with format f $d, $t, $s
/// </summary>
public abstract class ShiftVInstruction : RegisterEncodedInstruction {
    public RegisterIndex Destination;
    public RegisterIndex Target;
    public RegisterIndex Source;

    public override IEnumerable<uint> GetOperands() {
        yield return (uint)Destination;
        yield return (uint)Target;
        yield return (uint)Source;
    }

    public override uint Encode32() {
        return Encode32(0, (uint)Source, (uint)Target, (uint)Destination, 0, this.Opcode);
    }
}

/// <summary>
/// Base class for instructions with format f $s
/// </summary>
public abstract class JumpRInstruction : RegisterEncodedInstruction {
    public RegisterIndex Source;

    public override IEnumerable<uint> GetOperands() {
        yield return (uint)Source;
    }

    public override uint Encode32() {
        return Encode32(0, (uint)Source, 0, 0, 0, this.Opcode);
    }
}

/// <summary>
/// Base class for instructions with format f $d
/// </summary>
public abstract class MoveFromInstruction : RegisterEncodedInstruction {
    public RegisterIndex Destination;

    public override IEnumerable<uint> GetOperands() {
        yield return (uint)Destination;
    }

    public override uint Encode32() {
        return Encode32(0, 0, 0, (uint)Destination, 0, this.Opcode);
    }
}

/// <summary>
/// Base class for instructions with format f $s
/// </summary>
public abstract class MoveToInstruction : RegisterEncodedInstruction {
    public RegisterIndex Source;

    public override IEnumerable<uint> GetOperands() {
        yield return (uint)Source;
    }

    public override uint Encode32() {
        return Encode32(0, (uint)Source, 0, 0, 0, this.Opcode);
    }
}
using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Base class for instructions with Register type encoding
/// </summary>
public abstract class RegisterEncodedInstruction : IBytecodeInstruction {
    public abstract uint Opcode {get;}
    public abstract void Invoke(Cpu cpu, Fpu fpu, IMemory memory);
    public abstract uint Encode32();

    protected uint Encode32(uint source, uint target, uint dest, uint amount, uint function) {
        // Encoding
        // 000000ss sssttttt dddddaaa aaffffff
        uint encoded = 0;
        encoded |= (source  & 0b11111U) << 21;
        encoded |= (target  & 0b11111U) << 16;
        encoded |= (dest    & 0b11111U) << 11;
        encoded |= (amount  & 0b11111U) << 6;
        encoded |= function & 0b111111U;
        return encoded;
    }

    public static void Decode32(uint instruction, out uint source, out uint target, out uint dest, out uint amount, out uint function) {
        source = (instruction >> 21) & 0b11111U;
        target = (instruction >> 16) & 0b11111U;
        dest   = (instruction >> 11) & 0b11111U;
        amount = (instruction >> 6)  & 0b11111U;
        function=(instruction)       & 0b111111U;
    }
}

/// <summary>
/// Base class for instructions with format f $d, $s, $t
/// </summary>
public abstract class ArithLogInstruction : RegisterEncodedInstruction {
    public RegisterIndex Destination;
    public RegisterIndex Source;
    public RegisterIndex Target;

    public override uint Encode32() {
        return Encode32((uint)Source, (uint)Target, (uint)Destination, 0, this.Opcode);
    }
}

/// <summary>
/// Base class for instructions with format f $s, $t
/// </summary>
public abstract class DivMultInstruction : RegisterEncodedInstruction {
    public RegisterIndex Source;
    public RegisterIndex Target;

    public override uint Encode32() {
        return Encode32((uint)Source, (uint)Target, 0, 0, this.Opcode);
    }
}

/// <summary>
/// Base class for instructions with format f $s, $t, a
/// </summary>
public abstract class ShiftInstruction : RegisterEncodedInstruction {
    public RegisterIndex Destination;
    public RegisterIndex Target;
    public uint Amount;

    public override uint Encode32() {
        return Encode32(0, (uint)Target, (uint)Destination, this.Amount, this.Opcode);
    }
}

/// <summary>
/// Base class for instructions with format f $d, $t, $s
/// </summary>
public abstract class ShiftVInstruction : RegisterEncodedInstruction {
    public RegisterIndex Destination;
    public RegisterIndex Target;
    public RegisterIndex Source;

    public override uint Encode32() {
        return Encode32((uint)Source, (uint)Target, (uint)Destination, 0, this.Opcode);
    }
}

/// <summary>
/// Base class for instructions with format f $s
/// </summary>
public abstract class JumpRInstruction : RegisterEncodedInstruction {
    public RegisterIndex Source;

    public override uint Encode32() {
        return Encode32((uint)Source, 0, 0, 0, this.Opcode);
    }
}

/// <summary>
/// Base class for instructions with format f $d
/// </summary>
public abstract class MoveFromInstruction : RegisterEncodedInstruction {
    public RegisterIndex Destination;

    public override uint Encode32() {
        return Encode32(0, 0, (uint)Destination, 0, this.Opcode);
    }
}

/// <summary>
/// Base class for instructions with format f $s
/// </summary>
public abstract class MoveToInstruction : RegisterEncodedInstruction {
    public RegisterIndex Source;

    public override uint Encode32() {
        return Encode32((uint)Source, 0, 0, 0, this.Opcode);
    }
}
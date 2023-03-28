using System;

namespace Qkmaxware.Compiling.Mips.InstructionSet;

public interface IAssembleable {

}

/// <summary>
/// Base interface for all MIPS instructions
/// </summary>
public interface IInstruction : IAssembleable {
    /// <summary>
    /// Perform this instruction on the given simulator
    /// </summary>
    /// <param name="cpu">mips cpu</param>
    /// <param name="fpu">mips fpu</param>
    /// <param name="memory">permanent storage</param>
    public void Invoke(Cpu cpu, Fpu fpu, IMemory memory);
    /// <summary>
    /// Pretty-print the instruction
    /// </summary>
    /// <returns>string</returns>
    public string? ToString();
}

/// <summary>
/// Base interface for a pseudo instruction handled by the assembler
/// </summary>
public interface IPseudoInstruction : IAssembleable {

}

/// <summary>
/// Base class for all instructions with shared functionality
/// </summary>
public abstract class BaseInstruction : IInstruction {
    public abstract string InstrName();
    public abstract void Invoke(Cpu cpu, Fpu fpu, IMemory memory);

    protected Int32 i32(Register<uint> reg) {
        return BitConverter.ToInt32(BitConverter.GetBytes(reg.Read()));
    }

    protected UInt32 u32(Register<uint> reg) {
        return (reg.Read());
    }

    protected Single f32(Register<uint> reg) {
        return BitConverter.ToSingle(BitConverter.GetBytes(reg.Read()));
    }

    protected Single f32(Register<float> reg) {
        return reg.Read();
    }

    protected UInt32 u32(Register<float> reg) {
        return BitConverter.ToUInt32(BitConverter.GetBytes(reg.Read()));
    }

    protected UInt32 u32(UInt32 i) {
        return BitConverter.ToUInt32(BitConverter.GetBytes(i));
    }

    protected UInt32 u32(int i) {
        return BitConverter.ToUInt32(BitConverter.GetBytes(i));
    }

    protected UInt64 u64(double d) {
        return BitConverter.ToUInt32(BitConverter.GetBytes(d));
    }
}

/// <summary>
/// Base class for binary instructions
/// </summary>
public abstract class ThreeAddressInstruction : BaseInstruction {
    public RegisterIndex ResultRegister;
    public RegisterIndex LhsOperandRegister;
    public RegisterIndex RhsOperandRegister;

    public override string ToString() {
        return $"${InstrName()} ${this.ResultRegister},${this.LhsOperandRegister},${this.RhsOperandRegister}";
    }
}

/// <summary>
/// Base class for binary instructions
/// </summary>
public abstract class TwoAddressImmediateInstruction<T> : BaseInstruction {
    public RegisterIndex ResultRegister;
    public RegisterIndex LhsOperandRegister;
    public T? RhsOperand;

    public override string ToString() {
        return $"${InstrName()} ${this.ResultRegister},${this.LhsOperandRegister},${this.RhsOperand}";
    }
}

/// <summary>
/// 
/// </summary>
public abstract class OneAddressImmediateInstruction<T> : BaseInstruction {
    public RegisterIndex ResultRegister;
    public T? RhsOperand;

    public override string ToString() {
        return $"${InstrName()} ${this.ResultRegister},${this.RhsOperand}";
    }
}


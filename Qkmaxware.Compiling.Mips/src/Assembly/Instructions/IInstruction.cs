using System;

namespace Qkmaxware.Compiling.Mips.Assembly;

/// <summary>
/// Base interface for all MIPS instructions
/// </summary>
public interface IAssemblyInstruction {
    public void Visit(IInstructionVisitor visitor);
    public T Visit<T>(IInstructionVisitor<T> visitor);
    /// <summary>
    /// Pretty-print the instruction
    /// </summary>
    /// <returns>string</returns>
    public string? ToString();
}

/// <summary>
/// Base class for all instructions with shared functionality
/// </summary>
public abstract class BaseAssemblyInstruction : IAssemblyInstruction {
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

    public abstract void Visit(IInstructionVisitor visitor);
    public abstract T Visit<T>(IInstructionVisitor<T> visitor);

    public override string ToString() => InstrName();
}

/// <summary>
/// Base class for binary instructions
/// </summary>
public abstract class TwoAddressBinaryInstruction : BaseAssemblyInstruction {
    public RegisterIndex LhsOperandRegister;
    public RegisterIndex RhsOperandRegister;

    public override string ToString() {
        return $"{InstrName()} {this.LhsOperandRegister},{this.RhsOperandRegister}";
    }
}

/// <summary>
/// Base class for binary instructions
/// </summary>
public abstract class ThreeAddressInstruction : BaseAssemblyInstruction {
    public RegisterIndex ResultRegister;
    public RegisterIndex LhsOperandRegister;
    public RegisterIndex RhsOperandRegister;

    public override string ToString() {
        return $"{InstrName()} {this.ResultRegister},{this.LhsOperandRegister},{this.RhsOperandRegister}";
    }
}

/// <summary>
/// Base class for binary instructions
/// </summary>
public abstract class TwoAddressImmediateInstruction<T> : BaseAssemblyInstruction {
    public RegisterIndex ResultRegister;
    public RegisterIndex LhsOperandRegister;
    public T? RhsOperand;

    public override string ToString() {
        return $"{InstrName()} {this.ResultRegister},{this.LhsOperandRegister},{this.RhsOperand}";
    }
}

/// <summary>
/// 
/// </summary>
public abstract class OneAddressImmediateInstruction<T> : BaseAssemblyInstruction {
    public RegisterIndex ResultRegister;
    public T? RhsOperand;

    public override string ToString() {
        return $"{InstrName()} {this.ResultRegister},{this.RhsOperand}";
    }
}


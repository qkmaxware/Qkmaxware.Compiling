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


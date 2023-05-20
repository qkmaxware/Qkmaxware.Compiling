using Qkmaxware.Compiling.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// Base class for operands to tuple operations
/// </summary>
public abstract class TupleOperand {
    /// <summary>
    /// The type of value produced by this operand
    /// </summary>
    public abstract IrType TypeOf();
    /// <summary>
    /// Internal operand constructor to prevent unwanted sub-classing
    /// </summary>
    internal TupleOperand() {}
    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public abstract string PrintString();
}

/// <summary>
/// Base class for operands that act as a reference to something else in the code
/// </summary>
public abstract class ReferenceOperand : TupleOperand {
    /// <summary>
    /// Internal reference operand constructor to prevent unwanted sub-classing
    /// </summary>
    internal ReferenceOperand() {}
}

/// <summary>
/// Base class for operands that can be used as a value in an expression
/// </summary>
public abstract class ValueOperand : TupleOperand {
    /// <summary>
    /// Internal value operand constructor to prevent unwanted sub-classing
    /// </summary>
    internal ValueOperand() {}
}

/// <summary>
/// Base class for operands that are literal values
/// </summary>
public abstract class Literal : ValueOperand {}

/// <summary>
/// Base class for operands that are literal values and have a value defined by a C# type
/// </summary>
/// <typeparam name="T">value type</typeparam>
public abstract class Literal<T> : Literal {
    /// <summary>
    /// Value of the literal
    /// </summary>
    /// <value>value</value>
    public T Value {get; private set;}

    protected Literal(T value) {
        this.Value = value;
    }
}
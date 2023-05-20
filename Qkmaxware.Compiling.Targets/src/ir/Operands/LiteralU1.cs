using Qkmaxware.Compiling.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// A 32bit unsigned integer
/// </summary>
public sealed partial class LiteralU1 : Literal<uint> {
    /// <summary>
    /// The type of value produced by this operand
    /// </summary>
    public override IrType TypeOf() => IrType.U1;
    /// <summary>
    /// Create a new literal
    /// </summary>
    /// <param name="i">value of the literal</param>
    public LiteralU1(uint i) : base(Math.Min(1, i)) {}
    /// <summary>
    /// Create a new literal
    /// </summary>
    /// <param name="i">value of the literal</param>
    public LiteralU1(bool i) : base(i ? 1U : 0U) {}

    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public override string PrintString() => Value.ToString();

    public override string ToString() => Value.ToString();
}
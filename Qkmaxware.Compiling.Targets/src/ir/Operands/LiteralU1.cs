using Qkmaxware.Compiling.Targets.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Targets.Ir;

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

    public override string ToString() => Value.ToString();
}
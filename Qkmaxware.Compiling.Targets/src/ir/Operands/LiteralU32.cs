using Qkmaxware.Compiling.Targets.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Targets.Ir;

/// <summary>
/// A 32bit unsigned integer
/// </summary>
public sealed partial class LiteralU32 : Literal<uint> {
    /// <summary>
    /// The type of value produced by this operand
    /// </summary>
    public override IrType TypeOf() => IrType.U32;
    /// <summary>
    /// Create a new literal
    /// </summary>
    /// <param name="i">value of the literal</param>
    public LiteralU32(uint i) : base(i) {}

    public override string ToString() => Value.ToString();
}
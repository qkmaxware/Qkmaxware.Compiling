using Qkmaxware.Compiling.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// A 32bit integer
/// </summary>
public sealed partial class LiteralI32 : Literal<int> {
    /// <summary>
    /// The type of value produced by this operand
    /// </summary>
    public override IrType TypeOf() => IrType.I32;
    /// <summary>
    /// Create a new literal
    /// </summary>
    /// <param name="i">value of the literal</param>
    public LiteralI32(int i) : base(i) {}

    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public override string PrintString() => Value.ToString();

    public override string ToString() => Value.ToString();
}
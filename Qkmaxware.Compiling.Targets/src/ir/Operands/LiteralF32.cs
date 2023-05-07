using Qkmaxware.Compiling.Targets.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Targets.Ir;

/// <summary>
/// A 32bit floating point value
/// </summary>
public sealed partial class LiteralF32 : Literal<float> {
    /// <summary>
    /// The type of value produced by this operand
    /// </summary>
    public override IrType TypeOf() => IrType.F32;
    /// <summary>
    /// Create a new literal
    /// </summary>
    /// <param name="i">value of the literal</param>
    public LiteralF32(float i) : base(i) {}

    public override string ToString() => Value.ToString();
}

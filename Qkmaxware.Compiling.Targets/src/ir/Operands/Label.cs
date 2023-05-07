using Qkmaxware.Compiling.Targets.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Targets.Ir;

/// <summary>
/// A label to a jump point
/// </summary>
public sealed partial class Label : ReferenceOperand {
    /// <summary>
    /// The type of value produced by this operand
    /// </summary>
    public override IrType TypeOf() => IrType.U32;
}

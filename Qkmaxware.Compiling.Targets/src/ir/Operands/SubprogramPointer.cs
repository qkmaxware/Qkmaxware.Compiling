using Qkmaxware.Compiling.Targets.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Targets.Ir;

/// <summary>
/// A reference to a subroutine
/// </summary>
public sealed partial class SubprogramPointer : ReferenceOperand {
    /// <summary>
    /// The type of value produced by this operand
    /// </summary>
    public override IrType TypeOf() => IrType.U32;
}
namespace Qkmaxware.Compiling.Targets.Ir.TypeSystem;

public class F32ToU32 : TypeConversion {
    public override IrType From => IrType.F32;
    public override IrType To   => IrType.U32;
    public override void GenerateInstructions(IConversionMapping conversions) => conversions.Convert(this);
}
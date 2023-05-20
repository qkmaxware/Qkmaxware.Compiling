namespace Qkmaxware.Compiling.Ir.TypeSystem;

public class F32ToI32 : TypeConversion {
    public override IrType From => IrType.F32;
    public override IrType To   => IrType.I32;
    public override void GenerateInstructions(IConversionMapping conversions) => conversions.Convert(this);
}
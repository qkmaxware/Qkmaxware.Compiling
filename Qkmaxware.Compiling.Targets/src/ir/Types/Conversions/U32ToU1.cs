namespace Qkmaxware.Compiling.Ir.TypeSystem;

public class U32ToU1 : TypeConversion {
    public override IrType From => IrType.U32;
    public override IrType To   => IrType.U1;
    public override void GenerateInstructions(IConversionMapping conversions) => conversions.Convert(this);
}
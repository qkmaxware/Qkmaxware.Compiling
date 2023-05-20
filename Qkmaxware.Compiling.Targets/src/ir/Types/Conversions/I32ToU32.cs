namespace Qkmaxware.Compiling.Ir.TypeSystem;

public class I32ToU32 : TypeConversion {
    public override IrType From => IrType.I32;
    public override IrType To   => IrType.U32;
    public override void GenerateInstructions(IConversionMapping conversions) => conversions.Convert(this);
}
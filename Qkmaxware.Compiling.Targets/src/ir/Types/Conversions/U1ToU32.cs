namespace Qkmaxware.Compiling.Targets.Ir.TypeSystem;

public class U1ToU32 : TypeConversion {
    public override IrType From => IrType.U1;
    public override IrType To   => IrType.U32;
    public override void GenerateInstructions(IConversionMapping conversions) => conversions.Convert(this);
}
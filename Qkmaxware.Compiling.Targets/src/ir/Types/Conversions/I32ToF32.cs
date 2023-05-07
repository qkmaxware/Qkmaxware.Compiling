namespace Qkmaxware.Compiling.Targets.Ir.TypeSystem;

public class I32ToF32 : TypeConversion {
    public override IrType From => IrType.I32;
    public override IrType To   => IrType.F32;
    public override void GenerateInstructions(IConversionMapping conversions) => conversions.Convert(this);
}
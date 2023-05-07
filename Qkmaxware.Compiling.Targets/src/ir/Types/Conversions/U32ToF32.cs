namespace Qkmaxware.Compiling.Targets.Ir.TypeSystem;

public class U32ToF32 : TypeConversion {
    public override IrType From => IrType.U32;
    public override IrType To   => IrType.F32;
    public override void GenerateInstructions(IConversionMapping conversions) => conversions.Convert(this);
}
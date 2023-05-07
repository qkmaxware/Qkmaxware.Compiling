namespace Qkmaxware.Compiling.Targets.Ir.TypeSystem;

public interface IConversionMapping {
    public void Convert(I32ToF32 conv);
    public void Convert(F32ToI32 conv);
    public void Convert(U32ToF32 conv);
    public void Convert(F32ToU32 conv);
    public void Convert(U1ToU32 conv);
    public void Convert(U32ToU1 conv);
    public void Convert(U32ToI32 conv);
    public void Convert(I32ToU32 conv);
}
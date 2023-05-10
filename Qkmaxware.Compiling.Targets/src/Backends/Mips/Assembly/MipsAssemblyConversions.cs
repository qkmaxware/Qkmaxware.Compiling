using Qkmaxware.Compiling.Targets.Ir;
using Qkmaxware.Compiling.Targets.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Targets.Mips.Assembly;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

/// <summary>
/// Type conversions for MIPS assembly code
/// </summary>
internal class MipsAssemblyConversions : IConversionMapping {

    private TextSection code;
    private RegisterIndex toConvert;

    public MipsAssemblyConversions(TextSection code, RegisterIndex toConvert) {
        this.code = code;
        this.toConvert = toConvert;
    }

    public void Convert(I32ToF32 conv) {
        // Natively supported
        throw new NotImplementedException();
    }

    public void Convert(F32ToI32 conv) {
        // Natively supported
        throw new NotImplementedException();
    }

    public void Convert(U32ToF32 conv) {
        // Kinda natively supported, same as I32ToF32
        throw new NotImplementedException();
    }

    public void Convert(F32ToU32 conv) {
        // IF float < 0 SET 0
        // ELSE CONVERT TO I32 == U32
        throw new NotImplementedException();
    }

    public void Convert(U1ToU32 conv) {
        throw new NotImplementedException();
    }

    public void Convert(U32ToU1 conv) {
        throw new NotImplementedException();
    }

    public void Convert(U32ToI32 conv) {
        throw new NotImplementedException();
    }

    public void Convert(I32ToU32 conv) {
        throw new NotImplementedException();
    }
}
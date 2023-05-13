using Qkmaxware.Compiling.Targets.Ir;
using Qkmaxware.Compiling.Targets.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Targets.C;

internal class CConversionMapping : IConversionMapping {

    private TextWriter code;

    public CConversionMapping(TextWriter writer) {
        this.code = writer;
    }

    public void Convert(I32ToF32 conv) {
        code.Write("(float)");
    }

    public void Convert(F32ToI32 conv){
        code.Write("(int)");
    }

    public void Convert(U32ToF32 conv) {
        code.Write("(float)");
    }

    public void Convert(F32ToU32 conv) {
        code.Write("(uint)");
    }

    public void Convert(U1ToU32 conv) {}

    public void Convert(U32ToU1 conv) {}

    public void Convert(U32ToI32 conv) {
        code.Write("(int)");
    }

    public void Convert(I32ToU32 conv) {
        code.Write("(uint)");
    }
}

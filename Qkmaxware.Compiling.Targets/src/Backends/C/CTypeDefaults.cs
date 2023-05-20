using Qkmaxware.Compiling.Ir;
using Qkmaxware.Compiling.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Targets.C;

public class CTypeDefaults : IIrTypeVisitor {

    public TextWriter writer;
    public CTypeDefaults(TextWriter writer) {
        this.writer = writer;
    }

    public void Accept(F32 type) {
        writer.Write(IrType.F32.DefaultValue());
    }

    public void Accept(I32 type) {
        writer.Write(IrType.I32.DefaultValue());
    }

    public void Accept(U32 type) {
        writer.Write(IrType.U32.DefaultValue());
    }

    public void Accept(U1 type) {
        writer.Write(IrType.U1.DefaultValue());
    }
}
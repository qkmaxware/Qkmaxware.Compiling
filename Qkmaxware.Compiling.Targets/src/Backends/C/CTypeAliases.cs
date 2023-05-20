using Qkmaxware.Compiling.Ir;
using Qkmaxware.Compiling.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Targets.C;

public class CTypeAliases : IIrTypeVisitor {

    public TextWriter writer;
    public CTypeAliases(TextWriter writer) {
        this.writer = writer;
    }

    public void Accept(F32 type) {
        writer.Write("float");
    }

    public void Accept(I32 type) {
        writer.Write("int");
    }

    public void Accept(U32 type) {
        writer.Write("uint");
    }

    public void Accept(U1 type) {
        writer.Write("short");
    }
}
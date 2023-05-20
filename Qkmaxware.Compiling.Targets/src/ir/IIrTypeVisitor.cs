using Qkmaxware.Compiling.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Targets.Mips.Assembly;

namespace Qkmaxware.Compiling.Ir;

public interface IIrTypeVisitor {
    public void Accept(F32 type);
    public void Accept(I32 type);
    public void Accept(U32 type);
    public void Accept(U1 type);
}
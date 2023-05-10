using Qkmaxware.Compiling.Targets.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Targets.Mips.Assembly;

namespace Qkmaxware.Compiling.Targets.Ir;

public interface IIrTypeVisitor {
    public void Accept(F32 type);
    public void Accept(I32 type);
    public void Accept(U32 type);
    public void Accept(U1 type);
}
using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Targets.Mips.Assembly;

namespace Qkmaxware.Compiling.Targets.Ir;

public interface ModuleVisitor {
    public void VisitModule(Module module);
    public void VisitGlobalDeclaration(Global def);
    public void VisitSubprogram(Subprogram sub);
    public void VisitLocalDeclaration(Local def);
}
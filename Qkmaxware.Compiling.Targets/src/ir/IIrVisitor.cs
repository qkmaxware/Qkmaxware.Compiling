using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Targets.Mips.Assembly;

namespace Qkmaxware.Compiling.Ir;

public interface ModuleVisitor {
    // Provide a default implementation 
    public void VisitModule(Module module);
    public void VisitGlobalDeclaration(Global def);
    public void VisitSubprogram(Subprogram sub);
    public void VisitLocalDeclaration(Local def);
}
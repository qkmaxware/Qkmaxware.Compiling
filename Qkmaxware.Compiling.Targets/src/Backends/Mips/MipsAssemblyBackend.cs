using Qkmaxware.Compiling.Targets.Ir;
using Qkmaxware.Compiling.Targets.Mips.Assembly;

namespace Qkmaxware.Compiling.Targets.Mips;

public class MipsAssemblyBackend : IBackendTargetModule<Mips.Assembly.AssemblyProgram>, IBackendTargetFile {
    public FileInfo TryEmitToFile(Module module, string path_like) {
        if (!path_like.EndsWith(".asm"))
            path_like = path_like + ".asm";

        using (var writer = new StreamWriter(path_like)) {
            var prog = TryTransform(module);
            var encoder = new AssemblyWriter(writer);
            encoder.Emit(prog);
        }

        return new FileInfo(path_like);
    }

    public AssemblyProgram TryTransform(Module module) {
        var data = new DataSection();
        var text = new TextSection();
        Mips.Assembly.AssemblyProgram prog = new AssemblyProgram(data, text);
        var visitor = new MipsAssemblyVisitor(data, text);

        visitor.VisitModule(module);

        return prog;
    }
}
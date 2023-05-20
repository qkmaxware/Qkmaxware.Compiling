using Qkmaxware.Compiling.Ir;
using Qkmaxware.Compiling.Targets.Mips.Assembly;
using Qkmaxware.Compiling.Targets.Mips.Bytecode;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public class MipsBytecodeBackend : IBackendTargetModule<BytecodeProgram>, IBackendTargetFile {
    
    private MipsAssemblyBackend asmbackend = new MipsAssemblyBackend();
    private Mips.Assembler assembler = new Assembler();

    public FileInfo TryEmitToFile(Module module, string path_like) {
        if (!path_like.EndsWith(".mips"))
            path_like = path_like + ".mips";

        var prog = TryTransform(module);
        prog.DumpToFile(path_like);

        return new FileInfo(path_like);
    }

    public BytecodeProgram TryTransform(Module module) {
        var asm = asmbackend.TryTransform(module);
        return assembler.Assemble(asm);
    }
}
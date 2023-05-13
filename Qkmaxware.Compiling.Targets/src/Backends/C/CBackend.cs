using Qkmaxware.Compiling.Targets.Ir;

namespace Qkmaxware.Compiling.Targets.C;

public partial class CBackend : IBackendTargetFile {

    public static readonly string Tab = "    ";

    public FileInfo TryEmitToFile(Module module, string path_like) {
        if (path_like == null)
            throw new ArgumentException("File path cannot be null");

        var hpath = path_like.EndsWith(".h") || path_like.EndsWith(".hpp") || path_like.EndsWith(".hxx") 
                  ? path_like 
                  : path_like + ".h";
        var hname = Path.GetFileName(hpath);
        var ipath = path_like.EndsWith(".cpp") || path_like.EndsWith(".cxx") || path_like.EndsWith(".cc") 
                  ? path_like 
                  : path_like + ".cpp";

        var guard = "MODULE_" + DateTime.Now.ToString("DyyyyMMddTHHmmss") + "_H";

        using (var hwriter = new StreamWriter(hpath))
        using (var iwriter = new StreamWriter(ipath)) {
            // Write guard in header
            hwriter.WriteLine($"#ifndef {guard}");
            hwriter.WriteLine($"#define {guard}");
            hwriter.WriteLine();
            hwriter.Write(stdlib);
            hwriter.WriteLine();

            // Write include in implementation
            iwriter.WriteLine($"#include <stdio.h>");
            iwriter.WriteLine($"#include <stdlib.h>");
            iwriter.WriteLine($"#include <math.h>");
            iwriter.WriteLine($"#include \"{hname}\"");
            iwriter.WriteLine();

            new CModuleVisitor(hwriter, iwriter).VisitModule(module);

            // Close guard in header
            hwriter.WriteLine($"#endif");
        }

        return new FileInfo(ipath);
    }
}
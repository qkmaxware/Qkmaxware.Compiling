using Qkmaxware.Compiling.Ir;
using Qkmaxware.Compiling.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Targets.C;

internal class CModuleVisitor : ModuleVisitor {

    public TextWriter header;
    public TextWriter implementation;

    public static string GenerateFuncName(Subprogram sub) {
        return $" func_{sub.ProcedureIndex}";
    }

    public CModuleVisitor(TextWriter h, TextWriter cpp) {
        this.header = h;
        this.implementation = cpp;
    }

    public void VisitGlobalDeclaration(Global def) {
        var name = def.Name;
        var type = def.TypeOf();
        var index = def.VariableIndex;

        header.Write("// ");
        header.WriteLine(def.Name);
        type.Visit(new CTypeAliases(header)); header.Write($" global_{index} = "); type.Visit(new CTypeDefaults(header)); header.WriteLine(";");
    }

    public void VisitModule(Module module) {
        foreach (var global in module.Globals) {
            if (global is Global g)
                VisitGlobalDeclaration(g);
        }
        foreach (var subroutine in module.Subprograms) {
            VisitSubprogram(subroutine);
        }
    }

    public void VisitSubprogram(Subprogram sub) {
        if (sub.Name != null) {
            header.Write("// ");
            header.WriteLine(sub.Name);
        }
        {
            var aliases = new CTypeAliases(header);
            if (sub.ReturnLocal != null) {
                sub.ReturnLocal.TypeOf().Visit(aliases);
            } else {
                header.Write("void");
            }
            header.Write(GenerateFuncName(sub));

            header.Write('(');
            bool first = true;
            foreach (var arg in sub.Arguments) {
                if (!first)
                    header.Write(", ");
                arg.TypeOf().Visit(aliases);
                first = false;
            }
            header.WriteLine(");");
        } 
        if (sub.Name != null) {
            implementation.Write("// ");
            implementation.WriteLine(sub.Name);
        }
        {
            var aliases = new CTypeAliases(implementation);
            if (sub.ReturnLocal != null) {
                sub.ReturnLocal.TypeOf().Visit(aliases);
            } else {
                implementation.Write("void");
            }
            implementation.Write(GenerateFuncName(sub)); 

            implementation.Write('(');
            bool first = true;
            foreach (var arg in sub.Arguments) {
                if (!first)
                    implementation.Write(", ");
                arg.TypeOf().Visit(aliases); implementation.Write(" v"); implementation.Write(arg.Name);
                first = false;
            }
            implementation.WriteLine("){");

            foreach (var local in sub.Locals) {
                if (local is Local l)
                    VisitLocalDeclaration(l);
            }

            new CCodeWalker(sub.ReturnLocal != null ? "v" + sub.ReturnLocal.Name : null, implementation).StartWalk(sub.Entrypoint);

            implementation.WriteLine('}');
        }
    }

    public void VisitLocalDeclaration(Local def) {
        if (def.IsArgument) {
            return;
        }
        implementation.Write(CBackend.Tab);
        def.TypeOf().Visit(new CTypeAliases(implementation));
        implementation.Write(" v");
        implementation.Write(def.Name);
        implementation.Write(" = ");
        def.TypeOf().Visit(new CTypeDefaults(implementation)); 
        implementation.WriteLine(";");
    }
}
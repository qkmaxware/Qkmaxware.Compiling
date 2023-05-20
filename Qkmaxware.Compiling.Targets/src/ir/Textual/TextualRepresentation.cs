using System.Text;

namespace Qkmaxware.Compiling.Ir.Textual;

/// <summary>
/// Class for handling the textual representation of an IR module
/// </summary>
public class TextualRepresentation {

    public Module Parse(TextReader reader) {
        throw new NotImplementedException();
    }

    public void PrintTo(Module module, TextWriter writer) {
        var printer = new TextualRepresentationPrinter(writer);
        printer.PrintOut(module);
    }

    public string PrintToString(Module module) {
        using var sb = new StringWriter();
        PrintTo(module, sb);
        return sb.ToString();
    }

}

/// <summary>
/// Class for printing a module into IR textual representation
/// </summary>
internal class TextualRepresentationPrinter : BasicBlockWalker, ModuleVisitor, ITupleVisitor {
    private static string Tab = "    ";

    private TextWriter writer;

    public TextualRepresentationPrinter(TextWriter writer) {
        this.writer = writer;
    }

    public void PrintOut(Module module) {
        this.VisitModule(module);
    }
    
    private void writeid(string id) {
        writer.Write('\'');
        foreach (var c in id) {
            if( c > 127 ) {
                // This character is too big for ASCII
                writer.Write("\\u");
                writer.Write(((int) c).ToString("x4"));
            }
            else {
                writer.Write(c);
            }
        }
        writer.Write('\'');
    }

    public void VisitGlobalDeclaration(Global def) {
        writer.Write("global "); writeid(def.Name); writer.Write(':'); writer.Write(def.TypeOf().GetType().Name); writer.WriteLine(';');
    }

    public void VisitModule(Module module) {
        writer.Write("module "); writer.WriteLine();
        writer.WriteLine();

        foreach (var global in module.Globals) {
            if (global is Global g)
                VisitGlobalDeclaration(g);
        }
        writer.WriteLine();
        foreach (var subroutine in module.Subprograms) {
            VisitSubprogram(subroutine);
        }
    }

    public void VisitSubprogram(Subprogram sub) {
        writer.Write("subprogram "); writeid(sub.Name); writer.Write('(');
        bool first = true;
        foreach (var arg in sub.Arguments) {
            if (!first)
                writer.Write(", ");
            writeid(arg.Name); writer.Write(':'); writer.Write(arg.TypeOf().GetType().Name);
            first = false;
        }
        writer.Write(") : "); writer.Write(sub.ReturnLocal?.TypeOf()?.GetType()?.Name ?? "void"); writer.WriteLine(" {");
        foreach (var def in sub.Locals) {
            if (def is Local local) {
                writer.Write(Tab);
                VisitLocalDeclaration(local);
            }
        }
        writer.WriteLine();
        this.StartWalk(sub.Entrypoint);
        writer.WriteLine('}');
    }

    public void VisitLocalDeclaration(Local def) {
        if (def.IsArgument)
            return;

        writer.Write("local "); writeid(def.Name); writer.Write(':'); writer.Write(def.TypeOf().GetType().Name); writer.WriteLine(';');
    }

    public override void Visit(BasicBlock block) {
        writer.Write(Tab);
        writer.Write('#'); writer.Write("block_"); writer.Write(block.GetHashCode()); writer.WriteLine(" {");

        foreach (var inst in block) {
            if (inst != null) {
                writer.Write(Tab); writer.Write(Tab);
                inst.Visit(this);
                writer.WriteLine();
            }
        }
        // } -> exit  or  } -> goto #block_1234  or  } -> goto if 'var' zero #block_1234 else #block_5678
        // } ->  or  } ondone 
        writer.Write(Tab); writer.Write("} -> "); block.Transition.Visit(this);
        writer.WriteLine();
    }

    public void Accept(Copy tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(CopyFromDeref tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(CopyToDeref tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(CopyFromOffset tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(CopyToOffset tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Add tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Sub tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Mul tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Div tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Mod tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Rem tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Pow tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(LeftShift tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(RightShiftLogical tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(RightShiftArithmetic tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(And tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Or tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Xor tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Not tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Negate tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Complement tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(AbsoluteValue tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(ATan tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Cos tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Sin tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Sqrt tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Ln tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Inc tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Dec tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(IncDeref tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(DecDeref tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(LessThan tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(LessThanEqualTo tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(GreaterThan tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(GreaterThanEqualTo tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Equality tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Inequality tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(Exit tuple)
    {
        writer.Write(tuple.RenderString());
    }

    public void Accept(Jump tuple)
    {
        writer.Write(tuple.RenderString());
    }

    public void Accept(JumpIfZero tuple)
    {
        writer.Write(tuple.RenderString());
    }

    public void Accept(JumpIfNotZero tuple)
    {
        writer.Write(tuple.RenderString());
    }

    public void Accept(CallProcedure tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(CallFunction tuple)
    {
        writer.Write(tuple.PrintString());
    }

    public void Accept(ReturnProcedure tuple)
    {
        writer.Write(tuple.RenderString());
    }

    public void Accept(ReturnFunction tuple)
    {
        writer.Write(tuple.RenderString());
    }
}

internal class TextualRepresentationParser {

}
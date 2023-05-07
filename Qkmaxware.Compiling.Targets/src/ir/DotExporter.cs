namespace Qkmaxware.Compiling.Targets.Ir;

/// <summary>
/// Export modules to GraphViz .Dot format text files
/// </summary>
public class DotExporter {

    /// <summary>
    /// Export the given module as a .dot graph to the text-writer
    /// </summary>
    /// <param name="module">module to export</param>
    /// <param name="writer">text-writer to print exported text to</param>
    public void ExportTo(Module module, TextWriter writer) {
        writer.WriteLine("digraph module {");

        BlockDotExporter blockwalker = new BlockDotExporter(writer);
        foreach (var subprogram in module) {
            writer.WriteLine($"    subgraph \"cluster_proc_{subprogram.ProcedureIndex}\" {{");
            writer.WriteLine($"        \"style\" = \"filled\"");
            writer.WriteLine($"        \"color\" = \"lightgrey\"");
            writer.WriteLine($"        proc_{subprogram.ProcedureIndex}_start [label=start,shape=Mdiamond]");
            writer.WriteLine($"        proc_{subprogram.ProcedureIndex}_end   [label=return,shape=Msquare]");
            if (!String.IsNullOrEmpty(subprogram.Name)) {
                writer.WriteLine($"        \"label\" = \"subprogram {subprogram.Name}\"");
            }
            blockwalker.ProcedureIndex = subprogram.ProcedureIndex;
            blockwalker.StartWalk(subprogram.Entrypoint);
            writer.WriteLine("    }");
        }

        writer.WriteLine("}");
    }

}

class BlockDotExporter : BasicBlockWalker {

    private TextWriter writer;
    public uint ProcedureIndex;

    public BlockDotExporter(TextWriter writer) {
        this.writer = writer;
    }

    public override void Visit(BasicBlock block) {
        var blockName = $"block_{block.GetHashCode()}";
        writer.WriteLine($"        subgraph \"cluster_{blockName}\" {{");
        writer.WriteLine($"            \"style\" = \"filled\"");
        writer.WriteLine($"            \"color\" = \"grey\"");
        writer.WriteLine($"            \"label\" = \"{blockName}\"");

        // Do each instruction
        Tuple? previous = null;
        uint index = 0;
        foreach (var instr in block.Instructions) {
            writer.WriteLine($"            \"{blockName}_inst_{index}\" [\"label\"=\"{instr.RenderString()}\"]"); 
            // If this is a flow change, goto the flow change
            if (previous != null) {
                if (previous is CallProcedure proc) {
                    var proc_block = $"proc_{proc.Called.ProcedureIndex}_start";
                    writer.WriteLine($"            \"{blockName}_inst_{index - 1}\" -> \"{proc_block}\" [style=dashed]"); // Goto procedure
                    proc_block = $"proc_{proc.Called.ProcedureIndex}_end";
                    writer.WriteLine($"            \"{proc_block}\" -> \"{blockName}_inst_{index - 1}\" [style=dashed]"); // Return from procedure
                } else if (previous is CallFunction func) {
                    var proc_block = $"proc_{func.Called.ProcedureIndex}_start";
                    writer.WriteLine($"            \"{blockName}_inst_{index - 1}\" -> \"{proc_block}\" [style=dashed]"); // Goto procedure
                    proc_block = $"proc_{func.Called.ProcedureIndex}_end";
                    writer.WriteLine($"            \"{proc_block}\" -> \"{blockName}_inst_{index - 1}\" [style=dashed]"); // Return from procedure
                }

                writer.WriteLine($"            \"{blockName}_inst_{index - 1}\" -> \"{blockName}_inst_{index}\"");
            } else {
                writer.WriteLine($"            \"proc_{ProcedureIndex}_start\" -> \"{blockName}_inst_{index}\"");
            }
            previous = instr;
            index++;
        }

        // Do transitions
        if (block.Instructions.Count() > 0) {
            if (block.Transition != null) {
                foreach (var next in block.Transition.NextBlocks()) {
                    var nextName = $"block_{next.GetHashCode()}";
                    if (next.Instructions.Count() > 0) {
                        writer.WriteLine($"            \"{blockName}_inst_{index - 1}\" -> \"{nextName}_inst_0\"");
                    } else if (next.Transition is ReturnProcedure || next.Transition is ReturnFunction) {
                        writer.WriteLine($"            \"{blockName}_inst_{index - 1}\" -> \"proc_{ProcedureIndex}_end\"");
                    }
                }
                if (block.Transition is ReturnProcedure || block.Transition is ReturnFunction) {
                    writer.WriteLine($"            \"{blockName}_inst_{index - 1}\" -> \"proc_{ProcedureIndex}_end\"");
                }
            }
        }

        writer.WriteLine("        }");
    }
}
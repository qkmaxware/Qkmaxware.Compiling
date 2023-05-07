namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

/// <summary>
/// A writer to output an assembly file from an in-memory assembly
/// </summary>
public class AssemblyWriter {
    
    private TextWriter writer;
    public string TabCharacter = "    ";
    
    public AssemblyWriter(TextWriter writer) {
        this.writer = writer;
    }

    /// <summary>
    /// Emit a sequence of assembly instructions to the text writer
    /// </summary>
    /// <param name="instructions">instructions to emit</param>
    public void Emit(IEnumerable<IAssemblyInstruction> instructions) {
        writer.WriteLine(".text");
        foreach (var code in instructions) {
            writer.Write(TabCharacter);
            writer.WriteLine(code.ToAssemblyString());
        }
    }

    /// <summary>
    /// Emit a parsed assembly program AST to the text writer
    /// </summary>
    /// <param name="program">MIPS assembly program AST</param>
    public void Emit(AssemblyProgram program) {
        foreach (var section in program.GlobalSections) {
            writer.WriteLine(section);
        }
        foreach (var section in program.DataSections) {
            writer.WriteLine(section);
            foreach (var data in section.Data) {
                writer.Write(TabCharacter);
                writer.WriteLine(data);
            }
        }
        foreach (var section in program.TextSections) {
            writer.WriteLine(section);
            foreach (var code in section.Code) {
                writer.Write(TabCharacter);
                writer.WriteLine(code);
            }
        }
    }
}
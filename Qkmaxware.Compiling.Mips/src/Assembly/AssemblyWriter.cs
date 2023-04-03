namespace Qkmaxware.Compiling.Mips.Assembly;

/// <summary>
/// A writer to output an assembly file from an in-memory assembly
/// </summary>
public class AssemblyWriter {
    
    private TextWriter writer;
    public string TabCharacter = "    ";
    
    public AssemblyWriter(TextWriter writer) {
        this.writer = writer;
    }

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
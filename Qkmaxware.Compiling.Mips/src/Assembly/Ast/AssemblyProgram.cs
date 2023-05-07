namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public class AssemblyProgram {
    public List<Section> Sections {get; private set;} = new List<Section>();

    public IEnumerable<GlobalSection> GlobalSections => Sections.OfType<GlobalSection>();
    public IEnumerable<DataSection> DataSections => Sections.OfType<DataSection>();
    public IEnumerable<TextSection> TextSections => Sections.OfType<TextSection>();

    public AssemblyProgram() {

    }

    public AssemblyProgram(DataSection data, TextSection text) : this() {
        this.Sections.Add(data);
        this.Sections.Add(text);
    }
}
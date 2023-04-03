namespace Qkmaxware.Compiling.Mips.Assembly;

public class AssemblyProgram {
    public List<Section> Sections {get; private set;} = new List<Section>();

    public IEnumerable<GlobalSection> GlobalSections => Sections.OfType<GlobalSection>();
    public IEnumerable<DataSection> DataSections => Sections.OfType<DataSection>();
    public IEnumerable<TextSection> TextSections => Sections.OfType<TextSection>();
}
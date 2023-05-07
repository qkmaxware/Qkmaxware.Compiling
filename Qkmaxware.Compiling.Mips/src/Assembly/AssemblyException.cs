namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public class AssemblyException : System.Exception {

    public long SourcePosition {get; private set;}

    public AssemblyException(long position, string message) : base (message) {
        this.SourcePosition = position;
    }
}
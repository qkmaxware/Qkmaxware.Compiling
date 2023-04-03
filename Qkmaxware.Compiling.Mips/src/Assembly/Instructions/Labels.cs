namespace Qkmaxware.Compiling.Mips.Assembly;

public class LabelMarker : IAssemblyInstruction {
    public string Name {get; private set;}

    public LabelMarker(string name) {
        this.Name = name;
    }

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string ToString() {
        return $"{Name}:";
    }
}
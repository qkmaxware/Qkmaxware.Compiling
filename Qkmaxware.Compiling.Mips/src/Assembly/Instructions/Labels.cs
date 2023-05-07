namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public class LabelMarker : IAssemblyInstruction {
    public string Name {get; private set;}

    public LabelMarker() { Name = string.Empty; }
    public LabelMarker(string name) {
        this.Name = name;
    }

    public string InstrName() => "label";
    public string InstrFormat() => InstrName() + ":";
    public string InstrDescription() =>  "Create a label, labels are resolved to addresses in memory and can be used in jump or branch instructions.";

    public void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string ToString() {
        return $"{Name}:";
    }

    public override bool Equals(object? obj) {
        if (obj is LabelMarker marker) {
            return Name.Equals(marker.Name);
        }  
        return base.Equals(obj);
    }

    public override int GetHashCode() {
        return Name.GetHashCode();
    }
}
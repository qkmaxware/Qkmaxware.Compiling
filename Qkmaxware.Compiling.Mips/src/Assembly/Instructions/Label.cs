namespace Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

public class Label : IPseudoInstruction {

    public string Name {get; private set;}

    public Label(string name) {
        this.Name = name;
    }

    public string InstructionName() => "label";

    public string AssemblyFormat() => "label:";

    public string InstructionDescription() => "Create a label which can be used in jump or branch instructions";

    public string ToAssemblyString() => $"{Name}:";
}
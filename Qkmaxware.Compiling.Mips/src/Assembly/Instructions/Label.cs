namespace Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

public class Label : IPseudoInstruction {

    public string Name {get; private set;}

    public Label(string name) {
        this.Name = name;
    }
}
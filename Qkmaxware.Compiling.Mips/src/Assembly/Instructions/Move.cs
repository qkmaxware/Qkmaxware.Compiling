namespace Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

/// <summary>
/// MIPS move pseudo-instruction
/// </summary>
public class Move : IPseudoInstruction {

    public RegisterIndex Destination {get; set;}
    public RegisterIndex Source {get; set;}

    public Move() {}

    public Move(RegisterIndex dest, RegisterIndex source) {
        this.Destination = dest;
        this.Source = source;
    }

    public string InstructionName() => "move";

    public string AssemblyFormat() => "move $dest, $src";

    public string InstructionDescription() => "Move a value from register $src to register $dest.";

    public string ToAssemblyString() => $"move {Destination}, {Source}";

    public IEnumerable<Bytecode.IBytecodeInstruction> Assemble(AssemblerEnvironment env) {
        yield return new Bytecode.Add {
            Destination = this.Destination,
            LhsOperand = this.Source,
            RhsOperand = RegisterIndex.Zero
        };
    }
}
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

/// <summary>
/// Mips assembly label
/// </summary>
public class Label : IPseudoInstruction {

    public string? Name {get; set;}

    public Label () {}

    public Label(string name) {
        this.Name = name;
    }

    public string InstructionName() => "label";

    public string AssemblyFormat() => "label:";

    public string InstructionDescription() => "Create a label which can be used in jump or branch instructions";

    public string ToAssemblyString() => $"{Name}:";

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) {
        if (this.Name != null) {
            env.SetLabelAddress(this.Name, env.CurrentInstructionAddress());
        }
        // Makes no new instructions, simply sets the label address
        // Alternatively you could do a NOP
        yield break; 
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out IAssemblyInstruction? decoded) {
        decoded = null;
        return false;
    }
}
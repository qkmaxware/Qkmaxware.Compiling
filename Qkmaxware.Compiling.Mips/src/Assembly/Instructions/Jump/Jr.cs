using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

/// <summary>
/// Jump return
/// </summary>
public class Jr : IAssemblyInstruction {
    public string InstructionName() => "jr";

    public string AssemblyFormat() => $"{InstructionName()} $dest";

    public string InstructionDescription() => "Immediately jump to a given location in the program.";

    public string ToAssemblyString() => $"{InstructionName()}";

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) {
        var j = new Bytecode.Instructions.Jr {};
        yield return j;
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out IAssemblyInstruction? decoded) {
        if (opcode.Value != "jr") {
            decoded = null;
            return false;
        }

        // jalr $dest
        if (args.Count != 0) {
            throw new AssemblyException(opcode.Position, "Invalid number of argument(s)");
        }
        decoded = new Assembly.Instructions.Jr {};
        return true;
    }
}
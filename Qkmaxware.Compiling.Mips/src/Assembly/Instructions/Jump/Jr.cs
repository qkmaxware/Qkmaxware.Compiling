using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

/// <summary>
/// Jump register
/// </summary>
public class Jr : IAssemblyInstruction {
    public string InstructionName() => "jr";

    public string AssemblyFormat() => $"{InstructionName()} $dest";

    public string InstructionDescription() => "Immediately jump to a given location in the program.";

    public string ToAssemblyString() => $"{InstructionName()}";

    public RegisterIndex AddressRegister {get; set;}

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) {
        var j = new Bytecode.Instructions.Jr {
            Source = AddressRegister
        };
        yield return j;
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out IAssemblyInstruction? decoded) {
        if (opcode.Value != "jr") {
            decoded = null;
            return false;
        }

        // jalr $dest
        if (args.Count != 1) {
            throw new AssemblyException(opcode.Position, "Missing required argument(s)");
        }
        if (args[0] is RegisterToken r) {
            decoded = new Assembly.Instructions.Jr {
                AddressRegister = r.Value
            };
            return true;
        } else {
            throw new AssemblyException(args[0].Position, "Missing register containing address");
        }  
    }
}
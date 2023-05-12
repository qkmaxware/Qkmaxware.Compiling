using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

/// <summary>
/// Jump and link
/// </summary>
public class Jal : IAssemblyInstruction {
    public string InstructionName() => "jal";

    public string AssemblyFormat() => $"{InstructionName()} $dest";

    public string InstructionDescription() => "Immediately jump to a given location in the program.";

    public string ToAssemblyString() => $"{InstructionName()} {Address}";

    public AddressLikeValue? Address {get; set;}

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) {
        if (Address == null)
            yield break;

        var j = new Bytecode.Instructions.Jal {
            AddressOffset = 0
        };
        if (Address is LabelAddress Label) {
            env.ResolveLabelAddressOnceComputed(Label.Value, (addr) => j.AddressOffset = (int)addr);
        } else if (Address is IntegerAddress scalar) {
            j.AddressOffset = (int)scalar.Value;
        }
        yield return j;
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out IAssemblyInstruction? decoded) {
        if (opcode.Value != "jal") {
            decoded = null;
            return false;
        }

        // jal $dest
        if (args.Count != 1) {
            throw new AssemblyException(opcode.Position, "Missing required argument(s)");
        }
        if (args[0] is AddressLikeToken l) {
            decoded = new Assembly.Instructions.Jal {
                Address = l.GetAddress()
            };
            return true;
        } else {
            throw new AssemblyException(args[0].Position, "Missing destination address");
        }
    }
}
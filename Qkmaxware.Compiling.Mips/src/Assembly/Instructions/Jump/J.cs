using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

/// <summary>
/// Jump
/// </summary>
public class J : IAssemblyInstruction {
    public string InstructionName() => "j";

    public string AssemblyFormat() => "j $dest";

    public string InstructionDescription() => "Immediately jump to a given location in the program.";

    public string ToAssemblyString() => $"j {Address}";

    public AddressLikeValue? Address {get; set;}

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) {
        if (Address == null)
            yield break;

        var j = new Bytecode.Instructions.J {
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
        if (opcode.Value != "j") {
            decoded = null;
            return false;
        }

        // j $dest
        if (args.Count != 1) {
            throw new AssemblyException(opcode.Position, "Missing required argument(s)");
        }
        if (args[0] is AddressLikeToken l) {
            decoded = new Assembly.Instructions.J {
                Address = l.GetAddress()
            };
            return true;
        } else {
            throw new AssemblyException(args[0].Position, "Missing destination address");
        }   
    }
}
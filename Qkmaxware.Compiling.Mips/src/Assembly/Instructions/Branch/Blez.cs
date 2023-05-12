using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

/// <summary>
/// Branch less equal to zero
/// </summary>
public class Blez : IAssemblyInstruction {
    public string InstructionName() => "blez";

    public string AssemblyFormat() => $"{InstructionName()} $source, offset";

    public string InstructionDescription() => "If $source <= 0 increment the PC by the given offset.";

    public string ToAssemblyString() => $"{InstructionName()} {Source}, {Address}";

    public RegisterIndex Source {get; set;}
    public AddressLikeValue? Address {get; set;}

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) {
        if (Address == null)
            yield break;

        var j = new Bytecode.Instructions.Blez {
            Source = this.Source,
            AddressOffset = 0
        };
        var marker = env.CurrentInstructionAddress();
        if (Address is LabelAddress Label) {
            env.ResolveLabelAddressOnceComputed(Label.Value, (addr) => {
                j.AddressOffset = (int)((long)addr - (long)marker);
            });
        } else if (Address is IntegerAddress scalar) {
            j.AddressOffset = (int)scalar.Value;
        }
        yield return j;
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out IAssemblyInstruction? decoded) {
        decoded = null;
        if (opcode.Value != "blez") {
            return false;
        }

        // OPCODE $src, $rhs
        if (args.Count != 3) {
            throw new AssemblyException(opcode.Position, "Invalid number of argument(s)");
        }
        if (args[0] is not RegisterToken lhsT) {
            throw new AssemblyException(args[0].Position, "Missing left-hand side operand");
        }
        if (args[1] is not CommaToken) {
            throw new AssemblyException(args[1].Position, "Missing comma");
        }
        if (args[2] is not AddressLikeToken offsetT) {
            throw new AssemblyException(args[2].Position, "Missing offset");
        }
        decoded = new Assembly.Instructions.Blez {
            Source = lhsT.Value,
            Address = offsetT.GetAddress()
        };
        return true; 
    }
}
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

/// <summary>
/// Branch on coprocessor condition false
/// </summary>
public class Bc1t : IAssemblyInstruction {
    public string InstructionName() => "bc1f";

    public string AssemblyFormat() => $"{InstructionName()} cc, offset";

    public string InstructionDescription() => "If coprocessor 1 flag cc is set, increment the PC by the given offset.";

    public string ToAssemblyString() => $"{InstructionName()} {ConditionFlagIndex}, {Address}";

    public uint ConditionFlagIndex {get; set;}
    public AddressLikeValue? Address {get; set;}

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) {
        if (Address == null)
            yield break;

        var j = new Bytecode.Instructions.Bc1t {
            ConditionFlagIndex = this.ConditionFlagIndex,
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
        if (opcode.Value != "bc1f") {
            return false;
        }

        // OPCODE $src, $rhs
        if (args.Count != 3) {
            throw new AssemblyException(opcode.Position, "Invalid number of argument(s)");
        }
        if (args[0] is not ScalarConstantToken lhsT) {
            throw new AssemblyException(args[0].Position, "Missing condition flag");
        }
        if (args[1] is not CommaToken) {
            throw new AssemblyException(args[1].Position, "Missing comma");
        }
        if (args[2] is not AddressLikeToken offsetT) {
            throw new AssemblyException(args[2].Position, "Missing offset");
        }
        decoded = new Assembly.Instructions.Bc1t {
            ConditionFlagIndex = (uint)lhsT.IntegerValue,
            Address = offsetT.GetAddress()
        };
        return true; 
    }
}
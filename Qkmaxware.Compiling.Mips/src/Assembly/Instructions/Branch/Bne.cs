using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

/// <summary>
/// Branch on not equals (MIPS bne)
/// </summary>
public class Bne : IAssemblyInstruction {
    public string InstructionName() => "bne";

    public string AssemblyFormat() => $"{InstructionName()} $lhs, $rhs, offset";

    public string InstructionDescription() => "If $lhs != $rhs increment the PC by the given offset.";

    public string ToAssemblyString() => $"{InstructionName()} {LhsOperand}, {RhsOperand}, {Address}";

    public RegisterIndex LhsOperand {get; set;}
    public RegisterIndex RhsOperand {get; set;}
    public AddressLikeValue? Address {get; set;}

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) {
        if (Address == null)
            yield break;

        var j = new Bytecode.Instructions.Bne {
            LhsOperand = this.LhsOperand,
            RhsOperand = this.RhsOperand,
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
        if (opcode.Value != "bne") {
            return false;
        }

        // OPCODE $dest, $lhs, $rhs
        if (args.Count != 5) {
            throw new AssemblyException(opcode.Position, "Invalid number of argument(s)");
        }
        if (args[0] is not RegisterToken lhsT) {
            throw new AssemblyException(args[0].Position, "Missing left-hand side operand");
        }
        if (args[1] is not CommaToken) {
            throw new AssemblyException(args[1].Position, "Missing comma between arguments");
        }
        if (args[2] is not RegisterToken rhsT) {
            throw new AssemblyException(args[2].Position, "Missing right-hand side operand");
        }
        if (args[3] is not CommaToken) {
            throw new AssemblyException(args[3].Position, "Missing comma between arguments");
        }
        if (args[4] is not AddressLikeToken offsetT) {
            throw new AssemblyException(args[4].Position, "Missing offset");
        }
        decoded = new Assembly.Instructions.Bne {
            LhsOperand = lhsT.Value,
            RhsOperand = rhsT.Value,
            Address = offsetT.GetAddress()
        };
        return true; 
    }
}
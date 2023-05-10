using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

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

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) {
        yield return new Add {
            Destination = this.Destination,
            LhsOperand = this.Source,
            RhsOperand = RegisterIndex.Zero
        };
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out IAssemblyInstruction? decoded) {
        if (opcode.Value != "move") {
            decoded = null;
            return false;
        }

        // la $dest, value
        if (args.Count != 3) {
            throw new AssemblyException(opcode.Position, "Missing required argument(s)");
        }
        if (args[0] is not RegisterToken destT) {
            throw new AssemblyException(args[0].Position, "Missing destination register");
        }
        if (args[1] is not CommaToken) {
            throw new AssemblyException(args[1].Position, "Missing comma between arguments");
        }
        if (args[2] is not RegisterToken argT) {
            throw new AssemblyException(args[2].Position, "Missing source register");
        }
        var dest = destT;
        var arg = argT;

        decoded = new Move {
            Destination = dest.Value,
            Source = arg.Value
        };
        return true;
    }
}
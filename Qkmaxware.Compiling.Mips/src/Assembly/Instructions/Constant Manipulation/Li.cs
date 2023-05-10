using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

/// <summary>
/// MIPS load immediate pseudo-instruction
/// </summary>
public class Li : IPseudoInstruction {

    public RegisterIndex Destination {get; set;}
    public ScalarConstantToken? Value {get; set;}

    public Li() {}

    public Li(RegisterIndex dest, ScalarConstantToken value) {
        this.Destination = dest;
        this.Value = value;
    }

    public string InstructionName() => "li";

    public string AssemblyFormat() => "li $dest, value";

    public string InstructionDescription() => "Load an immediate value into register $dest.";

    public string ToAssemblyString() => $"la {Destination}, {Value}";

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) {
        var bits = BitConverter.ToUInt32(BitConverter.GetBytes(this.Value?.IntegerValue ?? 0));
        yield return new Lui {
            Destination = this.Destination,
            Immediate = bits.HighHalf()
        };
        yield return new Ori {
            Destination = this.Destination,
            LhsOperand = this.Destination,
            RhsOperand = bits.LowHalf()
        };
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out IAssemblyInstruction? decoded) {
        if (opcode.Value != "li") {
            decoded = null;
            return false;
        }

        // li $dest, value
        if (args.Count != 3) {
            throw new AssemblyException(opcode.Position, "Missing required argument(s)");
        }
        if (args[0] is not RegisterToken destT) {
            throw new AssemblyException(args[0].Position, "Missing destination register");
        }
        if (args[1] is not CommaToken) {
            throw new AssemblyException(args[1].Position, "Missing comma between arguments");
        }
        if (args[2] is not ScalarConstantToken argT) {
            throw new AssemblyException(args[2].Position, "Missing constant");
        }
        var dest = destT;
        var arg = argT;

        decoded = new Li {
            Destination = dest.Value,
            Value = arg,
        };
        return true;
    }
}
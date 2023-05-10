namespace Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

/// <summary>
/// MIPS load address pseudo-instruction
/// </summary>
public class La : IPseudoInstruction {

    public RegisterIndex Destination {get; set;}
    public string? Label {get; set;}

    public La() {}

    public La(RegisterIndex dest, string name) {
        this.Destination = dest;
        this.Label = name;
    }

    public string InstructionName() => "la";

    public string AssemblyFormat() => "la $dest, addressLabel";

    public string InstructionDescription() => "Load a word from memory located at the given label's address into register $dest.";

    public string ToAssemblyString() => $"la {Destination}, {Label}";

    public IEnumerable<Bytecode.IBytecodeInstruction> Assemble(AssemblerEnvironment env) {
        var upper = new Bytecode.Lui {
            Destination = this.Destination,
            Immediate = 0
        };
        var lower = new Bytecode.Ori {
            Destination = this.Destination,
            LhsOperand  = this.Destination,
            RhsOperand  = 0
        };
        if (this.Label != null) {
            env.ResolveLabelAddressOnceComputed(this.Label, (address) => {
                // Get the label address as soon as its computed
                upper.Immediate = address.HighHalf();
                lower.RhsOperand = address.LowHalf();
            });
        }
        yield return upper;
        yield return lower;
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out Mips.Assembly.IAssemblyInstruction? decoded) {
        if (opcode.Value != "la") {
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
        if (args[2] is not IdentifierToken argT) {
            throw new AssemblyException(args[2].Position, "Missing constant");
        }
        var dest = destT;
        var arg = argT;

        decoded = new La {
            Destination = dest.Value,
            Label = arg.Value
        };
        return true;
    }
}
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

    public IEnumerable<Bytecode.IBytecodeInstruction> Assemble(AssemblerEnvironment env) {
        var bits = BitConverter.ToUInt32(BitConverter.GetBytes(this.Value?.IntegerValue ?? 0));
        yield return new Bytecode.Lui {
            Destination = this.Destination,
            Immediate = bits.HighHalf()
        };
        yield return new Bytecode.Ori {
            Destination = this.Destination,
            LhsOperand = this.Destination,
            RhsOperand = bits.LowHalf()
        };
    }
}
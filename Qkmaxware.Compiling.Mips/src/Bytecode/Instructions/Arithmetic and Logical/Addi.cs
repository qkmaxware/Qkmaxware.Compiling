using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

/// <summary>
/// Signed addition of a register and an immediate value (MIPS addi)
/// </summary>
public class Addi : ArithLogIInstruction, IAssemblyInstruction {
    public static readonly uint BinaryCode = 0b001000U;
    public override uint Opcode => BinaryCode;

    /// <summary>
    /// The written format of this instruction in assembly
    /// </summary>
    /// <returns>description</returns>
    public string AssemblyFormat() => $"{this.InstructionName()} $dest, $lhs, value";

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public string InstructionDescription() => "Compute $lhs + value and store the result in $dest.";

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) { yield return this; }

    public RegisterIndex Destination {
        get => this.Target;
        set => this.Target = value;
    }

    public RegisterIndex LhsOperand {
        get => this.Source;
        set => this.Source = value;
    }
    public int RhsOperand {
        get => BitConverter.ToInt32(BitConverter.GetBytes(this.Immediate.SignExtend(16)));
        set => this.Immediate = BitConverter.ToUInt32(BitConverter.GetBytes(value));
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var lhs = cpu.Registers[this.LhsOperand].ReadAsInt32();
        var rhs = this.RhsOperand;

        cpu.Registers[this.Target].WriteInt32(lhs + rhs);
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (ImmediateEncodedInstruction.TryDecodeBytecode(bytecode, BinaryCode, out var source, out var target, out var immediate)) {
            decoded = new Addi {
                Target = (RegisterIndex)target,
                LhsOperand = (RegisterIndex)source,
                Immediate = immediate
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out IAssemblyInstruction? decoded) {
        Assembly.RegisterToken dest; Assembly.RegisterToken lhs; Assembly.ScalarConstantToken rhs;
        if (!IsAssemblyFormatDestLhsRhs<Addi, Assembly.RegisterToken, Assembly.RegisterToken, Assembly.ScalarConstantToken>(opcode, args, out dest, out lhs, out rhs)) {
            decoded = null;
            return false;
        }

        decoded = new Addi {
            Target = dest.Value,
            LhsOperand = lhs.Value,
            RhsOperand = rhs.IntegerValue,
        };
        return true;
    }
}
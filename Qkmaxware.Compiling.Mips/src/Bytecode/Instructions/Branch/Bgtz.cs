using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Branch on greater than 0 (MIPS bgtz)
/// </summary>
public class Bgtz : BranchZInstruction, Assembly.IAssemblyInstruction {
    public static readonly uint BinaryCode = 0b000111U;
    public override uint Opcode => BinaryCode;

    public int AddressOffset {
        get => BitConverter.ToInt32(BitConverter.GetBytes(this.Immediate));
        set => this.Immediate = BitConverter.ToUInt32(BitConverter.GetBytes(value));
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var operand = cpu.Registers[this.Source].ReadAsInt32();

        if (operand > 0) {
            cpu.PC += this.AddressOffset;
        }
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (ImmediateEncodedInstruction.TryDecodeBytecode(bytecode, BinaryCode, out var source, out var target, out var immediate)) {
            decoded = new Bgtz {
                Source = (RegisterIndex)source,
                Immediate = immediate
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }

    /// <summary>
    /// The written format of this instruction in assembly
    /// </summary>
    /// <returns>description</returns>
    public override string AssemblyFormat() => $"{this.InstructionName()} $source, offset";

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public override string InstructionDescription() => "If $source > 0 increment the PC by the given offset";
    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) { yield return this; }
    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out Mips.Assembly.IAssemblyInstruction? decoded) {
        Assembly.RegisterToken lhs; Assembly.ScalarConstantToken offset;
        if (!IsAssemblyFormatLhsOffset<Bgtz, Assembly.RegisterToken, Assembly.ScalarConstantToken>(opcode, args, out lhs, out offset)) {
            decoded = null;
            return false;
        }

        decoded = new Bgtz {
            Source = lhs.Value,
            AddressOffset = offset.IntegerValue
        };
        return true;
    }
}
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Branch on equals (MIPS beq)
/// </summary>
public class Beq : BranchInstruction, Assembly.IAssemblyInstruction {
    public static readonly uint BinaryCode = 0b100000U;
    public override uint Opcode => BinaryCode;

    public RegisterIndex LhsOperand {
        get => this.Source;
        set => this.Source = value;
    }
    public RegisterIndex RhsOperand {
        get => this.Target;
        set => this.Target = value;
    }
    public int AddressOffset {
        get => BitConverter.ToInt32(BitConverter.GetBytes(this.Immediate));
        set => this.Immediate = BitConverter.ToUInt32(BitConverter.GetBytes(value));
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var lhs = cpu.Registers[this.LhsOperand].ReadAsInt32();
        var rhs = cpu.Registers[this.RhsOperand].ReadAsInt32();

        if (lhs == rhs) {
            cpu.PC += this.AddressOffset;
        }
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (ImmediateEncodedInstruction.TryDecodeBytecode(bytecode, BinaryCode, out var source, out var target, out var immediate)) {
            decoded = new Beq {
                Source = (RegisterIndex)source,
                Target = (RegisterIndex)target,
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
    public override string AssemblyFormat() => $"{this.InstructionName()} $lhs, $rhs, offset";

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public override string InstructionDescription() => "If $lhs == $rhs increment the PC by the given offset";
    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) { yield return this; }
    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out Mips.Assembly.IAssemblyInstruction? decoded) {
        Assembly.RegisterToken lhs; Assembly.RegisterToken rhs; Assembly.ScalarConstantToken offset;
        if (!IsAssemblyFormatLhsRhsOffset<Beq, Assembly.RegisterToken, Assembly.RegisterToken, Assembly.ScalarConstantToken>(opcode, args, out lhs, out rhs, out offset)) {
            decoded = null;
            return false;
        }

        decoded = new Beq {
            LhsOperand = lhs.Value,
            RhsOperand = rhs.Value,
            AddressOffset = offset.IntegerValue
        };
        return true;
    }
}
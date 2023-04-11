using Qkmaxware.Compiling.Mips.Hardware;
// ADD
namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Set on less than for unsigned numbers and an immediate value (MIPS sltiu)
/// </summary>
public class Sltiu : ArithLogIInstruction {
    public static readonly uint BinaryCode = 0b001001U;
    public override uint Opcode => BinaryCode;

    public RegisterIndex LhsOperand {
        get => this.Source;
        set => this.Source = value;
    }
    public uint RhsOperand {
        get => this.Immediate;
        set => this.Immediate = value;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var lhs = cpu.Registers[this.LhsOperand].ReadAsInt32();
        var rhs = this.RhsOperand;

        cpu.Registers[this.Target].WriteInt32(lhs < rhs ? 1 : 0);
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (ImmediateEncodedInstruction.TryDecodeBytecode(bytecode, BinaryCode, out var source, out var target, out var immediate)) {
            decoded = new Sltiu {
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
}
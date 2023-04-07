using Qkmaxware.Compiling.Mips.Hardware;
// ADD
namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Set on less than for unsigned numbers and an immediate value (MIPS sltiu)
/// </summary>
public class Sltiu : ArithLogIInstruction {
    public static readonly uint BinaryCode = 001001U;
    public override uint Opcode => BinaryCode;

    public RegisterIndex LhsOperand {
        get => this.Source;
        set => this.Source = value;
    }
    public uint RhsOperand {
        get => this.Immediate;
        set => this.Immediate = value;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = cpu.Registers[this.LhsOperand].ReadAsInt32();
        var rhs = this.RhsOperand;

        cpu.Registers[this.Target].WriteInt32(lhs < rhs ? 1 : 0);
    }
}
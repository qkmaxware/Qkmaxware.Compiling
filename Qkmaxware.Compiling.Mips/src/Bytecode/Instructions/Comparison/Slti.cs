using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Set on less than for signed numbers and an immediate value (MIPS slti)
/// </summary>
public class Slti : ArithLogIInstruction {
    public static readonly uint BinaryCode = 001010U;
    public override uint Opcode => BinaryCode;

    public RegisterIndex LhsOperand {
        get => this.Source;
        set => this.Source = value;
    }
    public int RhsOperand {
        get => BitConverter.ToInt32(BitConverter.GetBytes(this.Immediate));
        set => this.Immediate = BitConverter.ToUInt32(BitConverter.GetBytes(value));
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = cpu.Registers[this.LhsOperand].ReadAsInt32();
        var rhs = this.RhsOperand;

        cpu.Registers[this.Target].WriteInt32(lhs < rhs ? 1 : 0);
    }
}
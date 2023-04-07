using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Signed addition of a register and an immediate value (MIPS addi)
/// </summary>
public class Addi : ArithLogIInstruction {
    public static readonly uint BinaryCode = 001000U;
    public override uint Opcode => BinaryCode;

    public RegisterIndex LhsOperand {
        get => this.Source;
        set => this.Source = value;
    }
    public int RhsOperand {
        get => BitConverter.ToInt32(BitConverter.GetBytes(this.Immediate.SignExtend(16)));
        set => this.Immediate = BitConverter.ToUInt32(BitConverter.GetBytes(value));
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = cpu.Registers[this.LhsOperand].ReadAsInt32();
        var rhs = this.RhsOperand;

        cpu.Registers[this.Target].WriteInt32(lhs + rhs);
    }
}
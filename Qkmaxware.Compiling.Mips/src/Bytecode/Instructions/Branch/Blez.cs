using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Branch on less than or equal to 0 (MIPS blez)
/// </summary>
public class Blez : BranchZInstruction {
    public static readonly uint BinaryCode = 000110U;
    public override uint Opcode => BinaryCode;

    public int AddressOffset {
        get => BitConverter.ToInt32(BitConverter.GetBytes(this.Immediate));
        set => this.Immediate = BitConverter.ToUInt32(BitConverter.GetBytes(value));
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var operand = cpu.Registers[this.Source].ReadAsInt32();

        if (operand <= 0) {
            cpu.PC += this.AddressOffset;
        }
    }
}
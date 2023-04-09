using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Branch on greater than 0 (MIPS bgtz)
/// </summary>
public class Bgtz : BranchZInstruction {
    public static readonly uint BinaryCode = 000111U;
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
}
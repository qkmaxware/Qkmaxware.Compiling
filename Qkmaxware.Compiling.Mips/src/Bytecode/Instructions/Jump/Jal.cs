using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Jump and link (MIPS jal)
/// </summary>
public class Jal : JumpInstruction {
    public static readonly uint BinaryCode = 000011U;
    public override uint Opcode => BinaryCode;
    
    public int AddressOffset {
        get => BitConverter.ToInt32(BitConverter.GetBytes(this.Immediate));
        set => this.Immediate = BitConverter.ToUInt32(BitConverter.GetBytes(value));
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        cpu.Registers[new RegisterIndex(31)].WriteInt32(cpu.PC << 2); // save old pc (as bytes not words)
        cpu.PC += this.AddressOffset;                                 // goto new pc
    }
}
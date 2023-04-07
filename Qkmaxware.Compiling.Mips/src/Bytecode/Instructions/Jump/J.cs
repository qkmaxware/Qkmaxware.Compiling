using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Jump (MIPS j)
/// </summary>
public class J : JumpInstruction {
    public static readonly uint BinaryCode = 000010U;
    public override uint Opcode => BinaryCode;
    
    public int AddressOffset {
        get => BitConverter.ToInt32(BitConverter.GetBytes(this.Immediate));
        set => this.Immediate = BitConverter.ToUInt32(BitConverter.GetBytes(value));
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        cpu.PC += this.AddressOffset;
    }
}
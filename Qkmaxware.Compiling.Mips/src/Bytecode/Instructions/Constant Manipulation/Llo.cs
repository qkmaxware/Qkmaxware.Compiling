using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Load low bits (MIPS llo)
/// </summary>
public class Llo : LoadIInstruction {
    public static readonly uint BinaryCode = 011000U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var value = (uint)this.Immediate.LowHalf();
        var prev = cpu.Registers[this.Target].ReadAsUInt32().ClearLowHalf();
        
        cpu.Registers[this.Target].WriteUInt32(prev | value);
    }
}
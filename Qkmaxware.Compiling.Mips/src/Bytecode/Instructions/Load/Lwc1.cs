using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Load word into FPU from memory (MIPS lwc1)
/// </summary>
public class Lwc1 : LoadStoreInstruction {
    public static readonly uint BinaryCode = 0x31U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var raw = memory.LoadWord(cpu.Registers[this.Source].ReadAsUInt32() + this.Immediate);
        fpu.Registers[this.Target].WriteUInt32(raw);
    }
}
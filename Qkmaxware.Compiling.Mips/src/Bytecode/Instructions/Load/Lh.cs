using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Load signed half word (MIPS lh)
/// </summary>
public class Lh : LoadStoreInstruction {
    public static readonly uint BinaryCode = 100001U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var raw = memory.LoadHalf(cpu.Registers[this.Source].ReadAsUInt32() + this.Immediate);
        var extended = ((uint)raw).SignExtend(16);
        cpu.Registers[this.Target].WriteUInt32(extended);
    }
}
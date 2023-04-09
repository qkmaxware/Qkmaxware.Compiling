using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Load signed byte (MIPS lb)
/// </summary>
public class Lb : LoadStoreInstruction {
    public static readonly uint BinaryCode = 100000U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var raw = memory.LoadByte(cpu.Registers[this.Source].ReadAsUInt32() + this.Immediate);
        var extended = ((uint)raw).SignExtend(8);
        cpu.Registers[this.Target].WriteUInt32(extended);
    }
}
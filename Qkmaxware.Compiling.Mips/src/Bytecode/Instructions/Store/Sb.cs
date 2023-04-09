using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Store signed byte (MIPS sb)
/// </summary>
public class Sb : LoadStoreInstruction {
    public static readonly uint BinaryCode = 101000U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var address = cpu.Registers[this.Source].ReadAsUInt32() + this.Immediate;
        memory.StoreByte(address, cpu.Registers[this.Target].ReadAsUInt32().LowByte());
    }
}
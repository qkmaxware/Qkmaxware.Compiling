using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Load unsigned half word (MIPS lhu)
/// </summary>
public class Lhu : LoadStoreInstruction {
    public static readonly uint BinaryCode = 100101U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var raw = memory.LoadHalf(cpu.Registers[this.Source].ReadAsUInt32() + this.Immediate);
        var extended = ((uint)raw);
        cpu.Registers[this.Target].WriteUInt32(extended);
    }
}
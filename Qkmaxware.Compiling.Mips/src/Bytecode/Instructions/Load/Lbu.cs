using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Load unsigned byte (MIPS lbu)
/// </summary>
public class Lbu : LoadStoreInstruction {
    public static readonly uint BinaryCode = 100100U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var raw = memory.LoadByte(cpu.Registers[this.Source].ReadAsUInt32() + this.Immediate);
        var extended = ((uint)raw);
        cpu.Registers[this.Target].WriteUInt32(extended);
    }
}
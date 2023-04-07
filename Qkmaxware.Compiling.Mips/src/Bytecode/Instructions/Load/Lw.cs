using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Load signed word (MIPS lw)
/// </summary>
public class Lw : LoadStoreInstruction {
    public static readonly uint BinaryCode = 100011U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var raw = memory.LoadWord(cpu.Registers[this.Source].ReadAsUInt32() + this.Immediate);
        cpu.Registers[this.Target].WriteUInt32(raw);
    }
}
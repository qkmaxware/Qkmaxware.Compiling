using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Store signed word (MIPS sw)
/// </summary>
public class Sw : LoadStoreInstruction {
    public static readonly uint BinaryCode = 101011U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var address = cpu.Registers[this.Source].ReadAsUInt32() + this.Immediate;
        memory.StoreWord(address, cpu.Registers[this.Target].ReadAsUInt32());
    }
}
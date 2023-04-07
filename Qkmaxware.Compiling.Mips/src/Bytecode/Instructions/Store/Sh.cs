using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Store signed half word (MIPS sh)
/// </summary>
public class Sh : LoadStoreInstruction {
    public static readonly uint BinaryCode = 101001U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var address = cpu.Registers[this.Source].ReadAsUInt32() + this.Immediate;
        memory.StoreHalf(address, cpu.Registers[this.Target].ReadAsUInt32().LowHalf());
    }
}
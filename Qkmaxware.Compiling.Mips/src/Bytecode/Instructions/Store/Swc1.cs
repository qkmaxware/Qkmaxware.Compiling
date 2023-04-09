using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Store word from FPU into memory (MIPS swc1)
/// </summary>
public class Swc1 : LoadStoreInstruction {
    public static readonly uint BinaryCode = 0x3dU;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var address = cpu.Registers[this.Source].ReadAsUInt32() + this.Immediate;
        memory.StoreWord(address, fpu.Registers[this.Target].ReadAsUInt32());
    }
}
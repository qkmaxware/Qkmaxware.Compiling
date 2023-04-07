using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Jump and link register (MIPS jalr)
/// </summary>
public class Jalr : JumpRInstruction {
    public static readonly uint BinaryCode = 001001;
    public override uint Opcode => BinaryCode;
    

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        cpu.Registers[new RegisterIndex(31)].WriteInt32(cpu.PC << 2); // save old pc (as bytes not words)
        cpu.PC += cpu.Registers[this.Source].ReadAsInt32() >> 2;      // goto new pc (as word not bytes)
    }
}
using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Jump return (MIPS jr)
/// </summary>
public class Jr : JumpRInstruction {
    public static readonly uint BinaryCode = 001000U;
    public override uint Opcode => BinaryCode;


    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        cpu.PC += (int)(cpu.Registers[this.Source].ReadAsUInt32() >> 2); // goto saved pc (as word not bytes)
    }
}
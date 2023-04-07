using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Move to hi register (MIPS mthi)
/// </summary>
public class Mthi : MoveToInstruction {
    public static readonly uint BinaryCode = 010001U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var toMove = cpu.Registers[this.Source].ReadAsUInt32();
        cpu.Registers.HI.WriteUInt32(toMove);
    }
}
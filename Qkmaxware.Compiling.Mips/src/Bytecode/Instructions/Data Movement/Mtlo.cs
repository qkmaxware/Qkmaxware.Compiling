using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Move to lo register (MIPS mtlo)
/// </summary>
public class Mtlo : MoveToInstruction {
    public static readonly uint BinaryCode = 010011U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var toMove = cpu.Registers[this.Source].ReadAsUInt32();
        cpu.Registers.LO.WriteUInt32(toMove);
    }
}
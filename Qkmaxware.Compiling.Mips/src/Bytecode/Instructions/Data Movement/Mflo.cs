using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Move from lo register (MIPS mflo)
/// </summary>
public class Mflo : MoveFromInstruction {
    public static readonly uint BinaryCode = 010010U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var toMove = cpu.Registers.LO.ReadAsUInt32();
        cpu.Registers[this.Destination].WriteUInt32(toMove);
    }
}
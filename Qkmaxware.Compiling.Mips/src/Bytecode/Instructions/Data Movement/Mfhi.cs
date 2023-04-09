using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Move from hi register (MIPS mfhi)
/// </summary>
public class Mfhi : MoveFromInstruction {
    public static readonly uint BinaryCode = 010000U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var toMove = cpu.Registers.HI.ReadAsUInt32();
        cpu.Registers[this.Destination].WriteUInt32(toMove);
    }
}
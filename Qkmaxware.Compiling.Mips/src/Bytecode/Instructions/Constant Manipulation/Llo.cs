using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Load low bits (MIPS llo)
/// </summary>
public class Llo : LoadIInstruction {
    public static readonly uint BinaryCode = 0b011000U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var value = (uint)this.Immediate.LowHalf();
        var prev = cpu.Registers[this.Target].ReadAsUInt32().ClearLowHalf();
        
        cpu.Registers[this.Target].WriteUInt32(prev | value);
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (ImmediateEncodedInstruction.TryDecodeBytecode(bytecode, BinaryCode, out var source, out var target, out var immediate)) {
            decoded = new Llo {
                Target = (RegisterIndex)target,
                Immediate = immediate
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }
}
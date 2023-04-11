using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Load high bits (MIPS lhi)
/// </summary>
public class Lhi : LoadIInstruction {
    public static readonly uint BinaryCode = 0b011001U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var value = (this.Immediate << 16);
        var prev = cpu.Registers[this.Target].ReadAsUInt32().ClearHighHalf();
        
        cpu.Registers[this.Target].WriteUInt32(prev | value);
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (ImmediateEncodedInstruction.TryDecodeBytecode(bytecode, BinaryCode, out var source, out var target, out var immediate)) {
            decoded = new Lhi {
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
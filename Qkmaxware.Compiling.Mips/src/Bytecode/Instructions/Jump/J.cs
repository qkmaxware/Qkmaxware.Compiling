using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Jump (MIPS j)
/// </summary>
public class J : JumpInstruction {
    public static readonly uint BinaryCode = 0b000010U;
    public override uint Opcode => BinaryCode;
    
    public int AddressOffset {
        get => BitConverter.ToInt32(BitConverter.GetBytes(this.Immediate));
        set => this.Immediate = BitConverter.ToUInt32(BitConverter.GetBytes(value));
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        cpu.PC += this.AddressOffset;
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (JumpEncodedInstruction.TryDecodeBytecode(bytecode, BinaryCode, out var immediate)) {
            decoded = new J {
                Immediate = immediate
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }
}
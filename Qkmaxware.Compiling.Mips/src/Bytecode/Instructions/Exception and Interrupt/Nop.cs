using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Do nothing
/// </summary>
public class Nop : IBytecodeInstruction {
    public static readonly uint BinaryCode = 0b000000U;
    public uint Opcode => BinaryCode;

    public uint Encode32() {
        return new WordEncoder(0U).Encoded;
    }

    public IEnumerable<uint> GetOperands() {
        yield break;
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (bytecode == 0x0U) {
            decoded = new Nop();
            return true;
        } else {
            decoded = null;
            return false;
        }
    }

    public void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        // Do nothing
    }
}
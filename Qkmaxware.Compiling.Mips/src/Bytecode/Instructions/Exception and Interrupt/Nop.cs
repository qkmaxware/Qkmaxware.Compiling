using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Do nothing
/// </summary>
public class Nop : BaseBytecodeInstruction {
    public static readonly uint BinaryCode = 0b000000U;
    public uint Opcode => BinaryCode;

    public override uint Encode32() {
        return new WordEncoder(0U).Encoded;
    }

    public override IEnumerable<uint> GetOperands() {
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

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        // Do nothing
    }

    /// <summary>
    /// Print this instruction as MIPS assembly code
    /// </summary>
    /// <returns>assembly string</returns>
    public override string ToAssemblyString() => $"{this.InstructionName()}";
}
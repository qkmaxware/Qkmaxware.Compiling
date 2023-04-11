using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Move to lo register (MIPS mtlo)
/// </summary>
public class Mtlo : MoveToInstruction {
    public static readonly uint BinaryCode = 0b010011U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var toMove = cpu.Registers[this.Source].ReadAsUInt32();
        cpu.Registers.LO.WriteUInt32(toMove);
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (RegisterEncodedInstruction.TryDecodeBytecode(bytecode, out var opcode, out var source, out var target, out var dest, out var amount, BinaryCode)) {
            decoded = new Mtlo {
                Source = (RegisterIndex)source
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }
}
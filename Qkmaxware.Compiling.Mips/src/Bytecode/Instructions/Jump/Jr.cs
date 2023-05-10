using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

/// <summary>
/// Jump return (MIPS jr)
/// </summary>
public class Jr : JumpRInstruction {
    public static readonly uint BinaryCode = 0b001000U;
    public override uint Opcode => BinaryCode;


    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        cpu.PC += (int)(cpu.Registers[this.Source].ReadAsUInt32() >> 2); // goto saved pc (as word not bytes)
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (RegisterEncodedInstruction.TryDecodeBytecode(bytecode, out var opcode, out var source, out var target, out var dest, out var amount, BinaryCode)) {
            decoded = new Jr {
                Source = (RegisterIndex)source
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }
}
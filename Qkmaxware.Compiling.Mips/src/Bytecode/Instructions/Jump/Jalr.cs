using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

/// <summary>
/// Jump and link register (MIPS jalr)
/// </summary>
public class Jalr : JumpRInstruction {
    public static readonly uint BinaryCode = 0b001001;
    public override uint Opcode => BinaryCode;
    
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        cpu.Registers[new RegisterIndex(31)].WriteInt32(cpu.PC << 2); // save old pc (as bytes not words)
        cpu.PC = cpu.Registers[this.Source].ReadAsInt32() >> 2;      // goto new pc (as word not bytes)
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (RegisterEncodedInstruction.TryDecodeBytecode(bytecode, out var opcode, out var source, out var target, out var dest, out var amount, BinaryCode)) {
            decoded = new Jalr {
                Source = (RegisterIndex)source
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }
}
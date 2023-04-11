using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Shift left logical of a register by the given amount in another register (MIPS sllv)
/// </summary>
public class Sllv : ShiftVInstruction {
    public static readonly uint BinaryCode = 0b000100U;
    public override uint Opcode => BinaryCode;

    public RegisterIndex LhsOperand {
        get => this.Target;
        set => this.Target = value;
    }
    public RegisterIndex RhsOperand {
        get => this.Source;
        set => this.Source = value;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var lhs = cpu.Registers[this.LhsOperand].ReadAsUInt32();
        var rhs = cpu.Registers[this.RhsOperand].ReadAsUInt32();

        cpu.Registers[this.Destination].WriteUInt32(lhs << (int)rhs);
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (RegisterEncodedInstruction.TryDecodeBytecode(bytecode, out var opcode, out var source, out var target, out var dest, out var amount, BinaryCode)) {
            decoded = new Sllv {
                Destination = (RegisterIndex)dest,
                Target = (RegisterIndex)target, 
                Source = (RegisterIndex)source
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }
}
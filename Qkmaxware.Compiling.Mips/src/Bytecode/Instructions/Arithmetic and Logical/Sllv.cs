using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Shift left logical of a register by the given amount in another register (MIPS sllv)
/// </summary>
public class Sllv : ShiftVInstruction {
    public static readonly uint BinaryCode = 000100U;
    public override uint Opcode => BinaryCode;

    public RegisterIndex LhsOperand {
        get => this.Target;
        set => this.Target = value;
    }
    public RegisterIndex RhsOperand {
        get => this.Source;
        set => this.Source = value;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = cpu.Registers[this.LhsOperand].ReadAsUInt32();
        var rhs = cpu.Registers[this.RhsOperand].ReadAsUInt32();

        cpu.Registers[this.Destination].WriteUInt32(lhs << (int)rhs);
    }
}
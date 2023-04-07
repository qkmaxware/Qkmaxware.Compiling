using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Bitwise OR of a register and an immediate value (MIPS ori)
/// </summary>
public class Ori : ArithLogIInstruction {
    public static readonly uint BinaryCode = 001101U;
    public override uint Opcode => BinaryCode;

    public RegisterIndex LhsOperand {
        get => this.Source;
        set => this.Source = value;
    }
    public uint RhsOperand {
        get => this.Immediate;
        set => this.Immediate = value;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = cpu.Registers[this.LhsOperand].ReadAsUInt32();
        var rhs = this.RhsOperand;

        cpu.Registers[this.Target].WriteUInt32(lhs | rhs);
    }
}
using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Bitwise XOR of a register and an immediate value (MIPS xori)
/// </summary>
public class Xori : ArithLogIInstruction {
    public static readonly uint BinaryCode = 001110U;
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

        cpu.Registers[this.Target].WriteUInt32(lhs ^ rhs);
    }
}
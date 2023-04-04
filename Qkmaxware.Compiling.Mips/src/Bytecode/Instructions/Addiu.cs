namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Unsigned addition of a register and an immediate (MIPS addu)
/// </summary>
public class AddUnsignedImmediate : ArithLogIInstruction {
    public static readonly uint BinaryCode = 001001U;
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

        cpu.Registers[this.Target].WriteUInt32(lhs + rhs);
    }
}
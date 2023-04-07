using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Bitwise AND of two registers (MIPS and)
/// </summary>
public class And : ArithLogInstruction {
    public static readonly uint BinaryCode = 100100U;
    public override uint Opcode => BinaryCode;

    public RegisterIndex LhsOperand {
        get => this.Source;
        set => this.Source = value;
    }
    public RegisterIndex RhsOperand {
        get => this.Target;
        set => this.Target = value;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        var lhs = cpu.Registers[this.LhsOperand].ReadAsUInt32();
        var rhs = cpu.Registers[this.RhsOperand].ReadAsUInt32();

        cpu.Registers[this.Destination].WriteUInt32(lhs & rhs);
    }
}
using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Signed division of two registers (MIPS div)
/// </summary>
public class DivSigned : DivMultInstruction {
    public static readonly uint BinaryCode = 011010U;
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
        var lhs = cpu.Registers[this.LhsOperand].ReadAsInt32();
        var rhs = cpu.Registers[this.RhsOperand].ReadAsInt32();

        var quotient = lhs / rhs;
        var remainder = lhs % rhs;

        cpu.Registers.LO.WriteInt32(quotient);
        cpu.Registers.HI.WriteInt32(remainder);
    }
}
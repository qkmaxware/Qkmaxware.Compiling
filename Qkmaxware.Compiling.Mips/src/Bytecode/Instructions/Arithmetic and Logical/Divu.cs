using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Unsigned division of two registers (MIPS divu)
/// </summary>
public class DivUnsigned : DivMultInstruction {
    public static readonly uint BinaryCode = 011011U;
    public override uint Opcode => BinaryCode;

    public RegisterIndex LhsOperand {
        get => this.Source;
        set => this.Source = value;
    }
    public RegisterIndex RhsOperand {
        get => this.Target;
        set => this.Target = value;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var lhs = cpu.Registers[this.LhsOperand].ReadAsUInt32();
        var rhs = cpu.Registers[this.RhsOperand].ReadAsUInt32();

        var quotient = lhs / rhs;
        var remainder = lhs % rhs;

        cpu.Registers.LO.WriteUInt32(quotient);
        cpu.Registers.HI.WriteUInt32(remainder);
    }
}
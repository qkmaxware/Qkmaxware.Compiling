using Qkmaxware.Compiling.Mips.Hardware;
// ADD 
namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Set on less than for unsigned numbers (MIPS sltu)
/// </summary>
public class Sltu : ArithLogInstruction {
    public static readonly uint BinaryCode = 101001U;
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

        cpu.Registers[this.Destination].WriteUInt32(lhs < rhs ? 1U : 0U);
    }
}
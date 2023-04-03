namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Signed addition of two registers (MIPS add)
/// </summary>
public class AddSigned : ArithLogInstruction {
    public override uint Opcode => 100000U;

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

        cpu.Registers[this.Destination].WriteInt32(lhs + rhs);
    }
}
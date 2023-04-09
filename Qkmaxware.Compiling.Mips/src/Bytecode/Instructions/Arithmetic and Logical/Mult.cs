using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Signed multiplication of two registers (MIPS mult)
/// </summary>
public class MultSigned : DivMultInstruction {
    public static readonly uint BinaryCode = 011000U;
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
        var lhs = cpu.Registers[this.LhsOperand].ReadAsInt32();
        var rhs = cpu.Registers[this.RhsOperand].ReadAsInt32();

        var product = (long)lhs * (long)rhs;

        cpu.Registers.LO.WriteInt32((int)(product >> 32));
        cpu.Registers.HI.WriteInt32((int)(product & 0xFFFFFFFF));
    }
}
using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Unsigned multiplication of two registers (MIPS multu)
/// </summary>
public class MultUnsigned : DivMultInstruction {
    public static readonly uint BinaryCode = 011001U;
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

        var product = (ulong)lhs * (ulong)rhs;

        cpu.Registers.LO.WriteUInt32((uint)(product >> 32));
        cpu.Registers.HI.WriteUInt32((uint)(product & 0xFFFFFFFF));
    }
}
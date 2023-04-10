using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Subtraction of FPU two registers (MIPS sub.s)
/// </summary>
public class SubS : FloatingPointEncodedInstruction {
    public RegisterIndex Destination { get; set; }
    public RegisterIndex LhsOperand { get; set; }
    public RegisterIndex RhsOperand { get; set; }

    public override IEnumerable<uint> GetOperands() {
        yield return (uint) Destination;
        yield return (uint) LhsOperand;
        yield return (uint) RhsOperand;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var lhs = fpu.Registers[this.LhsOperand].Read();
        var rhs = fpu.Registers[this.RhsOperand].Read();

        fpu.Registers[this.Destination].Write(lhs - rhs);
    }

    public override uint Encode32() {
        //   OOOOOOCC CCCTTTTT DDDDDIII IIIIIIII
        return new WordEncoder()
            .Encode(0x11U, 26..32).Encode(0x10, 21..26).Encode((uint)RhsOperand, 16..21).Encode((uint)LhsOperand, 11..16).Encode((uint)Destination, 6..11).Encode(1, 0..6)
            .Encoded;
    }
}
using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Multiplication of FPU two registers (MIPS abs.s)
/// </summary>
public class AbsS : FloatingPointEncodedInstruction {
    public RegisterIndex Destination { get; set; }
    public RegisterIndex Operand { get; set; }

    public override IEnumerable<uint> GetOperands() {
        yield return (uint) Destination;
        yield return (uint) Operand;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var lhs = fpu.Registers[this.Operand].Read();

        fpu.Registers[this.Destination].Write(Math.Abs(lhs));
    }

    public override uint Encode32() {
        //   OOOOOOCC CCCTTTTT DDDDDIII IIIIIIII
        return new WordEncoder()
            .Encode(0x11U, 26..32).Encode(0, 21..26).Encode(0, 16..21).Encode((uint)Operand, 11..16).Encode((uint)Destination, 6..11).Encode(5, 0..6)
            .Encoded;
    }
}
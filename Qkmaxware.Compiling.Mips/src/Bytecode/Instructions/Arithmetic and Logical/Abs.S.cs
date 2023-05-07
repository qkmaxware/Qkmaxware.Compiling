using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Multiplication of FPU two registers (MIPS abs.s)
/// </summary>
public class AbsS : FloatingPointEncodedInstruction {
    public RegisterIndex Destination { get; set; }
    public RegisterIndex Source { get; set; }

    public override IEnumerable<uint> GetOperands() {
        yield return (uint) Destination;
        yield return (uint) Source;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var lhs = fpu.Registers[this.Source].Read();

        fpu.Registers[this.Destination].Write(Math.Abs(lhs));
    }

    public override uint Encode32() {
        //   OOOOOOCC CCCTTTTT DDDDDIII IIIIIIII
        return new WordEncoder()
            .Encode(0x11U, 26..32).Encode(0, 21..26).Encode(0, 16..21).Encode((uint)Source, 11..16).Encode((uint)Destination, 6..11).Encode(5, 0..6)
            .Encoded;
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        // 0x11 0 0 fs fd 5
        decoded = null;
        var word = new WordEncoder(bytecode);
        var opcode = word.Decode(26..32);
        if (opcode != 0x11U) {
            return false; // Single precision opcodes
        }

        var group = word.Decode(21..26);
        if (group != 0) {
            return false; // Group 0
        }

        var ft = word.Decode(16..21);
        if (ft != 0) {
            return false; // No target
        }

        var func = word.Decode(0..6);
        if (func != 5) {
            return false; // Function 5
        }

        var fd = word.Decode(6..11);        // Destination
        var fs = word.Decode(11..16);       // Source operand

        decoded = new AbsS {
            Destination = (RegisterIndex)fd,
            Source = (RegisterIndex)fs
        };
        return true;
    }

    /// <summary>
    /// Print this instruction as MIPS assembly code
    /// </summary>
    /// <returns>assembly string</returns>
    public override string ToAssemblyString() => $"{this.InstructionName()} {this.Destination}, {this.Source}";
}
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Multiplication of FPU two registers (MIPS mul.s)
/// </summary>
public class MulS : FloatingPointEncodedInstruction {
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

        fpu.Registers[this.Destination].Write(lhs * rhs);
    }

    public override uint Encode32() {
        //   OOOOOOCC CCCTTTTT DDDDDIII IIIIIIII
        return new WordEncoder()
            .Encode(0x11U, 26..32).Encode(0x10, 21..26).Encode((uint)RhsOperand, 16..21).Encode((uint)LhsOperand, 11..16).Encode((uint)Destination, 6..11).Encode(2, 0..6)
            .Encoded;
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        // 0x11 0x10 ft fs fd 2
        decoded = null;
        var word = new WordEncoder(bytecode);
        var opcode = word.Decode(26..32);
        if (opcode != 0x11U) {
            return false; // Single precision opcodes
        }

        var group = word.Decode(21..26);
        if (group != 0x10) {
            return false; // Group 
        }

        var ft = word.Decode(16..21);

        var fs = word.Decode(11..16);     

        var fd = word.Decode(6..11);        

        var func = word.Decode(0..6);
        if (func != 2) {
            return false; // Function
        }

        decoded = new MulS {
            Destination = (RegisterIndex)fd,
            LhsOperand = (RegisterIndex)fs,
            RhsOperand = (RegisterIndex)ft
        };
        return true;
    }
}
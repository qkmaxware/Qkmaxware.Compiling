// Branch on coprocessor 1 true
// https://www.cs.nott.ac.uk/~psztxa/g51csa/l09-hand.pdf
// page 12

using Qkmaxware.Compiling.Targets.Mips.Hardware;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

/// <summary>
/// Branch on CO1 condition (MIPS bc1t)
/// </summary>
public class Bc1t : FloatingPointEncodedInstruction {

    public uint ConditionFlagIndex {get; set;}
    public int AddressOffset {get; set;}

    /// <summary>
    /// The written format of this assembly instruction
    /// </summary>
    /// <returns>description</returns>
    public string AssemblyFormat() => $"{this.InstructionName()} cc, offset";
    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public string InstructionDescription() => "If the FPU condition flag at index cc is set then move the PC by the given offset";

    public override string ToAssemblyString() => $"{this.InstructionName()} {ConditionFlagIndex}, {AddressOffset}";

    public override IEnumerable<uint> GetOperands() {
        yield return ConditionFlagIndex;
        yield return BitConverter.ToUInt32(BitConverter.GetBytes(AddressOffset));
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        if (fpu.Flags[(int)this.ConditionFlagIndex].IsSet) {
            cpu.PC += this.AddressOffset >> 2;
        }
    }
    
    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) { yield return this; }

    public override uint Encode32() {
        return new WordEncoder()
            .Encode(0x11U, 26..32)
            .Encode(8, 21..26)
            .Encode(this.ConditionFlagIndex, 18..21)
            .Encode(1, 16..18)
            .Encode(this.AddressOffset, 0..16)
            .Encoded;
    }
    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        decoded = null;
        var word = new WordEncoder(bytecode);
        if (word.Decode(26..32) != 0x11U) {
            return false; // Single precision opcodes
        }

        if (word.Decode(21..26) != 8) {
            return false; // Group 8
        }

        var cc = word.Decode(18..21); // Condition flag

        if (word.Decode(16..18) != 1) {
            return false; // Function 1
        }

        var offset = word.Decode(0..16);

        decoded = new Bc1t {
            ConditionFlagIndex = cc,
            AddressOffset = BitConverter.ToInt32(BitConverter.GetBytes(offset.SignExtend(15))), // last bit is the sign
        };
        return true;
    }
}
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

/// <summary>
/// Compare FPU registers for equality (MIPS c.eq.s)
/// </summary>
public class CLtS : FloatingPointEncodedInstruction, IAssemblyInstruction {
    public uint FlagIndex { get; set; }
    public RegisterIndex LhsOperand { get; set; }
    public RegisterIndex RhsOperand { get; set; }

    /// <summary>
    /// The written format of this instruction in assembly
    /// </summary>
    /// <returns>description</returns>
    public string AssemblyFormat() => $"{this.InstructionName()} cc, $lhs, $rhs";

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public string InstructionDescription() => "If $lhs < $rhs set the FPU flag given by cc.";

    public override IEnumerable<uint> GetOperands() {
        yield return (uint) FlagIndex;
        yield return (uint) LhsOperand;
        yield return (uint) RhsOperand;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var lhs = fpu.Registers[this.LhsOperand].Read();
        var rhs = fpu.Registers[this.RhsOperand].Read();

        fpu.Flags[(int)this.FlagIndex].SetIf(lhs < rhs);
    }

    /// <summary>
    /// Print this instruction as MIPS assembly code
    /// </summary>
    /// <returns>assembly string</returns>
    public override string ToAssemblyString() => $"{this.InstructionName()} {this.FlagIndex}, {this.LhsOperand}, {this.RhsOperand}";

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) { yield return this; }

    public override uint Encode32() {
        //   OOOOOOCC CCCTTTTT DDDDDIII IIIIIIII
        return new WordEncoder()
            .Encode(0x11U, 26..32)
            .Encode(0x10U, 21..26)
            .Encode(this.RhsOperand, 16..21)
            .Encode(this.LhsOperand, 11..16)
            .Encode(this.FlagIndex, 8..11)
            .Encode(0, 6..8)
            .Encode(0, 4..6) // FC what does this mean?
            .Encode(0xCU, 0..4)
            .Encoded;
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        // 0x11 0x10 ft fs fd 0
        decoded = null;
        var word = new WordEncoder(bytecode);

        if (!word.Is(26..32, 0x11U)) return false;
        if (!word.Is(21..26, 0x10U)) return false;


        var rhs = word.Decode(16..21);

        var lhs = word.Decode(11..16);     

        var cc = word.Decode(8..11);        

        if (!word.Is(6..8, 0U)) return false;
        if (!word.Is(0..4, 0xCU)) return false;

        decoded = new CLtS {
            FlagIndex = cc,
            LhsOperand = (RegisterIndex)lhs,
            RhsOperand = (RegisterIndex)rhs
        };
        return true;
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out IAssemblyInstruction? decoded) {
        Assembly.ScalarConstantToken cc; Assembly.RegisterToken lhs; Assembly.RegisterToken rhs;
        if (!IsAssemblyFormatDestLhsRhs<CLtS, Assembly.ScalarConstantToken, Assembly.RegisterToken, Assembly.RegisterToken>(opcode, args, out cc, out lhs, out rhs)) {
            decoded = null;
            return false;
        }

        decoded = new CLtS {
            FlagIndex = (uint)cc.IntegerValue,
            LhsOperand = lhs.Value,
            RhsOperand = rhs.Value,
        };
        return true;
    }

}
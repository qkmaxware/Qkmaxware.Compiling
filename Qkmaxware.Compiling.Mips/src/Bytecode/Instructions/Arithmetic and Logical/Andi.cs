using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Bitwise AND of a register and an immediate value (MIPS andi)
/// </summary>
public class Andi : ArithLogIInstruction {
    public static readonly uint BinaryCode = 0b001100U;
    public override uint Opcode => BinaryCode;

    /// <summary>
    /// The written format of this instruction in assembly
    /// </summary>
    /// <returns>description</returns>
    public override string AssemblyFormat() => $"{this.InstructionName} $dest, $lhs, value";

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public override string InstructionDescription() => "Compute $lhs & value and store the result in $dest.";

    public RegisterIndex LhsOperand {
        get => this.Source;
        set => this.Source = value;
    }
    public uint RhsOperand {
        get => this.Immediate;
        set => this.Immediate = value;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var lhs = cpu.Registers[this.LhsOperand].ReadAsUInt32();
        var rhs = this.RhsOperand;

        cpu.Registers[this.Target].WriteUInt32(lhs & rhs);
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (ImmediateEncodedInstruction.TryDecodeBytecode(bytecode, BinaryCode, out var source, out var target, out var immediate)) {
            decoded = new Andi {
                Target = (RegisterIndex)target,
                LhsOperand = (RegisterIndex)source,
                Immediate = immediate
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out Mips.Assembly.IAssemblyInstruction? decoded) {
        Assembly.RegisterToken dest; Assembly.RegisterToken lhs; Assembly.ScalarConstantToken rhs;
        if (!IsAssemblyFormatDestLhsRhs<Andi, Assembly.RegisterToken, Assembly.RegisterToken, Assembly.ScalarConstantToken>(opcode, args, out dest, out lhs, out rhs)) {
            decoded = null;
            return false;
        }

        decoded = new Andi {
            Target = dest.Value,
            LhsOperand = lhs.Value,
            RhsOperand = (uint)rhs.IntegerValue,
        };
        return true;
    }
}
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Unsigned multiplication of two registers (MIPS multu)
/// </summary>
public class Multu : DivMultInstruction {
    public static readonly uint BinaryCode = 0b011001U;
    public override uint Opcode => BinaryCode;

    /// <summary>
    /// The written format of this instruction in assembly
    /// </summary>
    /// <returns>description</returns>
    public override string AssemblyFormat() => $"{this.InstructionName} $lhs, $rhs";

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public override string InstructionDescription() => "Compute $lhs * $rhs storing the result's highest bits in HI lowest bits in LO.";

    public RegisterIndex LhsOperand {
        get => this.Source;
        set => this.Source = value;
    }
    public RegisterIndex RhsOperand {
        get => this.Target;
        set => this.Target = value;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var lhs = cpu.Registers[this.LhsOperand].ReadAsUInt32();
        var rhs = cpu.Registers[this.RhsOperand].ReadAsUInt32();

        var product = (ulong)lhs * (ulong)rhs;

        cpu.Registers.LO.WriteUInt32((uint)(product >> 32));
        cpu.Registers.HI.WriteUInt32((uint)(product & 0xFFFFFFFF));
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (RegisterEncodedInstruction.TryDecodeBytecode(bytecode, out var opcode, out var source, out var target, out var dest, out var amount, BinaryCode)) {
            decoded = new Div {
                LhsOperand = (RegisterIndex)source,
                RhsOperand = (RegisterIndex)target,
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out Mips.Assembly.IAssemblyInstruction? decoded) {
        Assembly.RegisterToken lhs; Assembly.RegisterToken rhs;
        if (!IsAssemblyFormatLhsRhs<Multu, Assembly.RegisterToken, Assembly.RegisterToken>(opcode, args, out lhs, out rhs)) {
            decoded = null;
            return false;
        }

        decoded = new Multu {
            LhsOperand = lhs.Value,
            RhsOperand = rhs.Value
        };
        return true;
    }
}
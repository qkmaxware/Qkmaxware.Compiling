using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Unsigned division of two registers (MIPS divu)
/// </summary>
public class Divu : DivMultInstruction, Assembly.IAssemblyInstruction {
    public static readonly uint BinaryCode = 0b011011U;
    public override uint Opcode => BinaryCode;

    /// <summary>
    /// The written format of this instruction in assembly
    /// </summary>
    /// <returns>description</returns>
    public override string AssemblyFormat() => $"{this.InstructionName()} $lhs, $rhs";

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public override string InstructionDescription() => "Compute $lhs / $rhs storing the result in LO and compute $lhs % $rhs storing the result in HI.";

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) { yield return this; }

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

        var quotient = lhs / rhs;
        var remainder = lhs % rhs;

        cpu.Registers.LO.WriteUInt32(quotient);
        cpu.Registers.HI.WriteUInt32(remainder);
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (RegisterEncodedInstruction.TryDecodeBytecode(bytecode, out var opcode, out var source, out var target, out var dest, out var amount, BinaryCode)) {
            decoded = new Divu {
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
        if (!IsAssemblyFormatLhsRhs<Divu, Assembly.RegisterToken, Assembly.RegisterToken>(opcode, args, out lhs, out rhs)) {
            decoded = null;
            return false;
        }

        decoded = new Divu {
            LhsOperand = lhs.Value,
            RhsOperand = rhs.Value
        };
        return true;
    }
}
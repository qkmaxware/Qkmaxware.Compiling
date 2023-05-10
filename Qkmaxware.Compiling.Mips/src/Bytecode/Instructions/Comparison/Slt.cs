using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Set on less than for signed numbers (MIPS slt)
/// </summary>
public class Slt : ArithLogInstruction, Assembly.IAssemblyInstruction {
    public static readonly uint BinaryCode = 0b101010U;
    public override uint Opcode => BinaryCode;

    /// <summary>
    /// The written format of this instruction in assembly
    /// </summary>
    /// <returns>description</returns>
    public override string AssemblyFormat() => $"{this.InstructionName()} $dest, $lhs, $rhs";

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public override string InstructionDescription() => "if $lhs < $rhs store 1 in $dest otherwise store 0 in $dest";

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
        var lhs = cpu.Registers[this.LhsOperand].ReadAsInt32();
        var rhs = cpu.Registers[this.RhsOperand].ReadAsInt32();

        cpu.Registers[this.Destination].WriteInt32(lhs < rhs ? 1 : 0);
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (RegisterEncodedInstruction.TryDecodeBytecode(bytecode, out var opcode, out var source, out var target, out var dest, out var amount, BinaryCode)) {
            decoded = new Slt {
                LhsOperand = (RegisterIndex)source,
                RhsOperand = (RegisterIndex)target,
                Destination = (RegisterIndex)dest,
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out Mips.Assembly.IAssemblyInstruction? decoded) {
        Assembly.RegisterToken dest; Assembly.RegisterToken lhs; Assembly.RegisterToken rhs;
        if (!IsAssemblyFormatDestLhsRhs<Slt, Assembly.RegisterToken, Assembly.RegisterToken, Assembly.RegisterToken>(opcode, args, out dest, out lhs, out rhs)) {
            decoded = null;
            return false;
        }

        decoded = new Slt {
            Destination = dest.Value,
            LhsOperand = lhs.Value,
            RhsOperand = rhs.Value,
        };
        return true;
    }
}
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Move from lo register (MIPS mflo)
/// </summary>
public class Mflo : MoveFromInstruction {
    public static readonly uint BinaryCode = 0b010010U;
    public override uint Opcode => BinaryCode;

    /// <summary>
    /// The written format of this instruction in assembly
    /// </summary>
    /// <returns>description</returns>
    public override string AssemblyFormat() => $"{this.InstructionName} $dest";

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public override string InstructionDescription() => "Move a value stored in the special LO register into $dest";

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var toMove = cpu.Registers.LO.ReadAsUInt32();
        cpu.Registers[this.Destination].WriteUInt32(toMove);
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (RegisterEncodedInstruction.TryDecodeBytecode(bytecode, out var opcode, out var source, out var target, out var dest, out var amount, BinaryCode)) {
            decoded = new Mflo {
                Destination = (RegisterIndex)dest
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out Mips.Assembly.IAssemblyInstruction? decoded) {
        Assembly.RegisterToken dest; 
        if (!IsAssemblyFormatDest<Mflo, Assembly.RegisterToken>(opcode, args, out dest)) {
            decoded = null;
            return false;
        }

        decoded = new Mflo {
            Destination = dest.Value,
        };
        return true;
    }
}
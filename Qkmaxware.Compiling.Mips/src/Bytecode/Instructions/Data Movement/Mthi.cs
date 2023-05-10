using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Move to hi register (MIPS mthi)
/// </summary>
public class Mthi : MoveToInstruction, Assembly.IAssemblyInstruction {
    public static readonly uint BinaryCode = 0b010001U;
    public override uint Opcode => BinaryCode;

    /// <summary>
    /// The written format of this instruction in assembly
    /// </summary>
    /// <returns>description</returns>
    public override string AssemblyFormat() => $"{this.InstructionName()} $arg";

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public override string InstructionDescription() => "Move a value stored in $arg into the special HI register";

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) { yield return this; }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var toMove = cpu.Registers[this.Source].ReadAsUInt32();
        cpu.Registers.HI.WriteUInt32(toMove);
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (RegisterEncodedInstruction.TryDecodeBytecode(bytecode, out var opcode, out var source, out var target, out var dest, out var amount, BinaryCode)) {
            decoded = new Mthi {
                Source = (RegisterIndex)source
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out Mips.Assembly.IAssemblyInstruction? decoded) {
        Assembly.RegisterToken dest; 
        if (!IsAssemblyFormatArg<Mthi, Assembly.RegisterToken>(opcode, args, out dest)) {
            decoded = null;
            return false;
        }

        decoded = new Mthi {
            Source = dest.Value,
        };
        return true;
    }
}
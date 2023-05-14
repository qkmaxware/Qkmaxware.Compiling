using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

/// <summary>
/// Load bits into the upper half-word, the lower half-word is cleared
/// </summary>
public class Lui : LoadIInstruction, IAssemblyInstruction {
    public static readonly uint BinaryCode = 0xF;
    public override uint Opcode => BinaryCode;

    public RegisterIndex Destination {
        get => this.Target;
        set => this.Target = value;
    }

    /// <summary>
    /// The written format of this instruction in assembly
    /// </summary>
    /// <returns>description</returns>
    public string AssemblyFormat() => $"{this.InstructionName()} $dest, value";

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public string InstructionDescription() => "Store the given value in the high half-word of $dest the lower half-word are cleared";

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) { yield return this; }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var value = (this.Immediate << 16);
        
        cpu.Registers[this.Target].WriteUInt32(value);
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (ImmediateEncodedInstruction.TryDecodeBytecode(bytecode, BinaryCode, out var source, out var target, out var immediate)) {
            decoded = new Lui {
                Target = (RegisterIndex)target,
                Immediate = immediate
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out IAssemblyInstruction? decoded) {
        Assembly.RegisterToken dest; Assembly.ScalarConstantToken arg;
        if (!IsAssemblyFormatDestArg<Lui, Assembly.RegisterToken, Assembly.ScalarConstantToken>(opcode, args, out dest, out arg)) {
            decoded = null;
            return false;
        }

        decoded = new Lui {
            Destination = dest.Value,
            Immediate = (uint)arg.IntegerValue,
        };
        return true;
    }
}
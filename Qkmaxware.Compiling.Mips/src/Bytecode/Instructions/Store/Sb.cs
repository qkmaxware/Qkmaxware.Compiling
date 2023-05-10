using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Store signed byte (MIPS sb)
/// </summary>
public class Sb : LoadStoreInstruction, Assembly.IAssemblyInstruction {
    public static readonly uint BinaryCode = 0b101000U;
    public override uint Opcode => BinaryCode;

    /// <summary>
    /// The written format of this instruction in assembly
    /// </summary>
    /// <returns>description</returns>
    public override string AssemblyFormat() => $"{this.InstructionName()} $src, offset($base)";

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public override string InstructionDescription() => "Store a lowest byte into memory address $base + offset from $src.";

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) { yield return this; }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var address = cpu.Registers[this.Source].ReadAsUInt32() + this.Immediate;
        memory.StoreByte(address, cpu.Registers[this.Target].ReadAsUInt32().LowByte());
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (ImmediateEncodedInstruction.TryDecodeBytecode(bytecode, BinaryCode, out var source, out var target, out var immediate)) {
            decoded = new Sb {
                Target = (RegisterIndex)target,
                Source = (RegisterIndex)source,
                Immediate = immediate
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out Mips.Assembly.IAssemblyInstruction? decoded) {
        Assembly.RegisterToken dest; Assembly.RegisterToken @base; Assembly.ScalarConstantToken offset;
        if (!IsAssemblyFormatSourceOffsetBase<Sb, Assembly.RegisterToken, Assembly.ScalarConstantToken, Assembly.RegisterToken>(opcode, args, out dest, out offset, out @base)) {
            decoded = null;
            return false;
        }

        decoded = new Sb {
            Target = dest.Value,
            Source = @base.Value,
            Immediate = (uint)offset.IntegerValue
        };
        return true;
    }
}
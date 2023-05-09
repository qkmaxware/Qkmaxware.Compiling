using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Load signed byte (MIPS lb)
/// </summary>
public class Lb : LoadStoreInstruction {
    public static readonly uint BinaryCode = 0b100000U;
    public override uint Opcode => BinaryCode;

    /// <summary>
    /// The written format of this instruction in assembly
    /// </summary>
    /// <returns>description</returns>
    public override string AssemblyFormat() => $"{this.InstructionName} $dest, offset($base)";

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public override string InstructionDescription() => "Load a byte from memory address $base + offset into $dest preserving its sign.";

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var raw = memory.LoadByte(cpu.Registers[this.Source].ReadAsUInt32() + this.Immediate);
        var extended = ((uint)raw).SignExtend(8);
        cpu.Registers[this.Target].WriteUInt32(extended);
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (ImmediateEncodedInstruction.TryDecodeBytecode(bytecode, BinaryCode, out var source, out var target, out var immediate)) {
            decoded = new Lb {
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
        if (!IsAssemblyFormatDestOffsetBase<Lb, Assembly.RegisterToken, Assembly.ScalarConstantToken, Assembly.RegisterToken>(opcode, args, out dest, out offset, out @base)) {
            decoded = null;
            return false;
        }

        decoded = new Lb {
            Target = dest.Value,
            Source = @base.Value,
            Immediate = (uint)offset.IntegerValue
        };
        return true;
    }
}
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Load unsigned byte (MIPS lbu)
/// </summary>
public class Lbu : LoadStoreInstruction {
    public static readonly uint BinaryCode = 0b100100U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var raw = memory.LoadByte(cpu.Registers[this.Source].ReadAsUInt32() + this.Immediate);
        var extended = ((uint)raw);
        cpu.Registers[this.Target].WriteUInt32(extended);
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (ImmediateEncodedInstruction.TryDecodeBytecode(bytecode, BinaryCode, out var source, out var target, out var immediate)) {
            decoded = new Lbu {
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
        if (!IsAssemblyFormatDestOffsetBase<Lbu, Assembly.RegisterToken, Assembly.ScalarConstantToken, Assembly.RegisterToken>(opcode, args, out dest, out offset, out @base)) {
            decoded = null;
            return false;
        }

        decoded = new Lbu {
            Target = dest.Value,
            Source = @base.Value,
            Immediate = (uint)offset.IntegerValue
        };
        return true;
    }
}
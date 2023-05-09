using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Load high bits (MIPS lhi)
/// </summary>
public class Lhi : LoadIInstruction {
    public static readonly uint BinaryCode = 0b011001U;
    public override uint Opcode => BinaryCode;

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var value = (this.Immediate << 16);
        var prev = cpu.Registers[this.Target].ReadAsUInt32().ClearHighHalf();
        
        cpu.Registers[this.Target].WriteUInt32(prev | value);
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        if (ImmediateEncodedInstruction.TryDecodeBytecode(bytecode, BinaryCode, out var source, out var target, out var immediate)) {
            decoded = new Lhi {
                Target = (RegisterIndex)target,
                Immediate = immediate
            };
            return true;
        } else {
            decoded = null;
            return false;
        }
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out Mips.Assembly.IAssemblyInstruction? decoded) {
        Assembly.RegisterToken dest; Assembly.ScalarConstantToken arg;
        if (!IsAssemblyFormatDestArg<Lhi, Assembly.RegisterToken, Assembly.ScalarConstantToken>(opcode, args, out dest, out arg)) {
            decoded = null;
            return false;
        }

        decoded = new Lhi {
            Target = dest.Value,
            Immediate = (uint)arg.IntegerValue,
        };
        return true;
    }
}
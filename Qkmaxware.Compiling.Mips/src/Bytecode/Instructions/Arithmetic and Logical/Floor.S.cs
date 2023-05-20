// single precision square root (page 31)
using Qkmaxware.Compiling.Targets.Mips.Hardware;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

/// <summary>
/// Floor to word of an FPU register (MIPS floor.w.s)
/// </summary>
public class FloorWS : FloatingPointEncodedInstruction, IAssemblyInstruction {
    public RegisterIndex Destination { get; set; }
    public RegisterIndex Source { get; set; }

    /// <summary>
    /// The written format of this instruction in assembly
    /// </summary>
    /// <returns>description</returns>
    public string AssemblyFormat() => $"{this.InstructionName()} $dest, $arg";

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public string InstructionDescription() => "Compute the floor of the floating point value stored in $arg and store it in $dest as an integer.";

    public override IEnumerable<uint> GetOperands() {
        yield return (uint) Destination;
        yield return (uint) Source;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var lhs = fpu.Registers[this.Source].Read();

        fpu.Registers[this.Destination].WriteInt32((int)MathF.Floor(lhs));
    }

    public override uint Encode32() {
        //   OOOOOOCC CCCTTTTT DDDDDIII IIIIIIII
        return new WordEncoder()
            .Encode(0x11U, 26..32)
            .Encode(0x10U, 21..26)
            .Encode(0, 16..21)
            .Encode((uint)Source, 11..16)
            .Encode((uint)Destination, 6..11)
            .Encode(0xFU, 0..6)
            .Encoded;
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        // 0x11 0 0 fs fd 5
        decoded = null;
        var word = new WordEncoder(bytecode);
        var opcode = word.Decode(26..32);
        if (opcode != 0x11U) {
            return false; // Single precision opcodes
        }

        var group = word.Decode(21..26);
        if (group != 0x10U) {
            return false; // Group 0
        }

        var ft = word.Decode(16..21);
        if (ft != 0) {
            return false; // No target
        }

        var func = word.Decode(0..6);
        if (func != 0xFU) {
            return false; // Function 0xf
        }

        var fd = word.Decode(6..11);        // Destination
        var fs = word.Decode(11..16);       // Source operand

        decoded = new FloorWS {
            Destination = (RegisterIndex)fd,
            Source = (RegisterIndex)fs
        };
        return true;
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out IAssemblyInstruction? decoded) {
        Assembly.RegisterToken dest; Assembly.RegisterToken arg;
        if (!IsAssemblyFormatDestArg<FloorWS, Assembly.RegisterToken, Assembly.RegisterToken>(opcode, args, out dest, out arg)) {
            decoded = null;
            return false;
        }

        decoded = new FloorWS {
            Destination = dest.Value,
            Source = arg.Value
        };
        return true;
    }

    /// <summary>
    /// Print this instruction as MIPS assembly code
    /// </summary>
    /// <returns>assembly string</returns>
    public override string ToAssemblyString() => $"{this.InstructionName()} {this.Destination}, {this.Source}";

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) { yield return this; }
}
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

/// <summary>
/// Move to FPU from CPU register with no conversion (MIPS mtc1)
/// </summary>
public class Mtc1 : FloatingPointEncodedInstruction, IAssemblyInstruction {
    public RegisterIndex CpuRegister {get; set;}
    public RegisterIndex FpuRegister {get; set;}

    /// <summary>
    /// The written format of this instruction in assembly
    /// </summary>
    /// <returns>description</returns>
    public string AssemblyFormat() => $"{this.InstructionName()} $dest, $arg";

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public string InstructionDescription() => "Move a value from CPU register $arg to FPU register $dest";

    public IEnumerable<IBytecodeInstruction> Assemble(AssemblerEnvironment env) { yield return this; }

    public override IEnumerable<uint> GetOperands() {
        yield return (uint) CpuRegister;
        yield return (uint) FpuRegister;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var toMove = cpu.Registers[this.CpuRegister].ReadAsUInt32();
        fpu.Registers[this.FpuRegister].WriteUInt32(toMove);
    }

    public override uint Encode32() {
        //   OOOOOOCC CCCTTTTT DDDDDIII IIIIIIII
        return new WordEncoder()
            .Encode(0x11U, 26..32).Encode(4, 21..26).Encode((uint)FpuRegister, 16..21).Encode((uint)CpuRegister, 11..16)
            .Encoded;
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        // 0x11 4 rt fs 0
        decoded = null;
        var word = new WordEncoder(bytecode);
        var opcode = word.Decode(26..32);
        if (opcode != 0x11U) {
            return false; // Single precision opcodes
        }

        var group = word.Decode(21..26);
        if (group != 4) {
            return false; // Group 
        }

        var rt = word.Decode(16..21);

        var rd = word.Decode(11..16);           

        var func = word.Decode(0..11);
        if (func != 0) {
            return false; // Function
        }

        decoded = new Mtc1 {
            FpuRegister = (RegisterIndex)rd,
            CpuRegister = (RegisterIndex)rt
        };
        return true;
    }

    public static bool TryDecodeAssembly(Assembly.IdentifierToken opcode, List<Mips.Assembly.Token> args, out IAssemblyInstruction? decoded) {
        Assembly.RegisterToken dest; Assembly.RegisterToken arg;
        if (!IsAssemblyFormatDestArg<Mtc1, Assembly.RegisterToken, Assembly.RegisterToken>(opcode, args, out dest, out arg)) {
            decoded = null;
            return false;
        }

        decoded = new Mtc1 {
            FpuRegister = dest.Value,
            CpuRegister = arg.Value,
        };
        return true;
    }

    /// <summary>
    /// Print this instruction as MIPS assembly code
    /// </summary>
    /// <returns>assembly string</returns>
    public override string ToAssemblyString() => $"{this.InstructionName()} {this.FpuRegister}, {this.CpuRegister}";
}
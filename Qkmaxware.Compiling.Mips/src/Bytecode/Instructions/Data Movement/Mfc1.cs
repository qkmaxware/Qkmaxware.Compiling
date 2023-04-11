using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Move from FPU to CPU register with no conversion  (MIPS mfc1)
/// </summary>
public class Mfc1 : FloatingPointEncodedInstruction {

    public RegisterIndex CpuRegister {get; set;}
    public RegisterIndex FpuRegister {get; set;}

    public override IEnumerable<uint> GetOperands() {
        yield return (uint) CpuRegister;
        yield return (uint) FpuRegister;
    }

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io) {
        var toMove = fpu.Registers[this.FpuRegister].ReadAsUInt32();
        cpu.Registers[this.CpuRegister].WriteUInt32(toMove);
    }

    public override uint Encode32() {
        //   OOOOOOCC CCCTTTTT DDDDDIII IIIIIIII
        return new WordEncoder()
            .Encode(0x11U, 26..32).Encode(0, 21..26).Encode((uint)CpuRegister, 16..21).Encode((uint)FpuRegister, 11..16)
            .Encoded;
    }

    public static bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded) {
        // 0x11 0 rt fs 0
        decoded = null;
        var word = new WordEncoder(bytecode);
        var opcode = word.Decode(26..32);
        if (opcode != 0x11U) {
            return false; // Single precision opcodes
        }

        var group = word.Decode(21..26);
        if (group != 0) {
            return false; // Group 
        }

        var rt = word.Decode(16..21);

        var fs = word.Decode(11..16);           

        var func = word.Decode(0..11);
        if (func != 0) {
            return false; // Function
        }

        decoded = new Mfc1 {
            FpuRegister = (RegisterIndex)fs,
            CpuRegister = (RegisterIndex)rt
        };
        return true;
    }
}
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
}
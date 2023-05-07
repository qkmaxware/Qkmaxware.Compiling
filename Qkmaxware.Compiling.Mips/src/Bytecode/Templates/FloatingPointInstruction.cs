using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Base class for instructions with Floating-point type encoding
/// </summary>
public abstract class FloatingPointEncodedInstruction : IBytecodeInstruction {
    public uint Opcode => 0x11U;
    public abstract void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io);
    public abstract uint Encode32();

    public abstract IEnumerable<uint> GetOperands();
}
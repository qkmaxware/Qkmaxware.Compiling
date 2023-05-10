using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Base class for instructions with Floating-point type encoding
/// </summary>
public abstract class FloatingPointEncodedInstruction : BaseBytecodeInstruction {
    public uint Opcode => 0x11U;
}
using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Interface for all MIPS bytecode instructions
/// </summary>
public interface IBytecodeInstruction {
    /// <summary>
    /// Numeric value representing the instruction
    /// </summary>
    /// <value>instruction code</value>
    public uint Opcode {get;}
    /// <summary>
    /// Encode the operation in MIPS32 bytecode
    /// </summary>
    /// <returns>encoded operation</returns>
    public uint Encode32();
    /// <summary>
    /// Execute the instruction on simulated hardware 
    /// </summary>
    /// <param name="cpu">Simulated MIPS CPU</param>
    /// <param name="fpu">Simulated MIPS Floating-point coprocessor</param>
    /// <param name="memory">Simulator linear byte-addressable memory</param>
    public void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io);

    public string InstructionName() => this.GetType().Name;

    public IEnumerable<uint> GetOperands();
}
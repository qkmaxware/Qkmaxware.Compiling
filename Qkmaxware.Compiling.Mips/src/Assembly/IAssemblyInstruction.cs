namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

/// <summary>
/// Interface for all MIPS assembly instructions
/// </summary>
public interface IAssemblyInstruction {
    /// <summary>
    /// Name of the instruction
    /// </summary>
    /// <returns>instruction's name</returns>
    public string InstructionName();
    /// <summary>
    /// The written format of this assembly instruction
    /// </summary>
    /// <returns>description</returns>
    public string AssemblyFormat();
    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public string InstructionDescription();
    /// <summary>
    /// Print this instruction as MIPS assembly code
    /// </summary>
    /// <returns>assembly string</returns>
    public string ToAssemblyString();
}

/// <summary>
/// Base interface for assembly instructions which are not actual hardware instructions and exist only to the assembler
/// </summary>
public interface IPseudoInstruction : IAssemblyInstruction {}
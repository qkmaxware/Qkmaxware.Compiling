using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

/// <summary>
/// Interface for value operands supported by MIPS
/// </summary>
public interface IMipsValueOperand {
    /// <summary>
    /// Encode the loading of this value into MIPS hardware
    /// </summary>
    /// <param name="index">register to load value into</param>
    /// <returns>list of assembly instructions</returns>
    public IEnumerable<IAssemblyInstruction> MipsInstructionsToLoadValueInto(RegisterIndex index);
}
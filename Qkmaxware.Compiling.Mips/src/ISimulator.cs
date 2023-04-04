using System.Collections.Generic;

namespace Qkmaxware.Compiling.Mips;

/// <summary>
/// Interface that all MIPS simulators must implement
/// </summary>
public interface ISimulator {
    public int Execute (List<Bytecode.IBytecodeInstruction> instrs);
}
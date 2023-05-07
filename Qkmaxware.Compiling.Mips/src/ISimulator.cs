using System.Collections.Generic;

namespace Qkmaxware.Compiling.Targets.Mips;

/// <summary>
/// Interface that all MIPS simulators must implement
/// </summary>
public interface ISimulator {
    public int Execute (Bytecode.BytecodeProgram instrs);
}
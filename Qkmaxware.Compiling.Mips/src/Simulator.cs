using System.Collections.Generic;

namespace Qkmaxware.Compiling.Mips;

/// <summary>
/// MIPS 32 simulator
/// </summary>
public class Simulator {
    private Cpu cpu;
    private Fpu fpu;
    private IMemory memory;
    private Fpu coprocessor0 => fpu;

    public Simulator(IMemory memory) {
        this.cpu = new Cpu();
        this.fpu = new Fpu();
        this.memory = memory;
    }    

    public void Execute(InstructionSet.IInstruction instr) {
        instr.Invoke(cpu, fpu, memory);
    }
    
    public void Execute (IEnumerable<InstructionSet.IInstruction> instrs) {
        foreach (var instr in instrs) {
            Execute(instr);
        }
    }
}
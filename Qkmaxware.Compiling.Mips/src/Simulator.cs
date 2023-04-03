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

    public void Execute(Bytecode.IBytecodeInstruction instr) {
        instr.Invoke(cpu, fpu, memory);
    }
    
    public int Execute (List<Bytecode.IBytecodeInstruction> instrs) {
        while (true) {
            if (cpu.PC < 0 || cpu.PC >= instrs.Count)
                break;
            var instr = instrs[cpu.PC];
            cpu.PC++;
            try {
                Execute(instr);
            } catch (ExitException exit) {
                return exit.ExitCode;
            }
        }
        return 0;
    }
}

/// <summary>
/// Exception thrown when a simulated program exits using the exit system call
/// </summary>
public class ExitException : System.Exception {
    public int ExitCode {get; private set;}
    public ExitException() : this(0) {}
    public ExitException(int code) {
        this.ExitCode = code;
    }
}
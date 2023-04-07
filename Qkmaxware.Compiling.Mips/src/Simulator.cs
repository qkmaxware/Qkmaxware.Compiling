using System.Collections.Generic;
using Qkmaxware.Compiling.Mips.Hardware;

namespace Qkmaxware.Compiling.Mips;

/// <summary>
/// MIPS 32 simulator
/// </summary>
public class Simulator : ISimulator {
    protected Cpu cpu {get; private set;}
    protected Fpu fpu {get; private set;}
    protected IMemory memory {get; private set;}
    protected Fpu coprocessor0 => fpu;

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
                OnBeforeInstruction(instr);
                Execute(instr);
                OnAfterInstruction(instr);
            } catch (ExitException exit) {
                return exit.ExitCode;
            }
        }
        return 0;
    }

    public virtual void OnBeforeInstruction(Bytecode.IBytecodeInstruction instr) {}
    public virtual void OnAfterInstruction(Bytecode.IBytecodeInstruction instr) {}
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
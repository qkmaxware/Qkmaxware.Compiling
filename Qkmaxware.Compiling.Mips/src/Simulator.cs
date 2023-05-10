using System.Collections.Generic;
using Qkmaxware.Compiling.Targets.Mips.Hardware;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips;

public class SimulatorIO {
    public TextReader StdIn {get; private set;}
    public TextWriter StdOut {get; private set;}

    public SimulatorIO() {
        this.StdIn = Console.In;
        this.StdOut = Console.Out;
    }

    public SimulatorIO(TextReader @in, TextWriter @out) {
        this.StdIn = @in;
        this.StdOut = @out;
    }
}

/// <summary>
/// MIPS 32 simulator
/// </summary>
public class Simulator : ISimulator {
    protected Cpu cpu {get; private set;}
    protected Fpu fpu {get; private set;}
    protected IMemory memory {get; private set;}
    protected Fpu coprocessor0 => fpu;

    private SimulatorIO io {get; set;}

    public Simulator(SimulatorIO io, IMemory memory) {
        this.cpu = new Cpu();
        this.fpu = new Fpu();
        this.memory = memory;
        this.io = io;
    }
    public Simulator(IMemory memory) : this(new SimulatorIO(), memory) {}    

    public void Execute(IBytecodeInstruction instr) {
        instr.Invoke(cpu, fpu, memory, this.io);
    }

    public int Execute (Bytecode.BytecodeProgram instrs) {
        while (true) {
            if (cpu.PC < 0 || cpu.PC >= instrs.InstructionCount)
                break;
            var instr = instrs[(uint)cpu.PC];
            try {
                OnBeforeInstruction(instr);
                Execute(instr);
                OnAfterInstruction(instr);
                cpu.PC++;
            } catch (ExitException exit) {
                return exit.ExitCode;
            }
        }
        return 0;
    }

    public virtual void OnBeforeInstruction(IBytecodeInstruction instr) {}
    public virtual void OnAfterInstruction(IBytecodeInstruction instr) {}
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
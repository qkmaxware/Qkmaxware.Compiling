using System.Collections.Generic;
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips;

/// <summary>
/// MIPS 32 simulator with debugging and stepping functions
/// </summary>
public class StepableSimulator {
    public Cpu Cpu {get; private set;}
    public Fpu Fpu {get; private set;}
    public IMemory Memory {get; private set;}
    public Fpu Coprocessor1 => Fpu;

    private SimulatorIO io {get; set;}

    private Bytecode.BytecodeProgram program;

    public StepableSimulator(SimulatorIO io, IMemory memory, Bytecode.BytecodeProgram program) {
        this.Cpu = new Cpu();
        this.Fpu = new Fpu();
        this.Memory = memory;
        this.io = io;
        this.program = program;
    }
    public StepableSimulator(IMemory memory, Bytecode.BytecodeProgram program) : this(new SimulatorIO(), memory, program) {}    

    protected void Execute(Bytecode.IBytecodeInstruction instr) {
        instr.Invoke(Cpu, Fpu, Memory, this.io);
    }

    public bool IsProgramDone => program == null ? true : Cpu.PC < 0 || Cpu.PC >= program.InstructionCount;

    public Bytecode.IBytecodeInstruction? NextInstruction => IsProgramDone ? null : program[(uint)Cpu.PC];

    public int RunAll () {
        while (true) {
            if (IsProgramDone)
                break;
            
            try {
                StepNext();
            } catch (ExitException exit) {
                return exit.ExitCode;
            }
        }
        return 0;
    }

    private void StepNext() {
        if (IsProgramDone)
            return;
        var instr = program[(uint)Cpu.PC];
        
        OnBeforeInstruction(instr);
        Execute(instr);
        OnAfterInstruction(instr);

        Cpu.PC++;
    }

    public bool TryStepNext() {
        try {
            StepNext();
            return true;
        } catch (ExitException) {
            return false;
        }
    }

    public virtual void OnBeforeInstruction(Bytecode.IBytecodeInstruction instr) {}
    public virtual void OnAfterInstruction(Bytecode.IBytecodeInstruction instr) {}
}

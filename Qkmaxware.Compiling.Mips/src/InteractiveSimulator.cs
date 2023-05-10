using System.Collections.Generic;
using Qkmaxware.Compiling.Targets.Mips.Hardware;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips;

/// <summary>
/// Interactive MIPS 32 simulator allowing for stepping through instructions and viewing memory using the console
/// </summary>
public class ConsoleInteractiveSimulator : Simulator {

    public ConsoleInteractiveSimulator(SimulatorIO io, IMemory memory) : base(io, memory) {}
    public ConsoleInteractiveSimulator(IMemory memory) : base(memory) {}

    public override void OnBeforeInstruction(IBytecodeInstruction instr) {
        Console.WriteLine("CPU");
        Console.Write("    "); Console.WriteLine(cpu);
        Console.WriteLine("FPU");
        Console.Write("    "); Console.WriteLine(fpu);
        Console.WriteLine();
        Console.WriteLine("About to execute instruction " + instr);
        Console.WriteLine(">    Press ENTER to continue");
        Console.WriteLine(">    Type 'exit' and press ENTER to quit");

        var cmd = Console.ReadLine()?.ToLower();
        switch (cmd) {
            case "exit":
            case "quit":
            case "close":
            case "stop":
                throw new ExitException();
            default:
                break; // continue to instruction execution
        }
    }
}
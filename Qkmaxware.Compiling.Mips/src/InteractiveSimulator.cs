using System.Collections.Generic;

namespace Qkmaxware.Compiling.Mips;

/// <summary>
/// Interactive MIPS 32 simulator allowing for stepping through instructions and viewing memory
/// </summary>
public class InteractiveSimulator : Simulator {

    public InteractiveSimulator(IMemory memory) : base(memory) {}

    public override void OnBeforeInstruction(Bytecode.IBytecodeInstruction instr) {
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
        }
    }
}
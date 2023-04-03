namespace Qkmaxware.Compiling.Mips.Assembly;

public class Syscall : BaseAssemblyInstruction {
    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        // Behaviour changes depending on $v0
        var system_call = cpu.Registers.V0.Read();

        switch (system_call) {
            case 1: 
                // print_int $a0
                Console.Write(i32(cpu.Registers.A0));
                break;
            case 2:
                // print_float $f12
                Console.Write(f32(fpu.Registers[12]));
                break;
            case 3:
                goto default;
            case 4:
                // print_string $a0
                var addr = u32(cpu.Registers.A0);
                uint character = default(char);
                // until null termination, print each ascii character
                while ((character = memory.LoadWord(addr)) != '\0') {
                    Console.Write((char)character);
                    addr += 1;
                }
                break;
            case 5:
                // read_int $v0
                {
                    var input = Console.ReadLine();
                    int i;
                    if (int.TryParse(input, out i)) {
                        cpu.Registers.V0.Write(u32(i));
                    } else {
                        throw new ArgumentException($"Input '{input}' is not an integer");
                    }
                }
                break;
            case 6:
                // read_float $v0
                {
                    var input = Console.ReadLine();
                    float i;
                    if (float.TryParse(input, out i)) {
                        fpu.Registers[0].Write(i);
                    } else {
                        throw new ArgumentException($"Input '{input}' is not a float");
                    }
                }
                break;
            case 7:
                goto default;
            case 8:
                goto default;
            case 9:
                goto default;
            case 10:
                // exit
                throw new ExitException();
            case 11:
                // read_char $v0
                {
                    var input = Console.Read();
                    cpu.Registers.V0.Write(u32(input));
                }
                break;
            case 17:
                // exit2 (exit with integer return)
                throw new ExitException((int)cpu.Registers.A0.Read());
            default:
                throw new NotImplementedException($"System call {system_call} is not implemented.");
        }
    }
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
    public override string InstrName() => "syscall";
}
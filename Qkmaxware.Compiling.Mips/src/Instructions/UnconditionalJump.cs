namespace Qkmaxware.Compiling.Mips.InstructionSet;

public abstract class UnconditionalJump : BaseInstruction {
    protected void Goto (Cpu cpu, uint byteAddress) {
        cpu.PC = (int)byteAddress;
    }
}

public class JumpTo : UnconditionalJump {
    public uint Address;

    public override string InstrName() => "j";

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        Goto(cpu, this.Address);
    }

    public override string ToString() => $"{InstrName()} {Address}";
}

public class JumpRegister : UnconditionalJump {
    public RegisterIndex Register;

    public override string InstrName() => "jr";

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        Goto(cpu, cpu.Registers[Register].Read());
    }

    public override string ToString() => $"{InstrName()} {Register}";
}

public class JumpAndLink : UnconditionalJump {
    public uint Address;

    public override string InstrName() => "jal";

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        cpu.Registers.RA.Write(u32(cpu.PC * 4 + 4));
        Goto(cpu, Address);
    }

    public override string ToString() => $"{InstrName()} {Address}";
}
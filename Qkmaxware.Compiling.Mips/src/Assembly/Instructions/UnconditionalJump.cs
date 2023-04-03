using Qkmaxware.Compiling.Mips.Assembly;

namespace Qkmaxware.Compiling.Mips.Assembly;

public abstract class UnconditionalJump : BaseAssemblyInstruction {
    protected void Goto (Cpu cpu, uint byteAddress) {
        cpu.PC = (int)byteAddress;
    }
}

public class JumpTo : UnconditionalJump {
    public IAddressLike Address;

    public override string InstrName() => "j";

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        //Goto(cpu, this.Address);
    }
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string ToString() => $"{InstrName()} {Address}";
}

public class JumpRegister : UnconditionalJump {
    public RegisterIndex Register;

    public override string InstrName() => "jr";

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        //Goto(cpu, cpu.Registers[Register].Read());
    }
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string ToString() => $"{InstrName()} {Register}";
}

public class JumpAndLink : UnconditionalJump {
    public IAddressLike Address;

    public override string InstrName() => "jal";

    public override void Invoke(Cpu cpu, Fpu fpu, IMemory memory) {
        cpu.Registers.RA.Write(u32(cpu.PC * 4 + 4));
        //Goto(cpu, Address);
    }
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
    
    public override string ToString() => $"{InstrName()} {Address}";
}
using Qkmaxware.Compiling.Targets.Mips.Assembly;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

public abstract class UnconditionalJump : BaseAssemblyInstruction {}

public class JumpTo : UnconditionalJump {
    public AddressLikeToken Address;

    public override string InstrName() => "j";
    public override string InstrFormat() => InstrName() + " label";
    public override string InstrDescription() =>  "Jump to a given address in the program.";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string ToString() => $"{InstrName()} {Address}";
}

public class JumpRegister : UnconditionalJump {
    public RegisterIndex Register;

    public override string InstrName() => "jr";
    public override string InstrFormat() => InstrName() + " $rAddress";
    public override string InstrDescription() =>  "Jump to the address stored in register rAddress.";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string ToString() => $"{InstrName()} {Register}";
}

public class JumpAndLink : UnconditionalJump {
    public AddressLikeToken Address;

    public override string InstrName() => "jal";
    public override string InstrFormat() => InstrName() + " label";
    public override string InstrDescription() =>  "Jump to a given address in the program and store the return address in $ra.";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
    
    public override string ToString() => $"{InstrName()} {Address}";
}
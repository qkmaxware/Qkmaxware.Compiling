using Qkmaxware.Compiling.Mips.Assembly;

namespace Qkmaxware.Compiling.Mips.Assembly;

public abstract class UnconditionalJump : BaseAssemblyInstruction {}

public class JumpTo : UnconditionalJump {
    public AddressLikeToken Address;

    public override string InstrName() => "j";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string ToString() => $"{InstrName()} {Address}";
}

public class JumpRegister : UnconditionalJump {
    public RegisterIndex Register;

    public override string InstrName() => "jr";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);

    public override string ToString() => $"{InstrName()} {Register}";
}

public class JumpAndLink : UnconditionalJump {
    public AddressLikeToken Address;

    public override string InstrName() => "jal";
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
    
    public override string ToString() => $"{InstrName()} {Address}";
}
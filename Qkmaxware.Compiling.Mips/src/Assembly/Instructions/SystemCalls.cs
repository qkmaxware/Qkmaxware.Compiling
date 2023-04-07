namespace Qkmaxware.Compiling.Mips.Assembly;

public class Syscall : BaseAssemblyInstruction {
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
    public override string InstrName() => "syscall";
}
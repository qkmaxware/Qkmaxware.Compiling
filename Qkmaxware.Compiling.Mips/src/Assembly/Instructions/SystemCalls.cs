namespace Qkmaxware.Compiling.Mips.Assembly;

public class Syscall : BaseAssemblyInstruction {
    public override void Visit(IInstructionVisitor visitor) => visitor.Accept(this);
    public override T Visit<T>(IInstructionVisitor<T> visitor) => visitor.Accept(this);
    public override string InstrName() => "syscall";
    public override string InstrFormat() => InstrName();
    public override string InstrDescription() =>  "Invoke specific system defined operations depending on the value in register $v0.";
}
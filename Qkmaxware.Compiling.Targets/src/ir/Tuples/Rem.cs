namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// Remainder instruction
/// </summary>
public class Rem : BinaryOperatorTuple {
    public Rem(ValueOperand lhs, ValueOperand rhs, Declaration result) : base(lhs, rhs, result) { }
    /// <summary>
    /// Description of this instruction's operation
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Store the results of {LeftOperand} rem {RightOperand} in {Result}";
    /// <summary>
    /// Visit this instruction
    /// </summary>
    /// <param name="visitor">visitor</param>
    public override void Visit(ITupleVisitor visitor) => visitor.Accept(this);
    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public override string PrintString() => $"{Result.PrintString()} := {LeftOperand.PrintString()} rem {RightOperand.PrintString()}";
}
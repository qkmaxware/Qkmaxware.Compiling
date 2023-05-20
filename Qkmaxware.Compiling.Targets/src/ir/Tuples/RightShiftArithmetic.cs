namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// Arithmetic right shift instruction
/// </summary>
public class RightShiftArithmetic : BinaryOperatorTuple {
    public RightShiftArithmetic(ValueOperand lhs, ValueOperand rhs, Declaration result) : base(lhs, rhs, result) { }
    /// <summary>
    /// Description of this instruction's operation
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Store the results of {LeftOperand}>>>{RightOperand} in {Result}";
    /// <summary>
    /// Visit this instruction
    /// </summary>
    /// <param name="visitor">visitor</param>
    public override void Visit(ITupleVisitor visitor) => visitor.Accept(this);
    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public override string PrintString() => $"{Result.PrintString()} := {LeftOperand.PrintString()}>>>{RightOperand.PrintString()}";
}
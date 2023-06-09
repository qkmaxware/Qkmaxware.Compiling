namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// Equality comparison returning a number
/// </summary>
public class Equality : BinaryOperatorTuple {
    public Equality(ValueOperand lhs, ValueOperand rhs, Declaration result) : base(lhs, rhs, result) { }
    /// <summary>
    /// Description of this instruction's operation
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Store 1 in {Result} if {LeftOperand}=={RightOperand}, 0 otherwise";
    /// <summary>
    /// Visit this instruction
    /// </summary>
    /// <param name="visitor">visitor</param>
    public override void Visit(ITupleVisitor visitor) => visitor.Accept(this);
    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public override string PrintString() => $"{Result.PrintString()} := {LeftOperand.PrintString()}=={RightOperand.PrintString()} ? 1 : 0";
}

/// <summary>
/// Inequality comparison returning a number
/// </summary>
public class Inequality : BinaryOperatorTuple {
    public Inequality(ValueOperand lhs, ValueOperand rhs, Declaration result) : base(lhs, rhs, result) { }
    /// <summary>
    /// Description of this instruction's operation
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Store 1 in {Result} if {LeftOperand}!={RightOperand}, 0 otherwise";
    /// <summary>
    /// Visit this instruction
    /// </summary>
    /// <param name="visitor">visitor</param>
    public override void Visit(ITupleVisitor visitor) => visitor.Accept(this);
    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public override string PrintString() => $"{Result.PrintString()} := {LeftOperand.PrintString()}!={RightOperand.PrintString()} ? 1 : 0";
}
namespace Qkmaxware.Compiling.Targets.Ir;

/// <summary>
/// Less than comparison returning a number
/// </summary>
public class LessThan : BinaryOperatorTuple {
    public LessThan(ValueOperand lhs, ValueOperand rhs, Declaration result) : base(lhs, rhs, result) { }
    /// <summary>
    /// Description of this instruction's operation
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Store 1 in {Result} if {LeftOperand}<{RightOperand}, 0 otherwise";
    /// <summary>
    /// Visit this instruction
    /// </summary>
    /// <param name="visitor">visitor</param>
    public override void Visit(ITupleVisitor visitor) => visitor.Accept(this);
    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public override string RenderString() => $"{Indentation}{Result} := {LeftOperand}<{RightOperand} ? 1 : 0";
}

/// <summary>
/// Less than or equal to comparison returning a number
/// </summary>
public class LessThanEqualTo : BinaryOperatorTuple {
    public LessThanEqualTo(ValueOperand lhs, ValueOperand rhs, Declaration result) : base(lhs, rhs, result) { }
    /// <summary>
    /// Description of this instruction's operation
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Store 1 in {Result} if {LeftOperand}<={RightOperand}, 0 otherwise";
    /// <summary>
    /// Visit this instruction
    /// </summary>
    /// <param name="visitor">visitor</param>
    public override void Visit(ITupleVisitor visitor) => visitor.Accept(this);
    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public override string RenderString() => $"{Indentation}{Result} := {LeftOperand}<={RightOperand} ? 1 : 0";
}
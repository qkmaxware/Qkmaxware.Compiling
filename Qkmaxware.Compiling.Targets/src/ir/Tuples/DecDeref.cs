namespace Qkmaxware.Compiling.Targets.Ir;

/// <summary>
/// Decrement memory location
/// </summary>
public class DecDeref : UnaryOperatorTuple {
    public DecDeref(ValueOperand op, Declaration result) : base(op, result) { }
    /// <summary>
    /// Description of this instruction's operation
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Store the results of {Operand} - 1 in memory location {Result}";
    /// <summary>
    /// Visit this instruction
    /// </summary>
    /// <param name="visitor">visitor</param>
    public override void Visit(ITupleVisitor visitor) => visitor.Accept(this);
    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public override string RenderString() => $"{Indentation}*{Result} := {Operand} - 1";
}
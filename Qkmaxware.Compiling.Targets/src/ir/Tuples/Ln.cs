namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// Ln of a number
/// </summary>
public class Ln : BuiltinFunctionTuple {
    public Ln(ValueOperand op, Declaration result) : base(op, result) { }
    /// <summary>
    /// Description of this instruction's operation
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Store the results of LN({Operand}) in {Result}";
    /// <summary>
    /// Visit this instruction
    /// </summary>
    /// <param name="visitor">visitor</param>
    public override void Visit(ITupleVisitor visitor) => visitor.Accept(this);
    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public override string PrintString() => $"{Result.PrintString()} := LN({Operand.PrintString()})";
}
namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// Base class for unary operators
/// </summary>
public abstract class UnaryOperatorTuple : Tuple {
    /// <summary>
    /// Operand on the left hand side of the operator
    /// </summary>
    /// <value>value</value>
    public ValueOperand Operand {get; private set;}
    /// <summary>
    /// Variable the results are being stored in
    /// </summary>
    /// <value>value</value>
    public Declaration Result {get; private set;}
    /// <summary>
    /// Create a new binary operator with the given operands and registers
    /// </summary>
    /// <param name="operand">left hand operator</param>
    /// <param name="result">result variable</param>
    public UnaryOperatorTuple(ValueOperand operand, Declaration result) {
        this.Operand = operand;
        this.Result = result;
    }
    /// <summary>
    /// Print this tuple as a string
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()   => $"({this.GetType().Name},{Operand},{Result})";
}

/// <summary>
/// Logical Not instruction
/// </summary>
public class Not : UnaryOperatorTuple {
    public Not(ValueOperand op, Declaration result) : base(op, result) { }
    /// <summary>
    /// Description of this instruction's operation
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Store the results of !{Operand} in {Result}";
    /// <summary>
    /// Visit this instruction
    /// </summary>
    /// <param name="visitor">visitor</param>
    public override void Visit(ITupleVisitor visitor) => visitor.Accept(this);
    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public override string PrintString() => $"{Result.PrintString()} := !{Operand.PrintString()}";
}
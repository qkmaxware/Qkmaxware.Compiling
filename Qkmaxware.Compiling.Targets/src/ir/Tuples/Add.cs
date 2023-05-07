namespace Qkmaxware.Compiling.Targets.Ir;

/// <summary>
/// Base class for binary operators
/// </summary>
public abstract class BinaryOperatorTuple : Tuple {
    /// <summary>
    /// Operand on the left hand side of the operator
    /// </summary>
    /// <value>value</value>
    public ValueOperand LeftOperand {get; private set;}
    /// <summary>
    /// Operand on the right hand side of the operator
    /// </summary>
    /// <value>value</value>
    public ValueOperand RightOperand {get; private set;}
    /// <summary>
    /// Variable the results are being stored in
    /// </summary>
    /// <value>value</value>
    public Declaration Result {get; private set;}
    /// <summary>
    /// Create a new binary operator with the given operands and registers
    /// </summary>
    /// <param name="lhs">left hand operator</param>
    /// <param name="rhs">right hand operator</param>
    /// <param name="result">result variable</param>
    public BinaryOperatorTuple(ValueOperand lhs, ValueOperand rhs, Declaration result) {
        this.LeftOperand = lhs;
        this.RightOperand = rhs;
        this.Result = result;
    }
    /// <summary>
    /// Print this tuple as a string
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()   => $"({this.GetType().Name},{LeftOperand},{RightOperand},{Result})";
}

/// <summary>
/// Addition instruction
/// </summary>
public class Add : BinaryOperatorTuple {
    public Add(ValueOperand lhs, ValueOperand rhs, Declaration result) : base(lhs, rhs, result) { }
    /// <summary>
    /// Description of this instruction's operation
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Store the results of {LeftOperand}+{RightOperand} in {Result}";
    /// <summary>
    /// Visit this instruction
    /// </summary>
    /// <param name="visitor">visitor</param>
    public override void Visit(ITupleVisitor visitor) => visitor.Accept(this);
    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public override string RenderString() => $"{Indentation}{Result} := {LeftOperand}+{RightOperand}";
}
namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// Simple value copy
/// </summary>
public class Copy : Tuple {
    /// <summary>
    /// Operand to copy value from
    /// </summary>
    /// <value>variable</value>
    public ValueOperand From {get; private set;}
    /// <summary>
    /// Operand to copy value to
    /// </summary>
    /// <value>variable</value>
    public Declaration To {get; private set;}
    /// <summary>
    /// Description of this instruction's operation
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Simple copy from {From} to {To}";
    /// <summary>
    /// Create a copy instruction from one variable to another
    /// </summary>
    /// <param name="from">source variable</param>
    /// <param name="to">target variable</param>
    public Copy(Declaration to, ValueOperand from) {
        this.From = from;
        this.To = to;
    }
    /// <summary>
    /// Visit this instruction
    /// </summary>
    /// <param name="visitor">visitor</param>
    public override void Visit(ITupleVisitor visitor) => visitor.Accept(this);
    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public override string PrintString()        => $"{To.PrintString()} := {From.PrintString()}";
    /// <summary>
    /// Print this tuple as a string
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()   => $"({this.GetType().Name},{To},{From})";
}
namespace Qkmaxware.Compiling.Targets.Ir;

/// <summary>
/// Copy from memory
/// </summary>
public class CopyFromDeref : Tuple {
    /// <summary>
    /// Operand to copy value from
    /// </summary>
    /// <value>variable</value>
    public TupleOperand From {get; private set;}
    /// <summary>
    /// Operand to copy value to
    /// </summary>
    /// <value>variable</value>
    public Declaration To {get; private set;}
    /// <summary>
    /// Description of this instruction's operation
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Copy the contents of memory at address {From} into {To}";
    /// <summary>
    /// Create a copy instruction from one variable to another
    /// </summary>
    /// <param name="from">source variable</param>
    /// <param name="to">target variable</param>
    public CopyFromDeref(TupleOperand from, Declaration to) {
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
    public override string RenderString()        => $"{Indentation}{To} := *{From}";
    /// <summary>
    /// Print this tuple as a string
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()   => $"({this.GetType().Name},{From},{To})";
}
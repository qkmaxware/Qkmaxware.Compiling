namespace Qkmaxware.Compiling.Targets.Ir;

/// <summary>
/// Copy from memory
/// </summary>
public class CopyFromOffset : Tuple {
    /// <summary>
    /// Base value operand to copy value from
    /// </summary>
    /// <value>variable</value>
    public TupleOperand Base {get; private set;}
    /// <summary>
    /// Offset value operand to copy value from
    /// </summary>
    /// <value>value</value>
    public ValueOperand Offset {get; private set;}
    /// <summary>
    /// Operand to copy value to
    /// </summary>
    /// <value>variable</value>
    public Declaration To {get; private set;}
    /// <summary>
    /// Description of this instruction's operation
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Copy the contents of memory at address {Base}+{Offset} into {To}";
    /// <summary>
    /// Create a copy instruction from one variable to another
    /// </summary>
    /// <param name="base">source location base</param>
    /// <param name="offset">source location offset</param>
    /// <param name="to">target variable</param>
    public CopyFromOffset(TupleOperand @base, ValueOperand offset, Declaration to) {
        this.Base = @base;
        this.Offset = offset;
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
    public override string RenderString()        => $"{Indentation}{To} := *({Base}+{Offset})";
    /// <summary>
    /// Print this tuple as a string
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()   => $"({this.GetType().Name},{Base},{Offset},{To})";
}
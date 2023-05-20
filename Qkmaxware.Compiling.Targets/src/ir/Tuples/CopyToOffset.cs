namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// Copy to memory
/// </summary>
public class CopyToOffset : Tuple {
    /// <summary>
    /// Operand to copy value from
    /// </summary>
    /// <value>variable</value>
    public ValueOperand From {get; private set;}
    /// <summary>
    /// Operand to copy value to
    /// </summary>
    /// <value>variable</value>
    public TupleOperand Base {get; private set;}
    /// <summary>
    /// Offset value operand to copy value to
    /// </summary>
    /// <value>value</value>
    public ValueOperand Offset {get; private set;}
    /// <summary>
    /// Description of this instruction's operation
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Copy {From} into the memory at address {Base}+{Offset}";
    /// <summary>
    /// Create a copy instruction from one variable to another
    /// </summary>
    /// <param name="from">source value</param>
    /// <param name="base">source location base</param>
    /// <param name="offset">source location offset</param>
    public CopyToOffset(ValueOperand from, TupleOperand @base, Declaration offset) {
        this.From = from;
        this.Base = @base;
        this.Offset = offset;
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
    public override string PrintString()        => $"*({Base.PrintString()}+{Offset.PrintString()}) := {From.PrintString()}";
    /// <summary>
    /// Print this tuple as a string
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()   => $"({this.GetType().Name},{From},{Base},{Offset})";
}
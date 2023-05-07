namespace Qkmaxware.Compiling.Targets.Ir;

/// <summary>
/// Call a non-returning program
/// </summary>
public class CallProcedure : Tuple {
    /// <summary>
    /// Subprogram to be called
    /// </summary>
    /// <value>subprogram</value>
    public Subprogram Called {get; private set;}

    /// <summary>
    /// Arguments to pass to the subprogram
    /// </summary>
    /// <returns>arguments</returns>
    public List<TupleOperand> Arguments {get; private set;} = new List<TupleOperand>();

    /// <summary>
    /// Description of the tuple's function
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Call procedure (non-value returning function) {Called}";

    public CallProcedure(Subprogram called, params TupleOperand[] args) {
        this.Called = called;
        this.Arguments.AddRange(args);
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
    public override string RenderString()        => $"{Indentation}{Called}({string.Join(',', this.Arguments)})";
    /// <summary>
    /// Print this tuple as a string
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()   => $"({this.GetType().Name},{string.Join(',', this.Arguments)})";
}
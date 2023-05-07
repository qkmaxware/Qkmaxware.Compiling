namespace Qkmaxware.Compiling.Targets.Ir;

/// <summary>
/// An IR branching instruction
/// </summary>
public class ReturnFunction : IBranch {
    protected static readonly string Indentation = "    ";
    
    /// <summary>
    /// Variable whose value is returned
    /// </summary>
    /// <value>variable</value>
    public Declaration ReturnVariable {get; private set;}

    /// <summary>
    /// Description of the tuple's function
    /// </summary>
    /// <value>description</value>
    public string Description => $"Return {ReturnVariable} from this function";

    public ReturnFunction(Declaration returnVariable) {
        this.ReturnVariable = returnVariable;
    }

    /// <summary>
    /// Visit this tuple using the given visitor
    /// </summary>
    /// <param name="visitor">visitor object</param>
    public void Visit (ITupleVisitor visitor) => visitor.Accept(this);

    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public string RenderString() => $"{Indentation}return {ReturnVariable}";

    /// <summary>
    /// Print this tuple as a string
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()   => $"({this.GetType().Name},{ReturnVariable})";

    public IEnumerable<BasicBlock> NextBlocks() => Enumerable.Empty<BasicBlock>();
}

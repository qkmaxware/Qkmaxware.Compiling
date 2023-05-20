namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// An IR branching instruction
/// </summary>
public class Jump : IBranch {
    protected static readonly string Indentation = "    ";
    
    /// <summary>
    /// Block to jump to
    /// </summary>
    /// <value>block</value>
    public BasicBlock Goto {get; private set;}

    /// <summary>
    /// Description of the tuple's function
    /// </summary>
    /// <value>description</value>
    public string Description => $"Jump to another block";

    public Jump(BasicBlock @goto) {
        this.Goto = @goto;
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
    public string RenderString() => $"jump {Goto.GetHashCode()}";

    /// <summary>
    /// Print this tuple as a string
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()   => $"({this.GetType().Name},{Goto.GetHashCode()})";

    public IEnumerable<BasicBlock> NextBlocks() {
        yield return Goto;
    }
}

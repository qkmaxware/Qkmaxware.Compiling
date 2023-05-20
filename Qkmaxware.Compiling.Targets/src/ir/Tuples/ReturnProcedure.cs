namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// An IR branching instruction
/// </summary>
public class ReturnProcedure : IBranch {
    /// <summary>
    /// Description of the tuple's function
    /// </summary>
    /// <value>description</value>
    public string Description => $"Return from this procedure";

    public ReturnProcedure() {}

    /// <summary>
    /// Visit this tuple using the given visitor
    /// </summary>
    /// <param name="visitor">visitor object</param>
    public void Visit (ITupleVisitor visitor) => visitor.Accept(this);

    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public string RenderString() => $"return";

    /// <summary>
    /// Print this tuple as a string
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()   => $"({this.GetType().Name})";

    public IEnumerable<BasicBlock> NextBlocks() => Enumerable.Empty<BasicBlock>();
}

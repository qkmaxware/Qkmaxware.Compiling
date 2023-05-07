namespace Qkmaxware.Compiling.Targets.Ir;

/// <summary>
/// An IR exit instruction
/// </summary>
public class Exit : IBranch {
    protected static readonly string Indentation = "    ";

    /// <summary>
    /// Description of the tuple's function
    /// </summary>
    /// <value>description</value>
    public string Description => $"Exit program";

    public Exit() {}

    /// <summary>
    /// Visit this tuple using the given visitor
    /// </summary>
    /// <param name="visitor">visitor object</param>
    public void Visit (ITupleVisitor visitor) => visitor.Accept(this);

    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public string RenderString() => $"{Indentation}exit";

    /// <summary>
    /// Print this tuple as a string
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()   => $"({this.GetType().Name})";

    public IEnumerable<BasicBlock> NextBlocks() {
        yield break;
    }
}

namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// Interface representing a branch between basic blocks
/// </summary>
public interface IBranch {
    /// <summary>
    /// All potential blocks that can follow after executing the transition code
    /// </summary>
    /// <returns>list of basic blocks</returns>
    public IEnumerable<BasicBlock> NextBlocks();

    /// <summary>
    /// Visit this tuple using the given visitor
    /// </summary>
    /// <param name="visitor">visitor object</param>
    public abstract void Visit (ITupleVisitor visitor);
}
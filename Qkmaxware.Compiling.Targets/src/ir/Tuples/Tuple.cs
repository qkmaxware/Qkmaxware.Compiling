namespace Qkmaxware.Compiling.Targets.Ir;

/// <summary>
/// An IR tuple instruction
/// </summary>
public abstract class Tuple {
    protected static readonly string Indentation = "    ";
    
    /// <summary>
    /// Description of the tuple's function
    /// </summary>
    /// <value>description</value>
    public abstract string Description {get;}

    /// <summary>
    /// Visit this tuple using the given visitor
    /// </summary>
    /// <param name="visitor">visitor object</param>
    public abstract void Visit (ITupleVisitor visitor);

    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public abstract string RenderString();
}
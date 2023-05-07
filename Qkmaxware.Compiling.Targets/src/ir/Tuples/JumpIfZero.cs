namespace Qkmaxware.Compiling.Targets.Ir;

/// <summary>
/// An IR branching instruction
/// </summary>
public class JumpIfZero : IBranch {
    protected static readonly string Indentation = "    ";
    
    /// <summary>
    /// Variable to check the condition of
    /// </summary>
    /// <value>variable</value>
    public Declaration ConditionVariable {get; private set;}

    /// <summary>
    /// Block to jump to if the variable is 0
    /// </summary>
    /// <value>block</value>
    public BasicBlock GotoIfZero {get; private set;}

    /// <summary>
    /// Block to jump to if the variable is not 0
    /// </summary>
    /// <value>block</value>
    public BasicBlock GotoNotZero {get; private set;}

    /// <summary>
    /// Description of the tuple's function
    /// </summary>
    /// <value>description</value>
    public string Description => $"Jump to one block if a variable is 0 and another block if it is not";

    public JumpIfZero(Declaration condition, BasicBlock ifZero, BasicBlock notZero) {
        this.ConditionVariable = condition;
        this.GotoIfZero = ifZero;
        this.GotoNotZero = notZero;
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
    public string RenderString() => $"{Indentation}if {ConditionVariable} == 0 then jump {GotoIfZero.GetHashCode()} else jump {GotoNotZero.GetHashCode()}";

    /// <summary>
    /// Print this tuple as a string
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()   => $"({this.GetType().Name},{GotoIfZero.GetHashCode()}, {GotoNotZero.GetHashCode()})";

    public IEnumerable<BasicBlock> NextBlocks() {
        yield return GotoIfZero;
        yield return GotoNotZero;
    }
}

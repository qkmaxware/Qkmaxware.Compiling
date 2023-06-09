namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// Call a value returning program
/// </summary>
public class CallFunction : Tuple {
    /// <summary>
    /// Subprogram to be called
    /// </summary>
    /// <value>subprogram</value>
    public Subprogram Called {get; private set;}

    /// <summary>
    /// Arguments to pass to the subprogram
    /// </summary>
    /// <returns>arguments</returns>
    public List<Declaration> Arguments {get; private set;} = new List<Declaration>();

    /// <summary>
    /// The variable to store the results of the function into
    /// </summary>
    /// <value>declaration</value>
    public Declaration ReturnVariable {get; private set;}

    /// <summary>
    /// Description of the tuple's function
    /// </summary>
    /// <value>description</value>
    public override string Description => $"Call function (value-value returning function) {Called} and store the results in {ReturnVariable}";

    public CallFunction(Subprogram called, Declaration returns, params Declaration[] args) {
        this.Called = called;
        this.ReturnVariable = returns;
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
    public override string PrintString()        => $"{ReturnVariable.PrintString()} := {Called}({string.Join(',', this.Arguments.Select(x => x.PrintString()))})";
    /// <summary>
    /// Print this tuple as a string
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()   => $"({this.GetType().Name},{ReturnVariable},{string.Join(',', this.Arguments)})";
}
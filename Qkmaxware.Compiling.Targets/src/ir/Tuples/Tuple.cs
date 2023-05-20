namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// An IR tuple instruction
/// </summary>
public abstract class Tuple {
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
    public abstract string PrintString();
}

/// <summary>
/// Base class for buit-in unary functions
/// </summary>
public abstract class BuiltinFunctionTuple : Tuple {
    /// <summary>
    /// Operand on the left hand side of the operator
    /// </summary>
    /// <value>value</value>
    public ValueOperand Operand {get; private set;}
    /// <summary>
    /// Variable the results are being stored in
    /// </summary>
    /// <value>value</value>
    public Declaration Result {get; private set;}
    /// <summary>
    /// Create a new binary operator with the given operands and registers
    /// </summary>
    /// <param name="operand">left hand operator</param>
    /// <param name="result">result variable</param>
    public BuiltinFunctionTuple(ValueOperand operand, Declaration result) {
        this.Operand = operand;
        this.Result = result;
    }
    /// <summary>
    /// Print this tuple as a string
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()   => $"({this.GetType().Name},{Operand},{Result})";
}

/// <summary>
/// Base class for binary operators
/// </summary>
public abstract class BinaryOperatorTuple : Tuple {
    /// <summary>
    /// Operand on the left hand side of the operator
    /// </summary>
    /// <value>value</value>
    public ValueOperand LeftOperand {get; private set;}
    /// <summary>
    /// Operand on the right hand side of the operator
    /// </summary>
    /// <value>value</value>
    public ValueOperand RightOperand {get; private set;}
    /// <summary>
    /// Variable the results are being stored in
    /// </summary>
    /// <value>value</value>
    public Declaration Result {get; private set;}
    /// <summary>
    /// Create a new binary operator with the given operands and registers
    /// </summary>
    /// <param name="lhs">left hand operator</param>
    /// <param name="rhs">right hand operator</param>
    /// <param name="result">result variable</param>
    public BinaryOperatorTuple(ValueOperand lhs, ValueOperand rhs, Declaration result) {
        this.LeftOperand = lhs;
        this.RightOperand = rhs;
        this.Result = result;
    }
    /// <summary>
    /// Print this tuple as a string
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()   => $"({this.GetType().Name},{LeftOperand},{RightOperand},{Result})";
}
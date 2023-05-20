namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// A visitor pattern for IR tuples
/// </summary>
public interface ITupleVisitor {
    // public void Accept(Tuple tuple);
    public void Accept(Copy tuple);
    public void Accept(CopyFromDeref tuple);
    public void Accept(CopyToDeref tuple);
    public void Accept(CopyFromOffset tuple);
    public void Accept(CopyToOffset tuple);
    public void Accept(Add tuple);
    public void Accept(Sub tuple);
    public void Accept(Mul tuple);
    public void Accept(Div tuple);
    public void Accept(Mod tuple);
    public void Accept(Rem tuple);
    public void Accept(Pow tuple);
    public void Accept(LeftShift tuple);
    public void Accept(RightShiftLogical tuple);
    public void Accept(RightShiftArithmetic tuple);
    public void Accept(And tuple);
    public void Accept(Or tuple);
    public void Accept(Xor tuple);
    public void Accept(Not tuple);
    public void Accept(Negate tuple);
    public void Accept(Complement tuple);
    public void Accept(AbsoluteValue tuple);
    public void Accept(ATan tuple);
    public void Accept(Cos tuple);
    public void Accept(Sin tuple);
    public void Accept(Sqrt tuple);
    public void Accept(Ln tuple);
    public void Accept(Inc tuple);
    public void Accept(Dec tuple);
    public void Accept(IncDeref tuple);
    public void Accept(DecDeref tuple);
    public void Accept(LessThan tuple);
    public void Accept(LessThanEqualTo tuple);
    public void Accept(GreaterThan tuple);
    public void Accept(GreaterThanEqualTo tuple);
    public void Accept(Equality tuple);
    public void Accept(Inequality tuple);

    public void Accept(Exit tuple);
    public void Accept(Jump tuple);
    public void Accept(JumpIfZero tuple);
    public void Accept(JumpIfNotZero tuple);
    public void Accept(CallProcedure tuple);
    public void Accept(CallFunction tuple);
    public void Accept(ReturnProcedure tuple);
    public void Accept(ReturnFunction tuple);
}
using Qkmaxware.Compiling.Ir;
using Qkmaxware.Compiling.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Targets.C;

internal class CCodeWalker : BasicBlockWalker, ITupleVisitor {
    private string? ret_name;
    private TextWriter code;

    private CConversionMapping convert;

    public CCodeWalker(string? ret_name, TextWriter writer) {
        this.ret_name = ret_name;
        this.code = writer;
        convert = new CConversionMapping(writer);
    }

    private string generateLabel(BasicBlock block) {
        return $"block_{block.GetHashCode()}";
    }

    private void exit() {
        code.Write(CBackend.Tab);
        code.Write("exit(0);");
    }

    public override void Visit(BasicBlock block) {
        code.Write(generateLabel(block)); code.WriteLine(":");
        foreach (var instruction in block) {
            instruction.Visit(this);
        }
        if (block.Transition == null) {
            // End program
            exit();
        } else {
            block.Transition.Visit(this);
        }
    }

    public void Accept(Copy tuple) {
        code.Write(CBackend.Tab);
        code.Write('v'); code.Write(tuple.To.Name);
        code.Write(" = ");
        foreach (var step in TypeConversion.EnumerateConversions(from: tuple.From.TypeOf(), to: tuple.To.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write('v'); code.Write(tuple.From);
        code.Write(";");
    }
    public void Accept(CopyFromDeref tuple) {}
    public void Accept(CopyToDeref tuple) {}
    public void Accept(CopyFromOffset tuple) {}
    public void Accept(CopyToOffset tuple) {}

    private void Accept(BinaryOperatorTuple tuple, string @operator) {
        code.Write(CBackend.Tab);
        code.Write('v'); code.Write(tuple.Result.Name);
        code.Write(" = ");
        foreach (var step in TypeConversion.EnumerateConversions(from: tuple.LeftOperand.TypeOf(), to: tuple.Result.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write(tuple.LeftOperand);
        code.Write(' '); code.Write(@operator); code.Write(' ');
        foreach (var step in TypeConversion.EnumerateConversions(from: tuple.RightOperand.TypeOf(), to: tuple.Result.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write(tuple.RightOperand);
        code.WriteLine(";");
    }
    public void Accept(Add tuple) {
        Accept(tuple, "+");
    }
    public void Accept(Sub tuple) {
        Accept(tuple, "-");
    }
    public void Accept(Mul tuple) {
        Accept(tuple, "*");
    }
    public void Accept(Div tuple) {
        Accept(tuple, "/");
    }
    public void Accept(Mod tuple) {}
    public void Accept(Rem tuple) {
        Accept(tuple, "%");
    }
    public void Accept(Pow tuple) {
        code.Write(CBackend.Tab);
        code.Write('v'); code.Write(tuple.Result.Name);
        code.Write(" = ");
        foreach (var step in TypeConversion.EnumerateConversions(from: IrType.F32, to: tuple.Result.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write("pow(");
        code.Write(tuple.LeftOperand);
        code.Write(", ");
        code.Write(tuple.RightOperand);
        code.WriteLine(");");
    }
    public void Accept(LeftShift tuple) {
        Accept(tuple, "<<");
    }
    public void Accept(RightShiftLogical tuple) {
        Accept(tuple, ">>");
    }
    public void Accept(RightShiftArithmetic tuple) {
        Accept(tuple, ">>");
    }
    public void Accept(And tuple) {
        Accept(tuple, "&");
    }
    public void Accept(Or tuple) {
        Accept(tuple, "|");
    }
    public void Accept(Xor tuple) {
        Accept(tuple, "^");
    }
    public void Accept(Not tuple) {
        code.Write(CBackend.Tab);
        code.Write('v'); code.Write(tuple.Result.Name);
        code.Write(" = ");
        foreach (var step in TypeConversion.EnumerateConversions(from: IrType.F32, to: tuple.Result.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write("(!");
        code.Write(tuple.Operand);
        code.WriteLine(");");
    }
    public void Accept(Negate tuple) {
        code.Write(CBackend.Tab);
        code.Write('v'); code.Write(tuple.Result.Name);
        code.Write(" = ");
        foreach (var step in TypeConversion.EnumerateConversions(from: IrType.F32, to: tuple.Result.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write("(-");
        code.Write(tuple.Operand);
        code.WriteLine(");");
    }
    public void Accept(Complement tuple) {
        code.Write(CBackend.Tab);
        code.Write('v'); code.Write(tuple.Result.Name);
        code.Write(" = ");
        foreach (var step in TypeConversion.EnumerateConversions(from: IrType.F32, to: tuple.Result.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write("(~");
        code.Write(tuple.Operand);
        code.WriteLine(");");
    }
    public void Accept(AbsoluteValue tuple) {
        code.Write(CBackend.Tab);
        code.Write('v'); code.Write(tuple.Result.Name);
        code.Write(" = ");
        foreach (var step in TypeConversion.EnumerateConversions(from: IrType.F32, to: tuple.Result.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write("fabs(");
        code.Write(tuple.Operand);
        code.WriteLine(");");
    }
    public void Accept(ATan tuple) {
        code.Write(CBackend.Tab);
        code.Write('v'); code.Write(tuple.Result.Name);
        code.Write(" = ");
        foreach (var step in TypeConversion.EnumerateConversions(from: IrType.F32, to: tuple.Result.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write("atan(");
        code.Write(tuple.Operand);
        code.WriteLine(");");
    }
    public void Accept(Cos tuple) {
        code.Write(CBackend.Tab);
        code.Write('v'); code.Write(tuple.Result.Name);
        code.Write(" = ");
        foreach (var step in TypeConversion.EnumerateConversions(from: IrType.F32, to: tuple.Result.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write("cos(");
        code.Write(tuple.Operand);
        code.WriteLine(");");
    }
    public void Accept(Sin tuple) {
        code.Write(CBackend.Tab);
        code.Write('v'); code.Write(tuple.Result.Name);
        code.Write(" = ");
        foreach (var step in TypeConversion.EnumerateConversions(from: IrType.F32, to: tuple.Result.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write("sin(");
        code.Write(tuple.Operand);
        code.WriteLine(");");
    }
    public void Accept(Sqrt tuple) {
        code.Write(CBackend.Tab);
        code.Write('v'); code.Write(tuple.Result.Name);
        code.Write(" = ");
        foreach (var step in TypeConversion.EnumerateConversions(from: IrType.F32, to: tuple.Result.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write("sqrt(");
        code.Write(tuple.Operand);
        code.WriteLine(");");
    }
    public void Accept(Ln tuple) {
        code.Write(CBackend.Tab);
        code.Write('v'); code.Write(tuple.Result.Name);
        code.Write(" = ");
        foreach (var step in TypeConversion.EnumerateConversions(from: IrType.F32, to: tuple.Result.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write("log(");
        code.Write(tuple.Operand);
        code.WriteLine(");");
    }
    public void Accept(Inc tuple) {
        code.Write(CBackend.Tab);
        code.Write('v'); code.Write(tuple.Result.Name);
        code.Write(" = ");
        foreach (var step in TypeConversion.EnumerateConversions(from: IrType.F32, to: tuple.Result.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write("(");
        code.Write(tuple.Operand);
        code.WriteLine(" + 1);");
    }
    public void Accept(Dec tuple) {
        code.Write(CBackend.Tab);
        code.Write('v'); code.Write(tuple.Result.Name);
        code.Write(" = ");
        foreach (var step in TypeConversion.EnumerateConversions(from: IrType.F32, to: tuple.Result.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write("(");
        code.Write(tuple.Operand);
        code.WriteLine(" - 1);");
    }
    public void Accept(IncDeref tuple) {}
    public void Accept(DecDeref tuple) {}
    public void Accept(LessThan tuple) {
        Accept(tuple, "<");
    }
    public void Accept(LessThanEqualTo tuple) {
        Accept(tuple, "<=");
    }
    public void Accept(GreaterThan tuple) {
        Accept(tuple, ">");
    }
    public void Accept(GreaterThanEqualTo tuple) {
        Accept(tuple, ">=");
    }
    public void Accept(Equality tuple) {
        Accept(tuple, "==");
    }
    public void Accept(Inequality tuple) {
        Accept(tuple, "!=");
    }

    public void Accept(Exit tuple) {
        exit();
    }
    public void Accept(Jump tuple) {
        code.Write(CBackend.Tab);
        code.Write("goto "); 
        code.Write(generateLabel(tuple.Goto));
        code.WriteLine(';');
    }
    public void Accept(JumpIfZero tuple) {
        code.Write(CBackend.Tab);
        code.Write("if (v");
        code.Write(tuple.ConditionVariable.Name);
        code.Write(" == 0){");
        {
            code.Write("goto ");
            code.Write(generateLabel(tuple.GotoIfZero));
            code.Write(';');
        }
        code.WriteLine('}');
        code.Write(CBackend.Tab);
        code.Write("else {");
        {
            code.Write("goto ");
            code.Write(generateLabel(tuple.GotoNotZero));
            code.Write(';');
        }
        code.WriteLine('}');
    }
    public void Accept(JumpIfNotZero tuple) {
        code.Write(CBackend.Tab);
        code.Write("if (v");
        code.Write(tuple.ConditionVariable.Name);
        code.Write(" != 0){");
        {
            code.Write("goto ");
            code.Write(generateLabel(tuple.GotoNotZero));
            code.Write(';');
        }
        code.WriteLine('}');
        code.Write(CBackend.Tab);
        code.Write("else {");
        {
            code.Write("goto ");
            code.Write(generateLabel(tuple.GotoIfZero));
            code.Write(';');
        }
        code.WriteLine('}');
    }
    private string getVarName(Declaration decl) {
        if (decl is Global g) {
            return $"global_{g.VariableIndex}";
        } else if (decl is Local l) {
            return $"v{l.Name}";
        } else {
            throw new ArgumentException("Unknown declaration type " + decl.GetType());
        }
    }
    public void Accept(CallProcedure tuple) {
        code.Write(CBackend.Tab);
        code.Write(CModuleVisitor.GenerateFuncName(tuple.Called));
        code.Write('(');
        bool first = true;
        foreach (var arg in tuple.Arguments) {
            if (!first)
                code.Write(", ");
            code.Write(getVarName(arg));
            first = false;
        }
        code.WriteLine(");");
    }
    public void Accept(CallFunction tuple) {
        code.Write(CBackend.Tab);
        code.Write('v'); code.Write(tuple.ReturnVariable.Name);
        code.Write(" = ");
        foreach (var step in TypeConversion.EnumerateConversions(from: tuple.Called.ReturnLocal?.TypeOf() ?? IrType.I32, to: tuple.ReturnVariable.TypeOf()))
            step.GenerateInstructions(this.convert);
        code.Write(CModuleVisitor.GenerateFuncName(tuple.Called));
        code.Write('(');
        bool first = true;
        foreach (var arg in tuple.Arguments) {
            if (!first)
                code.Write(", ");
            code.Write(getVarName(arg));
            first = false;
        }
        code.WriteLine(");");
    }
    public void Accept(ReturnProcedure tuple) {
        code.Write(CBackend.Tab);
        code.WriteLine("return;"); 
    }
    public void Accept(ReturnFunction tuple) {
        code.Write(CBackend.Tab);
        code.Write("return "); 
        code.Write(this.ret_name); 
        code.WriteLine(';'); 
    }
}
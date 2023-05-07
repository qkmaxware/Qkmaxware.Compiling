using Qkmaxware.Compiling.Targets.Ir;
using Qkmaxware.Compiling.Targets.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Targets.Mips.Assembly;

namespace Qkmaxware.Compiling.Targets.Mips;

internal class MipsAssemblyCodeWalker : BasicBlockWalker, ITupleVisitor {
    private TextSection text;

    public MipsAssemblyCodeWalker(TextSection text) {
        this.text = text;
    }

    private void exit() {
        text.Code.Add(new LoadImmediate {
            ResultRegister = RegisterIndex.V0,
            Constant = 10
        });
        text.Code.Add(new Syscall());
    }

    public override void Visit(BasicBlock block) {
        text.Code.Add(new LabelMarker($"block_{block.GetHashCode()}"));
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

    public void Accept(Copy tuple) 
    {
        if (tuple.From is not IMipsValueOperand from) {
            throw new NotImplementedException($"Cannot copy value from '{tuple.From.GetType()}'");
        }
        // Load value
        text.Code.AddRange(from.MipsInstructionsToLoadValueInto(RegisterIndex.T0));
        // Store value
        text.Code.Add(new StoreWord {
            SourceRegister = RegisterIndex.T0,
            BaseRegister = RegisterIndex.FP,
            Offset = tuple.To.VariableIndex * 4,
        });
    }

    public void Accept(CopyFromDeref tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(CopyToDeref tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(CopyFromOffset tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(CopyToOffset tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Add tuple)
    {
        var result = tuple.Result;                          // Is a variable
        if (tuple.LeftOperand is not IMipsValueOperand lhs) // Can be a literal or a variable
        {
            throw new NotImplementedException("Operand types must be supported by the MIPS ISA");
        } 
        if (tuple.RightOperand is not IMipsValueOperand rhs) // Can be a literal or a variable
        {
            throw new NotImplementedException("Operand types must be supported by the MIPS ISA");
        } 
        
        var res_register = RegisterIndex.T4;
        var lhs_register = RegisterIndex.T0;
        var rhs_register = RegisterIndex.T2;

        // Step 1: Load values into registers/stack
        this.text.Code.AddRange(lhs.MipsInstructionsToLoadValueInto(lhs_register)); 
        this.text.Code.AddRange(rhs.MipsInstructionsToLoadValueInto(rhs_register));

        // Step 2: Convert to common type (preserve stack/registers)
        var lhs_conversions = Ir.TypeSystem.TypeConversion.EnumerateConversions(from: tuple.LeftOperand.TypeOf(), to: result.TypeOf());
        var lhs_converter = new MipsAssemblyConversions(this.text, lhs_register);
        foreach (var conversion in lhs_conversions) {
            conversion.GenerateInstructions(lhs_converter);
        }
        var rhs_conversions = Ir.TypeSystem.TypeConversion.EnumerateConversions(from: tuple.RightOperand.TypeOf(), to: result.TypeOf());
        var rhs_converter = new MipsAssemblyConversions(this.text, rhs_register);
        foreach (var conversion in rhs_conversions) {
            conversion.GenerateInstructions(rhs_converter);
        }

        // Step 3: Do operation
        

        // Step 4: Push result onto stack/registers
        
        
        // Step 5: Store the result in the variable

        throw new NotImplementedException();
    }

    public void Accept(Sub tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Mul tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Div tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Mod tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Rem tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Pow tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(LeftShift tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(RightShiftLogical tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(RightShiftArithmetic tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Ir.And tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Ir.Or tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Ir.Xor tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Not tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Negate tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Complement tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(AbsoluteValue tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(ATan tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Cos tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Sin tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Sqrt tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Ln tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Inc tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Dec tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(IncDeref tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(DecDeref tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(LessThan tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(LessThanEqualTo tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(GreaterThan tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(GreaterThanEqualTo tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Equality tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Inequality tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Exit tuple) {
        exit();
    }

    public void Accept(Jump tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(JumpIfZero tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(JumpIfNotZero tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(CallProcedure tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(CallFunction tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(ReturnProcedure tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(ReturnFunction tuple)
    {
        throw new NotImplementedException();
    }
}
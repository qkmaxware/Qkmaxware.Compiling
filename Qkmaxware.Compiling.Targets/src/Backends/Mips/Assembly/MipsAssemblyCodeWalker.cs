using Qkmaxware.Compiling.Targets.Ir;
using Qkmaxware.Compiling.Targets.Mips.Assembly;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

internal class MipsAssemblyCodeWalker : BasicBlockWalker, ITupleVisitor {
    private TextSection text;

    public MipsAssemblyCodeWalker(TextSection text) {
        this.text = text;
    }

    private string generateLabel(BasicBlock block) {
        return $"block_{block.GetHashCode()}";
    }

    private void exit() {
        text.Code.Add(new Li {
            Destination = RegisterIndex.V0,
            Value = 10                      // 10 is the exit syscall in my simulator 
        });
        text.Code.Add(new Syscall());
    }

    public override void Visit(BasicBlock block) {
        text.Code.Add(new Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions.Label(generateLabel(block)));
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
        text.Code.AddRange(tuple.To.MipsInstructionsToStoreValue(RegisterIndex.T0));
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

    private RegisterIndex UnaryResultRegister = RegisterIndex.T4; 
    private RegisterIndex UnaryOperandRegister = RegisterIndex.T0; 

    private RegisterIndex BinaryResultRegister = RegisterIndex.T4;
    private RegisterIndex BinaryLhsOperatorRegister = RegisterIndex.T0;
    private RegisterIndex BinaryRhsOperatorRegister = RegisterIndex.T2;

    private void load(ValueOperand raw_arg, out IMipsValueOperand arg) {
        if (raw_arg is not IMipsValueOperand) // Can be a literal or a variable
        {
            throw new NotImplementedException("Operand types must be supported by the MIPS ISA");
        } 
        arg = (IMipsValueOperand)raw_arg;

        // Step 1: Load values into registers/stack
        this.text.Code.AddRange(arg.MipsInstructionsToLoadValueInto(UnaryOperandRegister)); 
    }

    private void load(ValueOperand lhs_arg, ValueOperand rhs_arg, out IMipsValueOperand lhs, out IMipsValueOperand rhs) {
        if (lhs_arg is not IMipsValueOperand) // Can be a literal or a variable
        {
            throw new NotImplementedException("Operand types must be supported by the MIPS ISA");
        } 
        if (rhs_arg is not IMipsValueOperand) // Can be a literal or a variable
        {
            throw new NotImplementedException("Operand types must be supported by the MIPS ISA");
        } 
        lhs = (IMipsValueOperand)lhs_arg;
        rhs = (IMipsValueOperand)rhs_arg;

        // Step 1: Load values into registers/stack
        this.text.Code.AddRange(lhs.MipsInstructionsToLoadValueInto(BinaryLhsOperatorRegister)); 
        this.text.Code.AddRange(rhs.MipsInstructionsToLoadValueInto(BinaryRhsOperatorRegister));
    }

    private void accept(Declaration result, ValueOperand lhs_arg, ValueOperand rhs_arg, BinaryOperator operation) {
        // Step 1: Load values into registers/stack
        IMipsValueOperand lhs; IMipsValueOperand rhs;
        load(lhs_arg, rhs_arg, out lhs, out rhs);

        // Step 2: Convert to common type (preserve stack/registers)
        var lhs_conversions = Ir.TypeSystem.TypeConversion.EnumerateConversions(from: lhs_arg.TypeOf(), to: result.TypeOf());
        var lhs_converter = new MipsAssemblyConversions(this.text, BinaryLhsOperatorRegister);
        foreach (var conversion in lhs_conversions) {
            conversion.GenerateInstructions(lhs_converter);
        }
        var rhs_conversions = Ir.TypeSystem.TypeConversion.EnumerateConversions(from: rhs_arg.TypeOf(), to: result.TypeOf());
        var rhs_converter = new MipsAssemblyConversions(this.text, BinaryRhsOperatorRegister);
        foreach (var conversion in rhs_conversions) {
            conversion.GenerateInstructions(rhs_converter);
        }

        // Step 3: Do operation
        result.TypeOf().Visit(operation);

        // Step 5: Store the result in the variable
        result.MipsInstructionsToStoreValue(BinaryResultRegister);
    }

    public void Accept(Ir.Add tuple) {
        accept(tuple.Result, tuple.LeftOperand, tuple.RightOperand, new AddOperator(this.text, BinaryResultRegister, BinaryLhsOperatorRegister, BinaryRhsOperatorRegister));
    }

    public void Accept(Ir.Sub tuple) {
        accept(tuple.Result, tuple.LeftOperand, tuple.RightOperand, new SubOperator(this.text, BinaryResultRegister, BinaryLhsOperatorRegister, BinaryRhsOperatorRegister));
    }

    public void Accept(Mul tuple) {
        accept(tuple.Result, tuple.LeftOperand, tuple.RightOperand, new MulOperator(this.text, BinaryResultRegister, BinaryLhsOperatorRegister, BinaryRhsOperatorRegister));
    }

    public void Accept(Ir.Div tuple) {
        accept(tuple.Result, tuple.LeftOperand, tuple.RightOperand, new DivOperator(this.text, BinaryResultRegister, BinaryLhsOperatorRegister, BinaryRhsOperatorRegister));
    }

    public void Accept(Mod tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Rem tuple) {
        accept(tuple.Result, tuple.LeftOperand, tuple.RightOperand, new RemOperator(this.text, BinaryResultRegister, BinaryLhsOperatorRegister, BinaryRhsOperatorRegister));
    }

    public void Accept(Pow tuple) 
    {
        throw new NotImplementedException();
    }

    public void Accept(LeftShift tuple) {
        IMipsValueOperand lhs; IMipsValueOperand rhs;
        load(tuple.LeftOperand, tuple.RightOperand, out lhs, out rhs);
        text.Code.Add(new Sllv {
            Destination = BinaryResultRegister,
            LhsOperand = BinaryLhsOperatorRegister,
            RhsOperand = BinaryRhsOperatorRegister
        });
        tuple.Result.MipsInstructionsToStoreValue(BinaryResultRegister);
    }

    public void Accept(RightShiftLogical tuple) {
        IMipsValueOperand lhs; IMipsValueOperand rhs;
        load(tuple.LeftOperand, tuple.RightOperand, out lhs, out rhs);
        text.Code.Add(new Srlv {
            Destination = BinaryResultRegister,
            LhsOperand = BinaryLhsOperatorRegister,
            RhsOperand = BinaryRhsOperatorRegister
        });
        tuple.Result.MipsInstructionsToStoreValue(BinaryResultRegister);
    }

    public void Accept(RightShiftArithmetic tuple) {
        IMipsValueOperand lhs; IMipsValueOperand rhs;
        load(tuple.LeftOperand, tuple.RightOperand, out lhs, out rhs);
        // Determine if highest bit is 1
        text.Code.Add(new Andi {
            Destination = RegisterIndex.At,
            LhsOperand = BinaryLhsOperatorRegister,
            Immediate = 0b10000000_00000000_00000000_00000000U // 1 in the highest bit
        });
        text.Code.Add(new Srlv {
            Destination = BinaryResultRegister,
            LhsOperand = BinaryLhsOperatorRegister,
            RhsOperand = BinaryRhsOperatorRegister
        });
        // Preserve Highest bit (if it was set)
        text.Code.Add(new Bytecode.Instructions.Or {
            Destination = BinaryResultRegister,
            LhsOperand = BinaryResultRegister,
            RhsOperand = RegisterIndex.At
        });
        tuple.Result.MipsInstructionsToStoreValue(BinaryResultRegister);
    }

    public void Accept(Ir.And tuple) {
        IMipsValueOperand lhs; IMipsValueOperand rhs;
        load(tuple.LeftOperand, tuple.RightOperand, out lhs, out rhs);
        text.Code.Add(new Bytecode.Instructions.And {
            Destination = BinaryResultRegister,
            LhsOperand = BinaryLhsOperatorRegister,
            RhsOperand = BinaryRhsOperatorRegister
        });
        tuple.Result.MipsInstructionsToStoreValue(BinaryResultRegister);
    }

    public void Accept(Ir.Or tuple) {
        IMipsValueOperand lhs; IMipsValueOperand rhs;
        load(tuple.LeftOperand, tuple.RightOperand, out lhs, out rhs);
        text.Code.Add(new Bytecode.Instructions.Or {
            Destination = BinaryResultRegister,
            LhsOperand = BinaryLhsOperatorRegister,
            RhsOperand = BinaryRhsOperatorRegister
        });
        tuple.Result.MipsInstructionsToStoreValue(BinaryResultRegister);
    }

    public void Accept(Ir.Xor tuple) {
        IMipsValueOperand lhs; IMipsValueOperand rhs;
        load(tuple.LeftOperand, tuple.RightOperand, out lhs, out rhs);
        text.Code.Add(new Bytecode.Instructions.Xor {
            Destination = BinaryResultRegister,
            LhsOperand = BinaryLhsOperatorRegister,
            RhsOperand = BinaryRhsOperatorRegister
        });
        tuple.Result.MipsInstructionsToStoreValue(BinaryResultRegister);
    }

    public void Accept(Not tuple) {
        IMipsValueOperand arg;
        load(tuple.Operand, out arg);
        // set not(arg) = 0         -- Assume arg is TRUE set !arg to FALSE
        // If arg > 0 {             -- IE is arg TRUE
        //                          -- --  Do nothing
        // } else {                 -- arg is FALSE
        //     set not(arg) = 1     -- -- Set !arg to TRUE
        // }
        text.Code.Add(new Move {
            Destination = UnaryResultRegister,
            Source = RegisterIndex.Zero
        });
        text.Code.Add(new Bgtz {
            Source = UnaryOperandRegister,
            AddressOffset = 16 // Skip 2 instructions (IE skip the LI instruction below (which is 2 instructions when expanded))
        });
        text.Code.Add (new Li {
            Destination = UnaryResultRegister,
            Value = 1.ReinterpretUint()
        });
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

    private void taylorExpansionAtan(RegisterIndex y, RegisterIndex x, RegisterIndex cpuTemp, RegisterIndex fpuTemp, RegisterIndex numerator, RegisterIndex denominator, int order = 5) {
        // atan(x) ~= SUM{k = 0->order} ( ((-1)^k * x^(1 + 2*k)) / (1 + 2*k)  )
        for (var k = 0; k < order; k++) {
            // All the numerator in the summation
            {
                var multiplier = MathF.Pow(-1.0f, k);
                if (multiplier == 0) {
                    continue;
                }
                // Compute numerator as x^ipow using repeated multiplications
                var ipow = 1 + 2 * k; // Number of times to multiply x
                text.Code.Add(new Li {
                    Destination = cpuTemp,
                    Value = (1.0f).ReinterpretUint()
                });
                text.Code.Add(new Mtc1{
                    FpuRegister = numerator,
                    CpuRegister = cpuTemp       // Set the inital numerator to 1 before repeated multiplications of x
                });
                for (var exp = 0; exp < ipow; exp++) {
                    text.Code.Add(new MulS {
                        Destination = numerator,
                        LhsOperand = numerator, 
                        RhsOperand = x
                    });
                }
                // Multiply numerator by multiplier
                text.Code.Add(new Li {
                    Destination = cpuTemp,
                    Value = multiplier.ReinterpretUint()
                });
                text.Code.Add(new Mtc1{
                    FpuRegister = fpuTemp,
                    CpuRegister = cpuTemp
                });
                text.Code.Add(new MulS {
                    Destination = numerator,
                    LhsOperand = fpuTemp, 
                    RhsOperand = numerator
                });
            }
            // All the denominator in the summation
            {
                var factorial = (float)(1 + 2 * k);
                text.Code.Add(new Li {
                    Destination = cpuTemp,
                    Value = factorial.ReinterpretUint()
                });
                text.Code.Add(new Mtc1{
                    FpuRegister = denominator,
                    CpuRegister = cpuTemp
                });
            }
            // Final division (next_term = numerator/denominator)
            text.Code.Add(new DivS {
                Destination = fpuTemp,
                LhsOperand = numerator,
                RhsOperand = denominator,
            });
            // Summation (y = y + next_term)
            text.Code.Add(new AddS {
                Destination = y,
                LhsOperand = y,
                RhsOperand = fpuTemp
            });
        }
    }

    public void Accept(ATan tuple)
    {
        // Load values
        // Convert to F32 (copy to FPU)
        // Note, horizonal asymptotes at y = +-pi/2 so we can just set it to that if we lose precision on the talor expansion (-1 to 1 currently?)
        // Do atan (no native FPU instruction) // -- https://www.wolframalpha.com/input?i=taylor+expansion+of+atan%28x%29
                                               // -- https://www.wolframalpha.com/input?i=plot+x+-+x%5E3%2F3+%2B+x%5E5%2F5+-+x%5E7%2F7+%2B+x%5E9%2F9+-+x%5E11%2F11+%2C+atan%28x%29%2C+x+-+x%5E3%2F3+%2B+x%5E5%2F5+-+x%5E7%2F7+%2B+x%5E9%2F9+-+x%5E11%2F11+%2B+x%5E13%2F13+-+x%5E15%2F15+%2B+x%5E17%2F17+-+x%5E19%2F19+
                                               // -- https://www.wolframalpha.com/input?i=plot+y+%3D+atan%28x%29%2C+y+%3D+-pi%2F2%2C+y+%3D+pi%2F2+between+-30+and+30
        taylorExpansionAtan(
            y:          new RegisterIndex(2),   // result stored in y
            x:          new RegisterIndex(8),   // argument stored and untouched in x
            cpuTemp:    RegisterIndex.At,       // temp register to copy CPU values into
            fpuTemp:    new RegisterIndex(10),  // temp register to copy FPU values into
            numerator:  new RegisterIndex(4),   // temp register to partially compute the numerator of the current expansion term
            denominator:new RegisterIndex(6)    // temp register to partially compute the denominator of the current expansion term
        );
        // Convert back to result type 
        // Copy to result register
        throw new NotImplementedException();
    }

    private void taylorExpansionCos(RegisterIndex y, RegisterIndex x, RegisterIndex cpuTemp, RegisterIndex fpuTemp, RegisterIndex numerator, RegisterIndex denominator, int order = 5) {
        // cos(x) ~= SUM{k = 0->order} ( ((-1)^k * x^(2*k)) / (2*k)!  )
        for (var k = 0; k < order; k++) {
            // All the numerator in the summation
            {
                var multiplier = MathF.Pow(-1.0f, k);
                if (multiplier == 0) {
                    continue;
                }
                // Compute numerator as x^ipow using repeated multiplications
                var ipow = 2 * k; // Number of times to multiply x
                text.Code.Add(new Li {
                    Destination = cpuTemp,
                    Value = (1.0f).ReinterpretUint()
                });
                text.Code.Add(new Mtc1{
                    FpuRegister = numerator,
                    CpuRegister = cpuTemp       // Set the inital numerator to 1 before repeated multiplications of x
                });
                for (var exp = 0; exp < ipow; exp++) {
                    text.Code.Add(new MulS {
                        Destination = numerator,
                        LhsOperand = numerator, 
                        RhsOperand = x
                    });
                }
                // Multiply numerator by multiplier
                text.Code.Add(new Li {
                    Destination = cpuTemp,
                    Value = multiplier.ReinterpretUint()
                });
                text.Code.Add(new Mtc1{
                    FpuRegister = fpuTemp,
                    CpuRegister = cpuTemp
                });
                text.Code.Add(new MulS {
                    Destination = numerator,
                    LhsOperand = fpuTemp, 
                    RhsOperand = numerator
                });
            }
            // All the denominator in the summation
            {
                var factorial = (float)this.factorial(2 * k);
                text.Code.Add(new Li {
                    Destination = cpuTemp,
                    Value = factorial.ReinterpretUint()
                });
                text.Code.Add(new Mtc1{
                    FpuRegister = denominator,
                    CpuRegister = cpuTemp
                });
            }
            // Final division (next_term = numerator/denominator)
            text.Code.Add(new DivS {
                Destination = fpuTemp,
                LhsOperand = numerator,
                RhsOperand = denominator,
            });
            // Summation (y = y + next_term)
            text.Code.Add(new AddS {
                Destination = y,
                LhsOperand = y,
                RhsOperand = fpuTemp
            });
        }
    }

    /*
    public static float Repeat(float t, float length) {
        // Mips does have a floor
        return Clamp(t - Mathf.Floor(t / length) * length, 0.0f, length);
    }
    */
    private void clamp(RegisterIndex value, RegisterIndex min, RegisterIndex max) {
        
    }
    private void repeatFloat(RegisterIndex arg, RegisterIndex length_temp, RegisterIndex calculation_temp, RegisterIndex min, RegisterIndex max) {
        // For a range 0..length like maybe 0 and 2pi? (though my tailor series do fall off in accuracy at 2pi so maybe -pi to pi is better?)
        text.Code.Add(new SubS {
            Destination = length_temp,
            LhsOperand = max,
            RhsOperand = min
        });
        // Compute temp1 = arg / length
        // Compute temp2 = floor(temp1)
        // Compute temp3 = temp2 * length
        // Compute temp4 = arg - temp3
        // Compute temp5 = clamp(temp4, 0, length)

    }

    public void Accept(Cos tuple)
    {
        // Load values
        // Convert to F32 (copy to FPU)
        // Get value in range of -2pi and 2pi
        // Do cos (no native FPU instruction) -- https://www.wolframalpha.com/input?i=taylor+expansion+of+cos%28x%29
        taylorExpansionCos(
            y:          new RegisterIndex(2),   // result stored in y
            x:          new RegisterIndex(8),   // argument stored and untouched in x
            cpuTemp:    RegisterIndex.At,       // temp register to copy CPU values into
            fpuTemp:    new RegisterIndex(10),  // temp register to copy FPU values into
            numerator:  new RegisterIndex(4),   // temp register to partially compute the numerator of the current expansion term
            denominator:new RegisterIndex(6)    // temp register to partially compute the denominator of the current expansion term
        );
        // Convert back to result type 
        // Copy to result register
        throw new NotImplementedException();
    }

    private int factorial(int n) {
        int factorialOfN = 1;
        for (int i = 1; i <= n; ++i) {
            factorialOfN *= i;   
        }
        return factorialOfN;
    }

    private void taylorExpansionSin(RegisterIndex y, RegisterIndex x, RegisterIndex cpuTemp, RegisterIndex fpuTemp, RegisterIndex numerator, RegisterIndex denominator, int order = 5) {
        // sin(x) ~= SUM{k = 0->order} ( ((-1)^k * x^(1+ 2*k)) / (1 + 2*k)!  )
        for (var k = 0; k < order; k++) {
            // All the numerator in the summation
            {
                var multiplier = MathF.Pow(-1.0f, k);
                if (multiplier == 0) {
                    continue;
                }
                // Compute numerator as x^ipow using repeated multiplications
                var ipow = 1 + 2 * k; // Number of times to multiply x
                text.Code.Add(new Li {
                    Destination = cpuTemp,
                    Value = (1.0f).ReinterpretUint()
                });
                text.Code.Add(new Mtc1{
                    FpuRegister = numerator,
                    CpuRegister = cpuTemp       // Set the inital numerator to 1 before repeated multiplications of x
                });
                for (var exp = 0; exp < ipow; exp++) {
                    text.Code.Add(new MulS {
                        Destination = numerator,
                        LhsOperand = numerator, 
                        RhsOperand = x
                    });
                }
                // Multiply numerator by multiplier
                text.Code.Add(new Li {
                    Destination = cpuTemp,
                    Value = multiplier.ReinterpretUint()
                });
                text.Code.Add(new Mtc1{
                    FpuRegister = fpuTemp,
                    CpuRegister = cpuTemp
                });
                text.Code.Add(new MulS {
                    Destination = numerator,
                    LhsOperand = fpuTemp, 
                    RhsOperand = numerator
                });
            }
            // All the denominator in the summation
            {
                var factorial = (float)this.factorial(1 + 2 * k);
                text.Code.Add(new Li {
                    Destination = cpuTemp,
                    Value = factorial.ReinterpretUint()
                });
                text.Code.Add(new Mtc1{
                    FpuRegister = denominator,
                    CpuRegister = cpuTemp
                });
            }
            // Final division (next_term = numerator/denominator)
            text.Code.Add(new DivS {
                Destination = fpuTemp,
                LhsOperand = numerator,
                RhsOperand = denominator,
            });
            // Summation (y = y + next_term)
            text.Code.Add(new AddS {
                Destination = y,
                LhsOperand = y,
                RhsOperand = fpuTemp
            });
        }
    }

    public void Accept(Sin tuple)
    {
        // Load values
        // Convert to F32 (copy to FPU)
        // Get value in range of -2pi and 2pi
        // Do sin (no native FPU instruction) // -- https://www.wolframalpha.com/input?i=taylor+expansion+of+sin%28x%29
                                              // -- https://www.wolframalpha.com/input?i=plot+x+-+x%5E3%2F6+%2B+x%5E5%2F120+-+x%5E7%2F5040+%2B+x%5E9%2F362880+-+x%5E11%2F39916800+%2B+x%5E13%2F6227020800+-+x%5E15%2F1307674368000+%2B+x%5E17%2F355687428096000%2C+sin%28x%29+between+-2*pi+and+2*pi+
        taylorExpansionSin(
            y:          new RegisterIndex(2),   // result stored in y
            x:          new RegisterIndex(8),   // argument stored and untouched in x
            cpuTemp:    RegisterIndex.At,       // temp register to copy CPU values into
            fpuTemp:    new RegisterIndex(10),  // temp register to copy FPU values into
            numerator:  new RegisterIndex(4),   // temp register to partially compute the numerator of the current expansion term
            denominator:new RegisterIndex(6)    // temp register to partially compute the denominator of the current expansion term
        );
        // Convert back to result type 
        // Copy to result register
        throw new NotImplementedException();
    }

    public void Accept(Sqrt tuple)
    {
        // Has native FPU instr
        throw new NotImplementedException();
    }

    public void Accept(Ln tuple)
    {
        // Load values
        // Convert to F32 (copy to FPU)
        // Do ln (no native FPU instruction) -- https://www.wolframalpha.com/input?i=taylor+expansion+of+ln%28x%29
        // Convert back to result type 
        // Copy to result register
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
        /*IMipsValueOperand lhs; IMipsValueOperand rhs;
        load(tuple.LeftOperand, tuple.RightOperand, out lhs, out rhs);
        text.Code.Add(new Bytecode.Instructions. {
            Destination = BinaryResultRegister,
            LhsOperand = BinaryLhsOperatorRegister,
            RhsOperand = BinaryRhsOperatorRegister
        });
        tuple.Result.MipsInstructionsToStoreValue(BinaryResultRegister);*/
        throw new NotImplementedException();
    }

    public void Accept(Inequality tuple)
    {
        throw new NotImplementedException();
    }

    public void Accept(Exit tuple) {
        exit();
    }

    public void Accept(Jump tuple) {
        text.Code.Add(new Assembly.Instructions.J { Address = new LabelAddress(generateLabel(tuple.Goto))});
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

    public void Accept(ReturnProcedure tuple) {
        // TODO 
    }

    public void Accept(ReturnFunction tuple)
    {
        throw new NotImplementedException();
    }
}
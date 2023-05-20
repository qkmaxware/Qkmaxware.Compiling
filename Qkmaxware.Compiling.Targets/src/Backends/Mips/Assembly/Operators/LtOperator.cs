using Qkmaxware.Compiling.Ir;
using Qkmaxware.Compiling.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

internal class LtOperator : BinaryOperator {

    public LtOperator(TextSection code, RegisterIndex result, RegisterIndex lhs, RegisterIndex rhs) : base(code, result, lhs, rhs) {}

    public override void Accept(F32 type) {
        // Move to FPU
        Section.Code.Add(new Mtc1 {
            CpuRegister = LhsOperand,
            FpuRegister = FpuLhsOperand,
        });
        Section.Code.Add(new Mtc1 {
            CpuRegister = RhsOperand,
            FpuRegister = FpuRhsOperand,
        });

        // Do operator
        Section.Code.Add(new CLtS {
            FlagIndex = 0,
            LhsOperand = FpuLhsOperand,
            RhsOperand = FpuRhsOperand,
        });
        Section.Code.Add(new Li {   
            Destination = Result,
            UintValue = 1           // Assume the result is true
        });
        Section.Code.Add(new Assembly.Instructions.Bc1t {
            ConditionFlagIndex = 0,
            Address = new IntegerAddress(8) // Skip setting the value to 0 (next instruction)
        });
        Section.Code.Add(new Bytecode.Instructions.Add {
            Destination = Result,
            LhsOperand = RegisterIndex.Zero,
            RhsOperand = RegisterIndex.Zero
        });
    }

    public override void Accept(I32 type) {
        // Direct instruction support
        Section.Code.Add(new Bytecode.Instructions.Slt {
            Destination = Result,
            LhsOperand = LhsOperand,
            RhsOperand = RhsOperand
        });
    }

    public override void Accept(U32 type) {
        // Direct instruction support
       Section.Code.Add(new Bytecode.Instructions.Sltu {
            Destination = Result,
            LhsOperand = LhsOperand,
            RhsOperand = RhsOperand
        });
    }

    public override void Accept(U1 type) {
        // Direct instruction support
        Section.Code.Add(new Bytecode.Instructions.Sltu {
            Destination = Result,
            LhsOperand = LhsOperand,
            RhsOperand = RhsOperand
        });
    }
}
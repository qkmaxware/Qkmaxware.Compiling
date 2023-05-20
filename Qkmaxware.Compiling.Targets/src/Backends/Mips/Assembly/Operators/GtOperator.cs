using Qkmaxware.Compiling.Ir;
using Qkmaxware.Compiling.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

internal class GtOperator : BinaryOperator {

    public GtOperator(TextSection code, RegisterIndex result, RegisterIndex lhs, RegisterIndex rhs) : base(code, result, lhs, rhs) {}

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
        Section.Code.Add(new CLeS {     // Compare less than or equal to
            FlagIndex = 0,
            LhsOperand = FpuLhsOperand,
            RhsOperand = FpuRhsOperand,
        });
        Section.Code.Add(new Li {   
            Destination = Result,
            UintValue = 0           // Assume the result is false
        });
        Section.Code.Add(new Assembly.Instructions.Bc1t {   // Thing is less than or equal to
            ConditionFlagIndex = 0,
            Address = new IntegerAddress(2 * 8) // Skip setting the value to 1 (next instruction li which is 2 bytecode instructions)
        });
        Section.Code.Add(new Assembly.Instructions.Li {
            Destination = Result,
            UintValue = 1
        });
    }

    public override void Accept(I32 type) {
        // Assume LT
        Section.Code.Add(new Li {
            Destination = Result,
            UintValue = 0
        });
        // Confirm LT
        Section.Code.Add(new Bytecode.Instructions.Sub {
            Destination = RegisterIndex.T0,
            LhsOperand = LhsOperand,
            RhsOperand = RhsOperand,
        });
        Section.Code.Add(new Assembly.Instructions.Blez {
            Source = RegisterIndex.T0,
            Address = new IntegerAddress(2 * 8) // Skip the next Li
        });
        // If not jumped over, its GT
        Section.Code.Add(new Li {
            Destination = Result,
            UintValue = 1
        });
    }

    public override void Accept(U32 type) {
        // Assume LT
        Section.Code.Add(new Li {
            Destination = Result,
            UintValue = 0
        });
        // Confirm LT
        Section.Code.Add(new Bytecode.Instructions.Subu {
            Destination = RegisterIndex.T0,
            LhsOperand = LhsOperand,
            RhsOperand = RhsOperand,
        });
        Section.Code.Add(new Assembly.Instructions.Blez {
            Source = RegisterIndex.T0,
            Address = new IntegerAddress(2 * 8) // Skip the next Li
        });
        // If not jumped over, its GT
        Section.Code.Add(new Li {
            Destination = Result,
            UintValue = 1
        });
    }

    public override void Accept(U1 type) {
        // Assume LT
        Section.Code.Add(new Li {
            Destination = Result,
            UintValue = 0
        });
        // Confirm LT
        Section.Code.Add(new Bytecode.Instructions.Subu {
            Destination = RegisterIndex.T0,
            LhsOperand = LhsOperand,
            RhsOperand = RhsOperand,
        });
        Section.Code.Add(new Assembly.Instructions.Blez {
            Source = RegisterIndex.T0,
            Address = new IntegerAddress(2 * 8) // Skip the next Li
        });
        // If not jumped over, its GT
        Section.Code.Add(new Li {
            Destination = Result,
            UintValue = 1
        });
    }
}
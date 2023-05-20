using Qkmaxware.Compiling.Ir;
using Qkmaxware.Compiling.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

internal class MulOperator : BinaryOperator {

    public MulOperator(TextSection code, RegisterIndex result, RegisterIndex lhs, RegisterIndex rhs) : base(code, result, lhs, rhs) {}

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
        Section.Code.Add(new MulS {
            Destination = FpuResult,
            LhsOperand = FpuLhsOperand,
            RhsOperand = FpuRhsOperand 
        });

        // Return from FPU
        Section.Code.Add(new Mfc1 {
            CpuRegister = Result,
            FpuRegister = FpuResult,
        });
    }

    public override void Accept(I32 type) {
        // Direct instruction support
        Section.Code.Add(new Bytecode.Instructions.Mult {
            LhsOperand = this.LhsOperand,
            RhsOperand = this.RhsOperand
        });
        // Ignore overflow...
        Section.Code.Add(new Bytecode.Instructions.Mflo {
            Destination = this.Result
        });
    }

    public override void Accept(U32 type) {
        // Direct instruction support
        Section.Code.Add(new Bytecode.Instructions.Multu {
            LhsOperand = this.LhsOperand,
            RhsOperand = this.RhsOperand
        });
        // Ignore overflow...
        Section.Code.Add(new Bytecode.Instructions.Mflo {
            Destination = this.Result
        });
    }

    public override void Accept(U1 type) {
        // Direct instruction support
        Section.Code.Add(new Bytecode.Instructions.Multu {
            LhsOperand = this.LhsOperand,
            RhsOperand = this.RhsOperand
        });
        // Ignore overflow...
        Section.Code.Add(new Bytecode.Instructions.Mflo {
            Destination = this.Result
        });
    }
}
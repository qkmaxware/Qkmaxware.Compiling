using Qkmaxware.Compiling.Ir;
using Qkmaxware.Compiling.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

internal class NegateOperator : UnaryOperator {

    public NegateOperator(TextSection code, RegisterIndex result, RegisterIndex operand) : base(code, result, operand) {}

    public override void Accept(F32 type) {
        // Move to FPU
        Section.Code.Add(new Mtc1 {
            CpuRegister = Operand,
            FpuRegister = FpuOperand,
        });
        Section.Code.Add(new Li {
            Destination = RegisterIndex.At,
            FloatValue = -1.0f
        });
        Section.Code.Add(new Mtc1 {
            CpuRegister = Operand,
            FpuRegister = FpuTemp,
        });

        // Do operator
        Section.Code.Add(new MulS {
            Destination = FpuResult,
            LhsOperand = FpuOperand,
            RhsOperand = FpuTemp 
        });

        // Return from FPU
        Section.Code.Add(new Mfc1 {
            CpuRegister = Result,
            FpuRegister = FpuResult,
        });
    }

    public override void Accept(I32 type) {
        // Direct instruction support
        Section.Code.Add(new Li {
            Destination = RegisterIndex.At,
            IntValue = -1
        });
        Section.Code.Add(new Mult {
            LhsOperand = RegisterIndex.At,
            RhsOperand = Operand
        });
        Section.Code.Add(new Mflo {
            Destination = Result
        });
    }

    public override void Accept(U32 type) {
        // Do nothing, can't negate an unsigned number
    }

    public override void Accept(U1 type) {
        // Do nothing, can't negate an unsigned number
    }
}
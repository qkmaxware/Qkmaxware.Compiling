using Qkmaxware.Compiling.Ir;
using Qkmaxware.Compiling.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

internal class AbsOperator : UnaryOperator {

    public AbsOperator(TextSection code, RegisterIndex result, RegisterIndex operand) : base(code, result, operand) {}

    public override void Accept(F32 type) {
        // Move to FPU
        Section.Code.Add(new Mtc1 {
            CpuRegister = Operand,
            FpuRegister = FpuOperand,
        });

        // Do operator
        Section.Code.Add(new AbsS {
            Destination = FpuResult,
            Source = FpuOperand,
        });

        // Return from FPU
        Section.Code.Add(new Mfc1 {
            CpuRegister = Result,
            FpuRegister = FpuResult,
        });
    }

    public override void Accept(I32 type) {
        // Direct instruction support
        var instr = 4;
        Section.Code.Add(new Assembly.Instructions.Bgtz {
            Source = Operand,
            Address = new IntegerAddress(instr * 8) // Skip next instruction(s) if already positive
        });
        Section.Code.Add(new Li {
            Destination = RegisterIndex.At,
            IntValue = -1
        }); //-- worth 2
        Section.Code.Add(new Mult {
            LhsOperand = Operand,
            RhsOperand = RegisterIndex.At
        }); //-- worth 1
        Section.Code.Add(new Mflo {
            Destination = Result
        }); //-- worth 1
    }

    public override void Accept(U32 type) {
        // Do nothing
    }

    public override void Accept(U1 type) {
        // Do nothing
    }
}
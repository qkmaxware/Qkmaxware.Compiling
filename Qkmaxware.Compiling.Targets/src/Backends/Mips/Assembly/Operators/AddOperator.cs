using Qkmaxware.Compiling.Targets.Ir;
using Qkmaxware.Compiling.Targets.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

internal class AddOperator : IIrTypeVisitor {
    private RegisterIndex Result;
    private RegisterIndex LhsOperand;
    private RegisterIndex RhsOperand;
    private TextSection Section;

    private RegisterIndex FpuResult = new RegisterIndex(0);
    private RegisterIndex FpuLhsOperand = new RegisterIndex(2);
    private RegisterIndex FpuRhsOperand = new RegisterIndex(4);

    public AddOperator(TextSection code, RegisterIndex result, RegisterIndex lhs, RegisterIndex rhs) {
        this.Result = result;
        this.LhsOperand = lhs;
        this.RhsOperand = rhs;
        this.Section = code;
    }

    public void Accept(F32 type) {
        // Move to FPU
        Section.Code.Add(new Mtc1 {
            CpuRegister = LhsOperand,
            FpuRegister = FpuLhsOperand,
        });
        Section.Code.Add(new Mtc1 {
            CpuRegister = RhsOperand,
            FpuRegister = FpuRhsOperand,
        });

        // Do add
        Section.Code.Add(new AddS {
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

    public void Accept(I32 type) {
        // Direct instruction support
        Section.Code.Add(new Bytecode.Instructions.Add {
            Destination = this.Result,
            LhsOperand = this.LhsOperand,
            RhsOperand = this.RhsOperand
        });
    }

    public void Accept(U32 type) {
        // Direct instruction support
        Section.Code.Add(new Bytecode.Instructions.Addu {
            Destination = this.Result,
            LhsOperand = this.LhsOperand,
            RhsOperand = this.RhsOperand
        });
    }

    public void Accept(U1 type) {
        // Direct instruction support
        Section.Code.Add(new Bytecode.Instructions.Addu {
            Destination = this.Result,
            LhsOperand = this.LhsOperand,
            RhsOperand = this.RhsOperand
        });
    }
}
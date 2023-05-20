using Qkmaxware.Compiling.Ir;
using Qkmaxware.Compiling.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

internal class RemOperator : BinaryOperator {

    public RemOperator(TextSection code, RegisterIndex result, RegisterIndex lhs, RegisterIndex rhs) : base(code, result, lhs, rhs) {}

    public override void Accept(F32 type) {
        throw new NotImplementedException();
    }

    public override void Accept(I32 type) {
        // Direct instruction support
        Section.Code.Add(new Bytecode.Instructions.Div {
            LhsOperand = this.LhsOperand,
            RhsOperand = this.RhsOperand
        });
        // Ignore quotient...
        Section.Code.Add(new Bytecode.Instructions.Mfhi {
            Destination = this.Result
        });
    }

    public override void Accept(U32 type) {
        // Direct instruction support
        Section.Code.Add(new Bytecode.Instructions.Divu {
            LhsOperand = this.LhsOperand,
            RhsOperand = this.RhsOperand
        });
        // Ignore quotient...
        Section.Code.Add(new Bytecode.Instructions.Mfhi {
            Destination = this.Result
        });
    }

    public override void Accept(U1 type) {
        // Direct instruction support
        Section.Code.Add(new Bytecode.Instructions.Divu {
            LhsOperand = this.LhsOperand,
            RhsOperand = this.RhsOperand
        });
        // Ignore quotient...
        Section.Code.Add(new Bytecode.Instructions.Mfhi {
            Destination = this.Result
        });
    }
}
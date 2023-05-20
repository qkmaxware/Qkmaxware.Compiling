using Qkmaxware.Compiling.Ir;
using Qkmaxware.Compiling.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

internal abstract class BinaryOperator : IIrTypeVisitor {

    protected RegisterIndex Result {get; private set;}
    protected RegisterIndex LhsOperand {get; private set;}
    protected RegisterIndex RhsOperand {get; private set;}
    protected TextSection Section {get; private set;}

    // Always leave F0 to 0 (like Zero on the CPU)
    protected RegisterIndex FpuResult {get; private set;} = new RegisterIndex(2);
    protected RegisterIndex FpuLhsOperand {get; private set;} = new RegisterIndex(4);
    protected RegisterIndex FpuRhsOperand {get; private set;} = new RegisterIndex(6);

    public BinaryOperator(TextSection code, RegisterIndex result, RegisterIndex lhs, RegisterIndex rhs) {
        this.Result = result;
        this.LhsOperand = lhs;
        this.RhsOperand = rhs;
        this.Section = code;
    }

    public abstract void Accept(F32 type);

    public abstract void Accept(I32 type);

    public abstract void Accept(U32 type);

    public abstract void Accept(U1 type);

}
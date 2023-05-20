using Qkmaxware.Compiling.Ir;
using Qkmaxware.Compiling.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

internal abstract class UnaryOperator : IIrTypeVisitor {

    protected RegisterIndex Result {get; private set;}
    protected RegisterIndex Operand {get; private set;}
    
    protected TextSection Section {get; private set;}

    // Always leave F0 to 0 (like Zero on the CPU)
    protected RegisterIndex FpuResult {get; private set;} = new RegisterIndex(2);
    protected RegisterIndex FpuOperand {get; private set;} = new RegisterIndex(4);
    protected RegisterIndex FpuTemp {get; private set;} = new RegisterIndex(6);

    public UnaryOperator(TextSection code, RegisterIndex result, RegisterIndex operand) {
        this.Result = result;
        this.Operand = operand;
        this.Section = code;
    }

    public abstract void Accept(F32 type);

    public abstract void Accept(I32 type);

    public abstract void Accept(U32 type);

    public abstract void Accept(U1 type);

}
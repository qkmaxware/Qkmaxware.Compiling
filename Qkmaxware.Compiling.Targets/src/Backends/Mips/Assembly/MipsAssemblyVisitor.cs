using Qkmaxware.Compiling.Ir;
using Qkmaxware.Compiling.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.Mips.Assembly;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

internal class MipsAssemblyVisitor : ModuleVisitor {

    private Mips.Assembly.DataSection data;
    private MipsAssemblyCodeWalker textEncoder;
    private TextSection text;

    public MipsAssemblyVisitor(DataSection data, TextSection text) {
        this.data = data;
        this.text = text;
        this.textEncoder = new MipsAssemblyCodeWalker(text);
    }

    public void VisitModule(Module module) {
        foreach (var global in module.Globals) {
            if (global is Global g)
                VisitGlobalDeclaration(g);
        }
        foreach (var subroutine in module.Subprograms) {
            VisitSubprogram(subroutine);
        }
    }

    private void StackPush(TextSection code, RegisterIndex from) {
        var stackRegister = RegisterIndex.SP;
        code.Code.Add(new Sw {
            Target = from,
            Source = stackRegister,
            Immediate = 0
        });
        code.Code.Add(new Addi {
            Target      = stackRegister,
            Source  = stackRegister,
            Immediate = (-4).ReinterpretUint(), // Stack grows from the bottom up
        });
    }

    private void StackPop(TextSection code, RegisterIndex to) {
        var stackRegister = RegisterIndex.SP;
        code.Code.Add(new Lw {
            Target = to,
            Source = stackRegister,
            Immediate = 0
        });
        code.Code.Add(new Addi {
            Target      = stackRegister,
            Source  = stackRegister,
            Immediate          = 4, // Stack grows from the bottom up
        });
    }

    public void VisitSubprogram(Subprogram sub) {
        // Create a text section for this subprogram and setup default instructions
        text.Code.Add(new Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions.Label("proc_" + sub.ProcedureIndex)); // Give this program a name

        // Push the return address (restored when procedure returns)
        StackPush(text, RegisterIndex.SP);      // Store previous stack pointer
        StackPush(text, RegisterIndex.FP);      // Store previous frame pointer
        StackPush(text, RegisterIndex.RA);      // Store previous return address

        // Store the start of the frame as the current frame-pointer
        text.Code.Add(new Move {
            Source = RegisterIndex.SP,
            Destination = RegisterIndex.FP
        });

        // Define all locals (these are all stored at FP + Local Index)
        foreach (var local in sub.Locals) {
            if (local is Local l)
                VisitLocalDeclaration(l);
        }

        // Copy argument values to the appropriate locals

        // Do the code for the entrypoint
        this.textEncoder.StartWalk(sub.Entrypoint);
    }

    private static uint NotImportant = 0;

    public void VisitGlobalDeclaration(Global def) {
        var type = def.TypeOf();
        var name = new LabelToken(NotImportant, def.MipsMemoryLabel);
        switch (type) {
            case I32 i32:
                data.Data.Add(new Data<int>(
                    name: name,
                    storage: new DirectiveToken(NotImportant, "word"),
                    value: i32.DefaultValue()
                ));
                break;
            case U1 u1:
                data.Data.Add(new Data<uint>(
                    name: name,
                    storage: new DirectiveToken(NotImportant, "word"),
                    value: u1.DefaultValue()
                ));
                break;
            case U32 u32:
                data.Data.Add(new Data<uint>(
                    name: name,
                    storage: new DirectiveToken(NotImportant, "word"),
                    value: u32.DefaultValue()
                ));
                break;
            case F32 f32:
                data.Data.Add(new Data<float>(
                    name: name,
                    storage: new DirectiveToken(NotImportant, "single"),
                    value: f32.DefaultValue()
                ));
                break;
            default:
                throw new NotImplementedException($"Type '{type}' is not supported by {typeof(MipsAssemblyBackend)}.");
        }
    }

    public void VisitLocalDeclaration(Local def) {
        var type = def.TypeOf();
        var temp = RegisterIndex.T0;
        switch (type) {
            case I32 i32:
                text.Code.Add(new Li{
                    Destination = temp,
                    IntValue = i32.DefaultValue(),
                });
                StackPush(text, temp);
                break;
            case U1 u1:
                text.Code.Add(new Li{
                    Destination = temp,
                    UintValue = u1.DefaultValue(),
                });
                StackPush(text, temp);
                break;
            case U32 u32:
                text.Code.Add(new Li{
                    Destination = temp,
                    UintValue = u32.DefaultValue(),
                });
                StackPush(text, temp);
                break;
            case F32 f32:
                text.Code.Add(new Li{
                    Destination = temp,
                    FloatValue = f32.DefaultValue(),
                });
                StackPush(text, temp);
                break;
            default:
                throw new NotImplementedException($"Type '{type}' is not supported by {typeof(MipsAssemblyBackend)}.");
        }
    }
}
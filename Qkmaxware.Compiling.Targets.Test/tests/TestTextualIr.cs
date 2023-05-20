using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.C;

namespace Qkmaxware.Compiling.Ir.Test;

[TestClass]
public class TestTextual2C {

    [TestMethod]
    public void TestPrintAdd() {
        var module = new Ir.Module();
        var pi = module.MakeGlobal(IrType.F32, "pi");
        
        var square = module.MakeFunction("square", IrType.F32, IrType.F32);
        square.Entrypoint.AddInstruction(new Mul(square.Arguments.ElementAt(0), square.Arguments.ElementAt(0), square.ReturnLocal));

        var area = module.MakeFunction("area", IrType.F32, IrType.F32);
        var temp = area.MakeLocal(IrType.F32, "rSquared");
        area.Entrypoint.AddInstruction(new CallFunction(square, temp, area.Arguments.ElementAt(0)));
        area.Entrypoint.AddInstruction(new Mul(temp, pi, area.ReturnLocal));

        var main = module.MakeProcedure("main");
        var arg1 = main.MakeLocal(IrType.F32, "arg_1");
        var arg2 = main.MakeLocal(IrType.F32, "arg_2");
        var arg3 = main.MakeLocal(IrType.F32, "arg_3");
        var arg4 = main.MakeLocal(IrType.F32, "arg_4"); 
        var test1 = main.MakeLocal(IrType.F32, "result_1");
        var test2 = main.MakeLocal(IrType.F32, "result_2");
        var test3 = main.MakeLocal(IrType.F32, "result_3");
        var test4 = main.MakeLocal(IrType.F32, "result_4"); 
        main.Entrypoint.AddInstructions(
            new Copy(arg1, new LiteralF32(4)),
            new Copy(arg2, new LiteralF32(6)),
            new Copy(arg3, new LiteralF32(8)),
            new Copy(arg3, new LiteralF32(10)),

            new CallFunction(area, test1, arg1),
            new CallFunction(area, test2, arg2),
            new CallFunction(area, test3, arg3),
            new CallFunction(area, test4, arg4)
        );

        var tex = new Textual.TextualRepresentation();
        using (var writer = new StreamWriter("TestTextual2C.TestPrintAdd.qkir")) {
            tex.PrintTo(module, writer);
        }
    }

}
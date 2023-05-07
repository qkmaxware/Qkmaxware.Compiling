using Qkmaxware.Compiling.Targets.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Targets.Ir.Test;

[TestClass]
public class TestDotExporter {

    [TestMethod]
    public void TestExport() {
        var exporter = new DotExporter();
        var module = new Ir.Module();

        // square (value) => value * value
        // area (radius) => pi * square(radius)
        var square = module.MakeFunction(IrType.I32, IrType.I32);
        square.Name = "square(int value):int";
        var area = module.MakeFunction(IrType.I32, IrType.I32);
        var temp1 = area.MakeLocal(IrType.I32, "temp");
        area.Name = "area(int radius):int";

        if (square.ReturnLocal == null)
            Assert.Fail();
        if (area.ReturnLocal == null)
            Assert.Fail();

        square.Entrypoint.Instructions.Add(new Mul(square.Locals[1], square.Locals[1], square.ReturnLocal));
        area.Entrypoint.Instructions.Add(new CallFunction(square, temp1, square.Locals[1]));
        area.Entrypoint.Instructions.Add(new Mul(new LiteralF32(3.14f), temp1, area.ReturnLocal));

        using (var file = new StreamWriter("TestDotExporter.TestExport.dot")) {
            exporter.ExportTo(module, file);
        }
    }
}
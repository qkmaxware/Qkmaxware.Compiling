using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Targets.Ir.TypeSystem;
using Qkmaxware.Compiling.Targets.C;

namespace Qkmaxware.Compiling.Targets.Ir.Test;

[TestClass]
public class TestIr2C {

    [TestMethod]
    public void TestGlobals() {
        var module = new Ir.Module();
        module.MakeGlobal(IrType.U1, "UInt1");
        module.MakeGlobal(IrType.U32, "UInt32");
        module.MakeGlobal(IrType.I32, "Int32");
        module.MakeGlobal(IrType.F32, "Single");

        var backend = new CBackend();
        backend.TryEmitToFile(module, "TestIr2C.TestGlobals");
    }

    [TestMethod]
    public void TestAdd() {
        var module = new Ir.Module();
        {
            var proc = module.MakeProcedure();
            proc.Name = "AddI32";
            var returns = proc.MakeLocal(IrType.I32, "return");
            // 4 + 6
            proc.Entrypoint.Instructions.Add(new Add(new LiteralI32(4), new LiteralI32(6), returns));
        }
        {
            var proc = module.MakeProcedure();
            proc.Name = "AddU32";
            var returns = proc.MakeLocal(IrType.U32, "return");
            // 4 + 6
            proc.Entrypoint.Instructions.Add(new Add(new LiteralU32(4), new LiteralU32(6), returns));
        }
        {
            var proc = module.MakeProcedure();
            proc.Name = "AddF32";
            var returns = proc.MakeLocal(IrType.F32, "return");
            // 4 + 6
            proc.Entrypoint.Instructions.Add(new Add(new LiteralF32(4), new LiteralF32(6), returns));
        }
        var backend = new CBackend();
        backend.TryEmitToFile(module, "TestIr2C.TestAdd");
    }

}
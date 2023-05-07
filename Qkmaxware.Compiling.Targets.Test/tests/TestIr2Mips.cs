using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Targets.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Targets.Ir.Test;

[TestClass]
public class TestIr2Mips {

    [TestMethod]
    public void TestGlobals() {
        var module = new Ir.Module();
        module.MakeGlobal(IrType.U1, "UInt1");
        module.MakeGlobal(IrType.U32, "UInt32");
        module.MakeGlobal(IrType.I32, "Int32");
        module.MakeGlobal(IrType.F32, "Single");

        var backend = new MipsAssemblyBackend();
        backend.TryEmitToFile(module, "TestIr2Mips.TestGlobals");
    }

}
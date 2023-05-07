using Qkmaxware.Compiling.Targets.Mips;
using Qkmaxware.Compiling.Targets.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Targets.Ir.Test;

[TestClass]
public class TestIrTypeConversion {

    [TestMethod]
    public void TestNoConvert() {
        var conversions = TypeConversion.EnumerateConversions(IrType.I32, IrType.I32);
        Assert.AreEqual(0, conversions.Count);
    }

    [TestMethod]
    public void TestSimpleConvert() {
        var conversions = TypeConversion.EnumerateConversions(IrType.I32, IrType.F32);
        Assert.AreEqual(1, conversions.Count);
        Assert.IsInstanceOfType(conversions[0], typeof(I32ToF32));
    }

    [TestMethod]
    public void TestShortestConvert() {
        var conversions = TypeConversion.EnumerateConversions(IrType.I32, IrType.U1);
        /*
        // Longest Path
        Assert.AreEqual(3, conversions.Count);
        Assert.IsInstanceOfType(conversions[0], typeof(I32ToF32));
        Assert.IsInstanceOfType(conversions[1], typeof(F32ToU32));
        Assert.IsInstanceOfType(conversions[2], typeof(U32ToU1));*/
        // Shortest Path
        Assert.AreEqual(2, conversions.Count);
        Assert.IsInstanceOfType(conversions[0], typeof(I32ToU32));
        Assert.IsInstanceOfType(conversions[1], typeof(U32ToU1));
    }

}
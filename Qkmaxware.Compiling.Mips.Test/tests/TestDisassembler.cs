using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Compiling.Targets.Mips.Assembly;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Qkmaxware.Compiling.Targets.Mips.Test;

[TestClass]
public class TestDisassembler {

    [TestMethod]
    public void TestSupportedOperations() {
        Assert.AreEqual(Disassembler.CountAllBytecodeOperations(), Disassembler.CountSupportedBytecodeOperations(), "Some bytecode instructions are not able to be decoded.");
    }

}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Compiling.Mips.Assembly;
using Qkmaxware.Compiling.Mips.Bytecode;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Qkmaxware.Compiling.Mips.Test;

[TestClass]
public class TestWordEncoder {

    [TestMethod]
    public void TestEncoding() {
        WordEncoder encoder;

        encoder = new WordEncoder();
        encoder.Encode(uint.MaxValue, 0..8);
        Assert.AreEqual("00000000000000000000000011111111", encoder.ToString());
        
        encoder = new WordEncoder();
        encoder.Encode(uint.MaxValue, 8..16);
        Assert.AreEqual("00000000000000001111111100000000", encoder.ToString());

        encoder = new WordEncoder();
        encoder.Encode(uint.MaxValue, 16..24);
        Assert.AreEqual("00000000111111110000000000000000", encoder.ToString());

        encoder = new WordEncoder();
        encoder.Encode(uint.MaxValue, 24..32);
        Assert.AreEqual("11111111000000000000000000000000", encoder.ToString());
    }

    [TestMethod]
    public void TestFloatingPointEncoding() {
        WordEncoder encoder;

        // OOOOOOCCCCCTTTTTDDDDDIIIIIIIIIII
        // 00000000000000000000000000000000

        encoder = new WordEncoder();
        encoder.Encode(uint.MaxValue, 26..32);
        Assert.AreEqual("11111100000000000000000000000000", encoder.ToString());
        
        encoder = new WordEncoder();
        encoder.Encode(uint.MaxValue, 21..26);
        Assert.AreEqual("00000011111000000000000000000000", encoder.ToString());

        encoder = new WordEncoder();
        encoder.Encode(uint.MaxValue, 16..21);
        Assert.AreEqual("00000000000111110000000000000000", encoder.ToString());

        encoder = new WordEncoder();
        encoder.Encode(uint.MaxValue, 11..16);
        Assert.AreEqual("00000000000000001111100000000000", encoder.ToString());

        encoder = new WordEncoder();
        encoder.Encode(uint.MaxValue, 0..11);
        Assert.AreEqual("00000000000000000000011111111111", encoder.ToString());
    }
}
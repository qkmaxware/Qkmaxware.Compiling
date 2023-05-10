using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Compiling.Targets.Mips.Assembly;
using Qkmaxware.Compiling.Targets.Mips.Bytecode;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qkmaxware.Compiling.Targets.Mips.Assembly.Instructions;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips.Test;

[TestClass]
public class TestEncodeDecode {

    [TestMethod]
    public void TestEncoding() {
        Add add = new Add {                     // 100000
            Destination = new RegisterIndex(1), // 00001
            LhsOperand = new RegisterIndex(3),  // 00011
            RhsOperand = new RegisterIndex(6)   // 00110
        };

        // 000000ss sssttttt dddddaaa aaffffff
        // 00000000 01100110 00001000 00100000
        uint encoded = add.Encode32();
        Assert.AreEqual("00000000011001100000100000100000", System.Convert.ToString(encoded, 2).PadLeft(32, '0'));

        // Check bit pattern
        var word = new WordEncoder(encoded);
        var src = word.Decode(21..26);
        var tgt = word.Decode(16..21);
        var des = word.Decode(11..16);

        Assert.AreEqual((uint)add.LhsOperand, src);
        Assert.AreEqual((uint)add.RhsOperand, tgt);
        Assert.AreEqual((uint)add.Destination, des);

        // Check decoding 
        IBytecodeInstruction? decoded;
        bool didDecode = Add.TryDecodeBytecode(encoded, out decoded);

        Assert.AreEqual(true, didDecode);
        Assert.IsNotNull(decoded);
        Assert.IsInstanceOfType(decoded, typeof(Add));
        Assert.AreEqual(add.Destination, ((Add)decoded).Destination);
        Assert.AreEqual(add.LhsOperand,  ((Add)decoded).LhsOperand);
        Assert.AreEqual(add.RhsOperand,  ((Add)decoded).RhsOperand);
    }
}
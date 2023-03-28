using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Compiling.Mips.Assembly;
using System.Collections.Generic;
using System.Linq;

namespace Qkmaxware.Compiling.Mips.Test;

[TestClass]
public class TestAssembly {

    private static string asm =
@".data
    fibs: .word  0 : 19         # ""array"" of words to contain fib values
    size: .word  19             # size of ""array"" (agrees with array declaration)
    prompt: .asciiz ""How many Fibonacci numbers to generate? (2 <= x <= 19)""
.text
      la   $s0, fibs        # load address of array
      la   $s5, size        # load address of size variable
      lw   $s5, 0($s5)      # load array size";

    [TestMethod]
    public void TestLexing() {
        AsmLexer lexer = new AsmLexer();
        var tokens = lexer.Tokenize(asm).ToArray();
        System.Console.WriteLine(string.Join(',', (IEnumerable<Token>)tokens));
        Assert.Fail();
    }

}
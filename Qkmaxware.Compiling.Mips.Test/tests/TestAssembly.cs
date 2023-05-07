using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Compiling.Targets.Mips.Assembly;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Qkmaxware.Compiling.Targets.Mips.Test;

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
    public void TestSupportedOperations() {
        Assert.AreEqual(Parser.CountAllAssemblyInstructions(), Parser.CountSupportedAssemblyInstructions(), "Some assembly instructions are not able to be parsed.");
    }

    [TestMethod]
    public void TestLexing() {
        Lexer lexer = new Lexer();
        var tokens = lexer.Tokenize(asm).ToArray();
        Assert.AreEqual(35, tokens.Length);
        Assert.IsInstanceOfType(tokens.First(), typeof(DirectiveToken));
        Assert.IsInstanceOfType(tokens.Last(), typeof(CloseParenthesisToken));
    }

    [TestMethod]
    public void TestParsing() {
        var lexer = new Lexer();
        var parser = new Parser();
        var lexemes = lexer.Tokenize(asm);
        var program = parser.Parse(lexemes);

        Assert.IsNotNull(program);
        Assert.AreEqual(2, program.Sections.Count);
        
        Assert.IsInstanceOfType(program.Sections[0], typeof(DataSection));
        Assert.AreEqual(3, program.DataSections.First().Data.Count);

        Assert.IsInstanceOfType(program.Sections[1], typeof(TextSection));
        Assert.AreEqual(3, program.TextSections.First().Code.Count);
    } 

    [TestMethod]
    public void TestWriting() {
        var asm = "fibonacci.asm".ReadEmbeddedFile();
        Assert.AreNotEqual(string.Empty, asm);
        var lexer = new Lexer();
        var parser = new Parser();
        var lexemes = lexer.Tokenize(asm);
        try {
            var program = parser.Parse(lexemes);

            using (var writer = new StreamWriter("fibonacci.re_emitted.asm")) {
                var w = new AssemblyWriter(writer);
                w.Emit(program);
            }
        } catch (AssemblyException ex) {
            var line = 0;
            for (var i = 0; i < ex.SourcePosition; i++) {
                if (asm[i] == '\n')
                    line++;
            }
            throw new System.Exception(ex.Message + " @line " + (line) + " " + asm.GetLine(line), ex);
        }
    }
}
using Qkmaxware.Compiling.Targets.Mips.Bytecode;
using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips;

public delegate bool TryDecodeBytecode(uint bytecode, out IBytecodeInstruction? decoded);

public class Disassembler {

    /// <summary>
    /// Count of all supported operations in this disassembler
    /// </summary>
    /// <returns>count</returns>
    public static int CountSupportedBytecodeOperations() => decoders.Count;
    /// <summary>
    /// Count of all bytecode operations in this assembly
    /// </summary>
    /// <returns>count</returns>
    public static int CountAllBytecodeOperations() =>
        typeof(Disassembler)
        .Assembly
        .GetTypes()
        .Where(type => type.IsClass && !type.IsAbstract && type.IsAssignableTo(typeof(IBytecodeInstruction)))
        .Count();
    /// <summary>
    /// Percentage of bytecode operations supported by this assembler
    /// </summary>
    /// <returns>normalized percent</returns>
    public static float PercentBytecodeOperationsSupported () => (float)CountSupportedBytecodeOperations() / (float)CountAllBytecodeOperations();

    private static List<TryDecodeBytecode> decoders = new List<TryDecodeBytecode> {
        #region Arithmetic & Logical
        AbsS.TryDecodeBytecode,
        Add.TryDecodeBytecode,
        AddS.TryDecodeBytecode,
        Addi.TryDecodeBytecode,
        Addiu.TryDecodeBytecode,
        Addu.TryDecodeBytecode,
        And.TryDecodeBytecode,
        Andi.TryDecodeBytecode,
        Div.TryDecodeBytecode,
        DivS.TryDecodeBytecode,
        Divu.TryDecodeBytecode,
        MulS.TryDecodeBytecode,
        Mult.TryDecodeBytecode,
        Multu.TryDecodeBytecode,
        Nor.TryDecodeBytecode,
        Or.TryDecodeBytecode,
        Ori.TryDecodeBytecode,
        Sllv.TryDecodeBytecode,
        Srlv.TryDecodeBytecode,
        Sub.TryDecodeBytecode,
        SubS.TryDecodeBytecode,
        Subu.TryDecodeBytecode,
        Xor.TryDecodeBytecode,
        Xori.TryDecodeBytecode,
        #endregion
        #region Branch
        Beq.TryDecodeBytecode,
        Bgtz.TryDecodeBytecode,
        Blez.TryDecodeBytecode,
        Bne.TryDecodeBytecode,
        #endregion 
        #region Comparison
        Slt.TryDecodeBytecode,
        Slti.TryDecodeBytecode,
        Sltiu.TryDecodeBytecode,
        Sltu.TryDecodeBytecode,
        #endregion
        #region Constant Manipulator
        Lui.TryDecodeBytecode,
        #endregion
        #region Data Movement
        Mfc1.TryDecodeBytecode,
        Mfhi.TryDecodeBytecode,
        Mflo.TryDecodeBytecode,
        Mtc1.TryDecodeBytecode,
        Mthi.TryDecodeBytecode,
        Mtlo.TryDecodeBytecode,
        #endregion
        #region Exception and Interrupts
        Nop.TryDecodeBytecode,
        Syscall.TryDecodeBytecode,
        #endregion
        #region Jump
        J.TryDecodeBytecode,
        Jal.TryDecodeBytecode,
        Jalr.TryDecodeBytecode,
        Jr.TryDecodeBytecode,
        #endregion
        #region Load
        Lb.TryDecodeBytecode,
        Lbu.TryDecodeBytecode,
        Lh.TryDecodeBytecode,
        Lhu.TryDecodeBytecode,
        Lw.TryDecodeBytecode,
        Lwc1.TryDecodeBytecode,
        #endregion
        #region  Store
        Sb.TryDecodeBytecode,
        Sh.TryDecodeBytecode,
        Sw.TryDecodeBytecode,
        Swc1.TryDecodeBytecode,
        #endregion
    };

    /// <summary>
    /// Disassemble a MIPS bytecode file into separate instructions
    /// </summary>
    /// <param name="reader">binary reader</param>
    /// <returns>list of decoded instructions</returns>
    public IEnumerable<IBytecodeInstruction> Disassemble(BinaryReader reader) {
        while (reader.BaseStream.Position != reader.BaseStream.Length) {
            var bytecode = reader.ReadUInt32();
            IBytecodeInstruction? decoded = null;
            IBytecodeInstruction? instr = null;
            foreach (var decoder in decoders) {
                if (decoder.Invoke(bytecode, out decoded)) {
                    instr = decoded;
                    break;
                }
            }

            if (instr != null) {
                // Made it through all decoders and one of them DID match
                yield return instr;
                continue;
            } else {
                // Made it through all decoders and didn't get a valid bytecode
                throw new FormatException($"Invalid bytecode format 0b{Convert.ToString(bytecode, 2).PadLeft(32, '0')}.");
            }
        }
    }
}
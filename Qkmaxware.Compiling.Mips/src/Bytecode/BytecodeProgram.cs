using System.Collections;
using System.Text;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Base class for bytecode encoded programs
/// </summary>
public abstract class BytecodeProgram : IEnumerable<IBytecodeInstruction> {
    public abstract int InstructionCount {get;}
    public abstract IBytecodeInstruction this[uint index] {get;}
    public abstract IEnumerator<IBytecodeInstruction> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Dump the MIPS binary instructions to the given writer
    /// </summary>
    /// <param name="writer">writer to dump to</param>
    public void Dump(BinaryWriter writer) {
        foreach (var instr in this) {
            writer.Write(instr.Encode32());
        }
    }

    /// <summary>
    /// Dump the MIPS binary to file
    /// </summary>
    public void DumpToFile(string file_path) {
        using var file = File.OpenWrite(file_path);
        using var writer = new BinaryWriter(file);

        Dump(writer);
    }
}

/// <summary>
/// A bytecode program held entirely in memory for manipulating instructions
/// </summary>
public class InMemoryBytecodeProgram : BytecodeProgram {

    /// <summary>
    /// Number of instructions
    /// </summary>
    public override int InstructionCount => list.Count;

    public override IBytecodeInstruction this[uint index] => list[(int)index];

    /// <summary>
    /// Size of the program
    /// </summary>
    /// <returns>size</returns>
    public DataSize Size => DataSize.Bytes((uint)InstructionCount * 4U); // 4 bytes per word

    private List<IBytecodeInstruction> list = new List<IBytecodeInstruction>();

    /// <summary>
    /// Add a bytecode instruction
    /// </summary>
    /// <param name="instruction">instruction to add</param>
    public void Add(IBytecodeInstruction instruction) => list.Add(instruction);

    /// <summary>
    /// Add several bytecode instructions
    /// </summary>
    /// <param name="instructions">instructions to add</param>
    public void AddAll(IEnumerable<IBytecodeInstruction> instructions) => list.AddRange(instructions);

    /// <summary>
    /// Copy all instructions from this program and emit them to another writer
    /// </summary>
    /// <param name="writer">writer to write to</param>
    public void CopyTo(InMemoryBytecodeProgram writer) {
        foreach (var i in this.list)
            writer.Add(i);
    }

    /// <summary>
    /// Enumerate over bytecode instructions
    /// </summary>
    /// <returns>enumerator</returns>
    public override IEnumerator<IBytecodeInstruction> GetEnumerator() => list.GetEnumerator();

    /// <summary>
    /// Write binary program to as hex string
    /// </summary>
    /// <returns>hex string</returns>
    public override string ToString() {
        StringBuilder sb = new StringBuilder();

        foreach (var instr in this.list) {
            foreach (var @byte in BitConverter.GetBytes(instr.Encode32())) {
                sb.Append(@byte.ToString("X2")); sb.Append(' ');
            }
        }

        return sb.ToString();
    }
}
using System.Collections;

namespace Qkmaxware.Compiling.Mips.Bytecode;

/// <summary>
/// Base class for bytecode encoded programs
/// </summary>
public abstract class BytecodeProgram : IEnumerable<IBytecodeInstruction> {
    public abstract IEnumerator<IBytecodeInstruction> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() {
        return this.GetEnumerator();
    }
}

/// <summary>
/// Interface for an object that can write/emit bytecode instructions
/// </summary>
public interface IBytecodeWriter {
    public void Encode(IBytecodeInstruction instruction);
    public void EncodeAll(IEnumerable<IBytecodeInstruction> instructions);
}

/// <summary>
/// A bytecode program held entirely in memory for manipulating instructions
/// </summary>
public class InMemoryBytecodeProgram : BytecodeProgram, IBytecodeWriter {

    /// <summary>
    /// Number of instructions
    /// </summary>
    public int Count => list.Count;

    /// <summary>
    /// Size of the program
    /// </summary>
    /// <returns>size</returns>
    public DataSize Size => DataSize.Bytes((uint)Count * 4U); // 4 bytes per word

    private List<IBytecodeInstruction> list = new List<IBytecodeInstruction>();

    public void Encode(IBytecodeInstruction instruction) => list.Add(instruction);
    public void EncodeAll(IEnumerable<IBytecodeInstruction> instructions) => list.AddRange(instructions);

    /// <summary>
    /// Copy all instructions from this program and emit them to another writer
    /// </summary>
    /// <param name="writer">writer to write to</param>
    public void CopyTo(IBytecodeWriter writer) {
        foreach (var i in this.list)
            writer.Encode(i);
    }

    public override IEnumerator<IBytecodeInstruction> GetEnumerator() => list.GetEnumerator();
}
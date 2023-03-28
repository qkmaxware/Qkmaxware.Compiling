namespace Qkmaxware.Compiling.Mips;

/// <summary>
/// Readonly array 
/// </summary>
public class ReadOnlyArray<T> {
    private T[] values;

    public T this[int index] {
        get {
            return values[index];
        }
    }

    public ReadOnlyArray(T[] values) {
        this.values = values;
    }

    public int IndexOf(T value) {
        return System.Array.IndexOf(this.values, value);
    }
    public bool Contains(T value) {
        return System.Array.IndexOf(this.values, value) != -1;
    }
}
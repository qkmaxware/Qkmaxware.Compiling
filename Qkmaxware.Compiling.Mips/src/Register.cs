namespace Qkmaxware.Compiling.Mips;

/// <summary>
/// Base class for 32bit CPU registers
/// </summary>
public abstract class Register<T> {
    public abstract T Read();
    public abstract void Write(T value);
    public abstract void Reset();
}

/// <summary>
/// A register that can be read from or written to
/// </summary>
public class RwRegister<T> : Register<T> where T:struct {
    private T value;

    public override T Read() => this.value;
    public override void Write(T value) => this.value = value;
    public override void Reset() => this.value = default(T);
}

/// <summary>
/// A register that always returns a constant value
/// </summary>
public class ConstRegister<T> : Register<T> {
    private T value {get; init;}
    public ConstRegister(T value) {
        this.value = value;
    }
    public override T Read() => this.value;
    public override void Write(T value) {}

    public override void Reset() {}
}

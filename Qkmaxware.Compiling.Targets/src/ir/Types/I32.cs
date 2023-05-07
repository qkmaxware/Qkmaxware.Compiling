namespace Qkmaxware.Compiling.Targets.Ir.TypeSystem;

public abstract partial class IrType {
    /// <summary>
    /// Registration of a 32bit integer
    /// </summary>
    /// <returns>type</returns>
    public static readonly I32 I32 = new I32();
}

/// <summary>
/// 32 bit signed integer
/// </summary>
public sealed class I32 : IrType<int> {
    public override uint BitCount() => 32;
    public override int MaxValue() => int.MaxValue;
    public override int MinValue() => int.MinValue;
    public override int DefaultValue() => default(int);

    /// <summary>
    /// Internal constructor to prevent others from instancing this class
    /// </summary>
    internal I32() {}

    /// <summary>
    /// List all types that values of this type can be converted into
    /// </summary>
    /// <returns>list of valid conversions</returns>
    public override IEnumerable<TypeConversion> EnumerateConversions() {
        yield return new I32ToF32();
        yield return new I32ToU32();
    }
}
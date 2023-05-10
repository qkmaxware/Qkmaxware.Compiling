namespace Qkmaxware.Compiling.Targets.Ir.TypeSystem;

public abstract partial class IrType {
    /// <summary>
    /// Registration of a 32bit integer
    /// </summary>
    /// <returns>type</returns>
    public static readonly U32 U32 = new U32();
}

/// <summary>
/// 32 bit unsigned integer
/// </summary>
public sealed class U32 : IrType<uint> {
    public override uint BitCount() => 32;
    public override uint MaxValue() => uint.MaxValue;
    public override uint MinValue() => uint.MinValue;
    public override uint DefaultValue() => default(uint);

    /// <summary>
    /// Internal constructor to prevent others from instancing this class
    /// </summary>
    internal U32() {}

    /// <summary>
    /// Visit this type
    /// </summary>
    /// <param name="visitor">visitor visiting this type</param>
    public override void Visit(IIrTypeVisitor visitor) => visitor.Accept(this);

    /// <summary>
    /// List all types that values of this type can be converted into
    /// </summary>
    /// <returns>list of valid conversions</returns>
    public override IEnumerable<TypeConversion> EnumerateConversions() {
        yield return new U32ToF32();
        yield return new U32ToI32();
        yield return new U32ToU1();
    }
}
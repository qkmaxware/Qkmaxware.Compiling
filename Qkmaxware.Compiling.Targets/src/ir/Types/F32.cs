namespace Qkmaxware.Compiling.Ir.TypeSystem;

public abstract partial class IrType {
    /// <summary>
    /// Registration of a 32bit float
    /// </summary>
    /// <returns>type</returns>
    public static readonly F32 F32 = new F32();
}

/// <summary>
/// 32 bit unsigned integer
/// </summary>
public sealed class F32 : IrType<float> {
    public override uint BitCount() => 32;
    public override float MaxValue() => float.MaxValue;
    public override float MinValue() => float.MinValue;
    public override float DefaultValue() => default(float);

    /// <summary>
    /// Internal constructor to prevent others from instancing this class
    /// </summary>
    internal F32() {}

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
        yield return new F32ToI32();
        yield return new F32ToU32();
    }
}
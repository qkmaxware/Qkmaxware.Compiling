namespace Qkmaxware.Compiling.Targets.Ir.TypeSystem;

public abstract partial class IrType {
    /// <summary>
    /// Registration of a single bit integer
    /// </summary>
    /// <returns>type</returns>
    public static readonly U1 U1 = new U1();
}

/// <summary>
/// 32 bit unsigned integer
/// </summary>
public sealed class U1 : IrType<uint> {
    public override uint BitCount() => 1;
    public override uint MaxValue() => 1U;
    public override uint MinValue() => 0U;
    public override uint DefaultValue() => default(uint);

    /// <summary>
    /// Internal constructor to prevent others from instancing this class
    /// </summary>
    internal U1() {}
    
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
        yield return new U1ToU32();
    }
}
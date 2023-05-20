namespace Qkmaxware.Compiling.Ir.TypeSystem;

/// <summary>
/// A type for a value within the IR
/// </summary>
public abstract partial class IrType {
    /// <summary>
    /// Minimum number of bits consumed by values of this type
    /// </summary>
    /// <returns>bit count</returns>
    public abstract uint BitCount();

    /// <summary>
    /// List all types that values of this type can be converted into
    /// </summary>
    /// <returns>list of valid conversions</returns>
    public abstract IEnumerable<TypeConversion> EnumerateConversions();

    /// <summary>
    /// Visit this type
    /// </summary>
    /// <param name="visitor">visitor visiting this type</param>
    public abstract void Visit(IIrTypeVisitor visitor);
}

/// <summary>
/// A type for a value within the IR based on an underlying C# type
/// </summary>
public abstract class IrType<TUnderlying> : IrType {
    /// <summary>
    /// Maximum value that can be represented by this type
    /// </summary>
    /// <returns>value</returns>
    public abstract TUnderlying MaxValue();
    /// <summary>
    /// Minimum value that can be represented by this type
    /// </summary>
    /// <returns>value</returns>
    public abstract TUnderlying MinValue();
    /// <summary>
    /// Default value for this type before assignments are done
    /// </summary>
    /// <returns>value</returns>
    public abstract TUnderlying DefaultValue();
}

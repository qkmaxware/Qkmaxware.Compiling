using Qkmaxware.Compiling.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// Variable declaration
/// </summary>
public abstract partial class Declaration : ValueOperand {
    /// <summary>
    /// Variable index within local or global scope
    /// </summary>
    /// <value>index</value>
    public uint VariableIndex {get; private set;}
    /// <summary>
    /// Name of the declared variable
    /// </summary>
    /// <value>name</value>
    public string Name {get; private set;}

    private IrType DeclaredType;
    /// <summary>
    /// Type of the stored value
    /// </summary>
    /// <value>type</value>
    public override IrType TypeOf() => DeclaredType;

    protected Declaration(uint index, IrType typespec, string name) {
        this.VariableIndex = index;
        this.DeclaredType = typespec;
        this.Name = name;
    }

    public override string ToString() => Name + ":" + TypeOf().GetType().Name;
}

/// <summary>
/// Global variable declaration of type T
/// </summary>
/// <typeparam name="T">type of the stored value</typeparam>
public partial class Global : Declaration {
    public Global(uint index, IrType type, string name) : base(index, type, name) {}

    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public override string PrintString() {
        return $"global '{this.Name.UnicodeEscape()}'";
    }
}

/// <summary>
/// Procedure local declaration of type T
/// </summary>
/// <typeparam name="T">type of the stored value</typeparam>
public partial class Local : Declaration {
    public bool IsArgument {get; private set;}

    public Local(uint index, IrType type, string name) : base(index, type, name) {}

    internal void SetAsArgument(bool isArgument) {
        this.IsArgument = isArgument;
    }

    /// <summary>
    /// Render this tuple to string
    /// </summary>
    /// <returns>string</returns>
    public override string PrintString() {
        return $"local '{this.Name.UnicodeEscape()}'";
    }
}
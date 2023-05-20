using Qkmaxware.Compiling.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Ir;

/// <summary>
/// A subroutine composed on IR tuples
/// </summary>
public class Subprogram {
    /// <summary>
    /// The index of this subprogram within the compilation module
    /// </summary>
    /// <value>index</value>
    public uint ProcedureIndex {get; private set;}
    
    /// <summary>
    /// Optional alias for a subprogram
    /// </summary>
    /// <value>optional name</value>
    public string Name {get; set;}

    /// <summary>
    /// Create a new subprogram with the given index within the compilation module
    /// </summary>
    /// <param name="index">compilation module subprogram index</param>
    public Subprogram(uint index, string name) : this(index, name, new BasicBlock()) {}

    /// <summary>
    /// Create a new subprogram with the given index and code blocks.
    /// </summary>
    /// <param name="index">compilation module subprogram index</param>
    /// <param name="entry">entrypoint code</param>
    /// <param name="exit">exit code</param>
    public Subprogram(uint index, string name, BasicBlock entry) {
        this.Name = name;
        this.ProcedureIndex = index;
        this.Entrypoint = entry;

        this.Locals = new ReadOnlyList<Local>(this._locals);

        this.Entrypoint.Transition = new ReturnProcedure();
    }

    private List<Local> _locals = new List<Local>();
    private Namespace _localNamespace = new Namespace();
    uint nextLocalIndex = 0U;

    /// <summary>
    /// List of local variables to this subroutine
    /// </summary>
    public ReadOnlyList<Local> Locals {get; private set;}

    /// <summary>
    /// List of all local variables that are used as subprogram arguments
    /// </summary>
    /// <typeparam name="Local"></typeparam>
    /// <returns>list of arguments</returns>
    public IEnumerable<Local> Arguments => _locals.OfType<Local>().Where(local => local.IsArgument);
    
    /// <summary>
    /// Local used as a return value from this subprogram (can be null for no return value)
    /// </summary>
    /// <value>local</value>
    public Local? ReturnLocal {get; internal set;}

    /// <summary>
    /// Checks if this subprogram acts like a function (subprogram returns a value)
    /// </summary>
    public bool IsFunction => ReturnLocal != null;

    /// <summary>
    /// Checks if this subprogram acts like a procedure (subprogram does not return a value)
    /// </summary>
    public bool IsProcedure => ReturnLocal == null;

    /// <summary>
    /// Make a new local variable for this subroutine
    /// </summary>
    /// <param name="desiredName">the desired variable name, may not end up being the actual variable name</param>
    /// <returns>reference to local variable</returns>
    public Local MakeLocal(IrType type, string desiredName = "local") {
        // Make "unique" name
        var name = _localNamespace.Declare(desiredName);
        // Create variable
        var local = new Local(nextLocalIndex++, type, name);
        _locals.Add(local);
        return local;
    }

    /// <summary>
    /// Entrypoint code block
    /// </summary>
    /// <returns>code block</returns>
    public BasicBlock Entrypoint {get; private set;}


    public override string ToString() {
        return Name ?? "Procedure_" + this.ProcedureIndex;
    }

}
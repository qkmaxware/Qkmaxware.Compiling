using Qkmaxware.Compiling.Targets.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Targets.Ir;

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
    public string? Name {get; set;}

    /// <summary>
    /// Create a new subprogram with the given index within the compilation module
    /// </summary>
    /// <param name="index">compilation module subprogram index</param>
    public Subprogram(uint index) : this(index, new BasicBlock(), new BasicBlock()) {}

    /// <summary>
    /// Create a new subprogram with the given index and code blocks.
    /// </summary>
    /// <param name="index">compilation module subprogram index</param>
    /// <param name="entry">entrypoint code</param>
    /// <param name="exit">exit code</param>
    public Subprogram(uint index, BasicBlock entry, BasicBlock exit) {
        this.ProcedureIndex = index;
        this.Entrypoint = entry;
        this.Exit = exit;

        this.Locals = new ReadOnlyList<Declaration>(this._locals);

        this.Entrypoint.Transition = new Jump(this.Exit);
        this.Exit.Transition = new ReturnProcedure();
    }

    private List<Declaration> _locals = new List<Declaration>();
    private Dictionary<string, Declaration> locals = new Dictionary<string, Declaration>();
    uint nextLocalIndex = 0U;

    /// <summary>
    /// List of local variables to this subroutine
    /// </summary>
    public ReadOnlyList<Declaration> Locals {get; private set;}
    
    /// <summary>
    /// Local used as a return value from this subprogram (can be null for no return value)
    /// </summary>
    /// <value>local</value>
    public Declaration? ReturnLocal {get; internal set;}

    /// <summary>
    /// Make a new local variable for this subroutine
    /// </summary>
    /// <param name="desiredName">the desired variable name, may not end up being the actual variable name</param>
    /// <returns>reference to local variable</returns>
    public Local MakeLocal(IrType type, string desiredName = "local") {
        // Make "unique" name
        var name = desiredName;
        int index = 0;
        while (locals.ContainsKey(name)) {
            name = desiredName + (++index);
        }
        // Create variable
        var local = new Local(nextLocalIndex++, type, name);
        _locals.Add(local);
         locals.Add(name, local);
        return local;
    }

    /// <summary>
    /// Entrypoint code block
    /// </summary>
    /// <returns>code block</returns>
    public BasicBlock Entrypoint {get; private set;}

    /// <summary>
    /// Subroutine exit block
    /// </summary>
    /// <returns>block</returns>
    public BasicBlock Exit {get; private set;}

    public override string ToString() {
        return Name ?? "Procedure_" + this.ProcedureIndex;
    }

}
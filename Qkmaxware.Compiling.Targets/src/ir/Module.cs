using System.Collections;
using Qkmaxware.Compiling.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Ir;

// http://marvin.cs.uidaho.edu/Teaching/CS445/c-Grammar.pdf
public class Module : IEnumerable<Subprogram> {
    private Namespace _globalNamespace = new Namespace();
    private List<Global> _globals = new List<Global>();
    public IEnumerable<Global> Globals => _globals.AsReadOnly();
    uint nextGlobalInd = 0U;
    public Global MakeGlobal(IrType type, string desiredName) {
        // Make "unique" name
        var name = _globalNamespace.Declare(desiredName);
        // Create variable
        var def = new Global(nextGlobalInd++, type, name);
        _globals.Add(def);
        return def;
    }

    private List<Subprogram> _subprograms = new List<Subprogram>();
    public IEnumerable<Subprogram> Subprograms => _subprograms.AsReadOnly();
    uint nextProcedureInd = 0U;

    /// <summary>
    /// Create a procedure subprogram. Procedures do work but never return any value
    /// </summary>
    /// <param name="args">list of procedure arguments</param>
    /// <returns>subprogram</returns>
    public Subprogram MakeProcedure(string name, params IrType[] args) {
        var sub = new Subprogram(nextProcedureInd++, _globalNamespace.Declare(name));
        foreach (var arg in args) {
            sub.MakeLocal(arg, "arg").SetAsArgument(true);
        }
        _subprograms.Add(sub);
        return sub;
    }

    /// <summary>
    /// Create a function subprogram. Functions return a value.
    /// </summary>
    /// <param name="returns">type of the return value</param>
    /// <param name="args">list of function arguments</param>
    /// <returns>subprogram</returns>
    public Subprogram MakeFunction(string name, IrType returns, params IrType[] args) {
        var sub = new Subprogram(nextProcedureInd++, _globalNamespace.Declare(name));
        foreach (var arg in args) {
            sub.MakeLocal(arg, "arg").SetAsArgument(true);
        }
        var ret = sub.MakeLocal(returns, "return");
        sub.ReturnLocal = ret;
        sub.Entrypoint.Transition = new ReturnFunction(ret); // Last instruction is to load the return value before the we exit the subprogram
        _subprograms.Add(sub);
        return sub;
    }

    public IEnumerator<Subprogram> GetEnumerator() => this.Subprograms.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.Subprograms.GetEnumerator();
}
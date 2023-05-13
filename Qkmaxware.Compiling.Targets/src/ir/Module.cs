using System.Collections;
using Qkmaxware.Compiling.Targets.Ir.TypeSystem;

namespace Qkmaxware.Compiling.Targets.Ir;

// http://marvin.cs.uidaho.edu/Teaching/CS445/c-Grammar.pdf

public class Module : IEnumerable<Subprogram> {
    private Dictionary<string, Declaration> _globals = new Dictionary<string, Declaration>();
    public IEnumerable<Declaration> Globals => _globals.Values;
    uint nextGlobalInd = 0U;
    public Global MakeGlobal(IrType type, string desiredName) {
        // Make "unique" name
        var name = desiredName;
        int index = 0;
        while (_globals.ContainsKey(name)) {
            name = desiredName + (++index);
        }
        // Create variable
        var def = new Global(nextGlobalInd++, type, name);
        _globals.Add(name, def);
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
    public Subprogram MakeProcedure(params IrType[] args) {
        var sub = new Subprogram(nextProcedureInd++);
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
    public Subprogram MakeFunction(IrType returns, params IrType[] args) {
        var sub = new Subprogram(nextProcedureInd++);
        foreach (var arg in args) {
            sub.MakeLocal(arg, "arg").SetAsArgument(true);
        }
        var ret = sub.MakeLocal(returns, "return");
        sub.ReturnLocal = ret;
        sub.Exit.Transition = new ReturnFunction(ret); // Last instruction is to load the return value before the we exit the subprogram
        _subprograms.Add(sub);
        return sub;
    }

    public IEnumerator<Subprogram> GetEnumerator() => this.Subprograms.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.Subprograms.GetEnumerator();
}
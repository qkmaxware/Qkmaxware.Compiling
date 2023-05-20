using System.Collections;

namespace Qkmaxware.Compiling.Ir;

public class BasicBlock : IEnumerable<Tuple> {
    public List<Tuple> Instructions {get; private set;} = new List<Tuple>();
    public IBranch Transition {get; set;} = new Exit(); // By default blocks call "exit"

    public void AddInstruction(Tuple tuple) => Instructions.Add(tuple);
    public void AddInstructions(params Tuple[] tuples) => Instructions.AddRange(tuples);
    public void AddInstructions(IEnumerable<Tuple> tuples) => Instructions.AddRange(tuples);

    public void InsertAfter(BasicBlock next) {
        var old = this.Transition;
        this.Transition = new Jump(next);
        next.Transition = old;
    }

    public IEnumerator<Tuple> GetEnumerator() => Instructions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Instructions.GetEnumerator();
}
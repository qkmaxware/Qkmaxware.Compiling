using System.Collections;

namespace Qkmaxware.Compiling.Targets.Ir;

public class BasicBlock : IEnumerable<Tuple> {
    public List<Tuple> Instructions {get; private set;} = new List<Tuple>();
    public IBranch Transition {get; set;} = new Exit(); // By default blocks call "exit"

    public IEnumerator<Tuple> GetEnumerator() => Instructions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Instructions.GetEnumerator();
}
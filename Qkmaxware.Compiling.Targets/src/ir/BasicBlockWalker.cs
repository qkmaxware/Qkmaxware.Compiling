namespace Qkmaxware.Compiling.Ir;

public abstract class BasicBlockWalker {
    public void StartWalk(BasicBlock startAt) {
        HashSet<BasicBlock> visited = new HashSet<BasicBlock>();
        Queue<BasicBlock> nextUp = new Queue<BasicBlock>();
        nextUp.Enqueue(startAt);

        BasicBlock? block;
        while (nextUp.TryDequeue(out block)) {
            Visit(block);
            if (block.Transition != null) {
                foreach (var next in block.Transition.NextBlocks()) {
                    if (!visited.Contains(next)) {
                        nextUp.Enqueue(next);   // Put this up as another block to work on
                        visited.Add(next);      // Make sure we don't iterate this block again
                    }
                }
            }
        }
    }

    public abstract void Visit(BasicBlock block);
}
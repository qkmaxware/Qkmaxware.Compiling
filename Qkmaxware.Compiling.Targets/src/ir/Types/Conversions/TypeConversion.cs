namespace Qkmaxware.Compiling.Ir.TypeSystem;

public abstract class TypeConversion {
    public abstract IrType From {get;}
    public abstract IrType To {get;}

    /// <summary>
    /// Generate hardware specific instructions for this conversion
    /// </summary>
    /// <param name="conversions">instruction-conversion mapping</param>
    public abstract void GenerateInstructions(IConversionMapping conversions);

    private class SearchNode {
        public TypeConversion? ConversionToGetHere;
        public IrType ResultType;
        public uint Distance;
        public SearchNode? Previous;

        public SearchNode(IrType resultType, TypeConversion? conv) { this.ResultType = resultType; ConversionToGetHere = conv; }
    }

    private static List<TypeConversion> backtrace(SearchNode? end) {
        List<TypeConversion> o = new List<TypeConversion>();
        while (end != null) {
            if (end.ConversionToGetHere != null) {
                o.Add(end.ConversionToGetHere);
            }
            end = end.Previous;
        }
        o.Reverse();
        return o;
    }

    /// <summary>
    /// Determine the shortest set of conversions required to convert values from a specific IR type to another
    /// </summary>
    /// <param name="from">type to convert from</param>
    /// <param name="to">type to convert to</param>
    /// <returns>list of conversions to the required type</returns>
    public static List<TypeConversion> EnumerateConversions(IrType from, IrType to) {
        // Already at the end
        if (from == to) {
            return new List<TypeConversion>();
        }

        // Otherwise search for a path to the end  (Dijkstra's Shortest Path)
        Dictionary<IrType, SearchNode> nodes = new Dictionary<IrType, SearchNode>();
        PriorityQueue<SearchNode, uint> options = new PriorityQueue<SearchNode, uint>();
        options.Enqueue(new SearchNode(from, null), 0); // Starting conversion (no initial conversion)

        SearchNode? current; uint currentPriority;
        while (options.TryDequeue(out current, out currentPriority)) {
            if (current.ResultType == to) {
                // We are done!!!!
                return backtrace(current);
            }

            foreach (var conversion in current.ResultType.EnumerateConversions()) {
                if (nodes.ContainsKey(conversion.To)) {
                    // Node does exist, see if this path is better
                    var prev_best = nodes[conversion.To];
                    var next = current.Distance + 1;
                    if (next < prev_best.Distance) {
                        // It is better, replace it
                        prev_best.Distance = next;
                        prev_best.Previous = current;
                    }
                } else {
                    // Node doesn't exit, this is the best path (as it is the only path)
                    var node = new SearchNode(conversion.To, conversion){
                        Distance = current.Distance + 1,
                        Previous = current
                    };
                    nodes.Add(conversion.To, node);
                    options.Enqueue(node, node.Distance);
                }
            }
        }

        throw new ArgumentException($"No valid sequence of conversions found between '{from}' to '{to}'");
    }
}


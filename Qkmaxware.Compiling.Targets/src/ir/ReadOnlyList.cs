using System.Collections;

namespace Qkmaxware.Compiling.Targets.Ir;

public class ReadOnlyList<T> : IEnumerable<T> {

    private List<T> underlying;

    public ReadOnlyList(List<T> @ref) {
        this.underlying = @ref;
    }

    public T this[int index] {
        get => this.underlying[index];
    } 

    public IEnumerator<T> GetEnumerator() => this.underlying.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.underlying.GetEnumerator();

}
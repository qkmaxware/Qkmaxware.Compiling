using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

/// <summary>
/// A peekable stream of tokens
/// </summary>
internal class BufferedTokenStream {

    private long _position = 0;
    public long SourcePosition => _position;
    private List<Token?> lookaheads = new List<Token?>(3);

    private IEnumerator<Token> source;

    public BufferedTokenStream(IEnumerable<Token> tokens) {
        this.source = tokens.GetEnumerator();

        ensureCapacity(2);
    }

    protected Token? MakeToken() {
        if (source.MoveNext()) {
            var current = source.Current;
            return current;
        } else {
            return null;
        }
    }

    private void ensureCapacity(int capacity) {
        while (lookaheads.Count < capacity) {
            var maybeChar = MakeToken();
            if (maybeChar == null) {
                lookaheads.Add(null);
            } else {
                lookaheads.Add((Token)maybeChar);
            }
        }
    }

    /// <summary>
    /// Check to see if the stream can be advanced
    /// </summary>
    /// <returns>true if the stream has more elements to advance to</returns>
    public bool HasNext() => this.lookaheads.Count > 0 && this.lookaheads[0] != null;

    public Token? Advance() {
        var prev_first = lookaheads[0];

        for (var i = 1; i < this.lookaheads.Count; i++) {
            this.lookaheads[i - 1] = this.lookaheads[i];
        }
        this.lookaheads[lookaheads.Count - 1] = MakeToken();

        var first = this.lookaheads[0];
        if (first != null) {
            this._position = first.Position;
        }

        return prev_first;
    }

    public Token? Peek(int offset) {
        ensureCapacity(offset + 1);
        var x = this.lookaheads[offset];
        return x;
    }

    public bool IsLookahead<T>(int offset) {
        var tok = Peek(offset);
        if (tok == null)
            return false;
        return tok is T;
    }
}
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Qkmaxware.Compiling.Mips.Assembly;

public abstract class Token {
    public long Position {get; private set;}

    public Token(long Position) {
        this.Position = Position;
    }
}

public abstract class Token<T> : Token {
    public T Value {get; private set;}
    
    public Token(long pos, T value) : base(pos) {
        this.Value = value;
    }
}
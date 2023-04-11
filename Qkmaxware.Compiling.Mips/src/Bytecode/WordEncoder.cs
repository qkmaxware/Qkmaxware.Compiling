namespace Qkmaxware.Compiling.Mips.Bytecode;

public class WordEncoder {
    public uint Encoded {get; private set;}

    public WordEncoder() {}
    public WordEncoder(uint state) {
        this.Encoded = state;
    }

    public void Clear() {
        this.Encoded = default(uint);
    }

    public WordEncoder Encode(uint value, Range bits) {
        // Create bitmask
        var mask = 0U;
        var (start, length) = bits.GetOffsetAndLength(32);
        for (var i = 0; i < length; i++) {
            mask <<= 1;
            mask |= 1;
        }

        // Clip using bitmask
        value = value & mask;

        // Move over the given number of bits and write them
        this.Encoded |= value << start;

        return this;
    }

    public Bit[] ToBitArray() {
        var array = new Bit[32];
        var mask = 1;
        for (var i = 0; i < array.Length; i++) {
            if ((this.Encoded & mask) != 0) {
                array[array.Length - 1 - i] = Bit.One;
            } else {
                array[array.Length - 1 - i] = Bit.Zero;
            }
            mask <<= 1;
        }
        return array;
    }

    public uint Decode(Range bits) {
        var mask = 0U;
        var (start, length) = bits.GetOffsetAndLength(32);
        for (var i = 0; i < length; i++) {
            mask <<= 1;
            mask |= 1;
        }

        return (this.Encoded >> start) & mask;
    }

    public override string ToString() {
        return Convert.ToString(this.Encoded, 2).PadLeft(32, '0');
    }
}
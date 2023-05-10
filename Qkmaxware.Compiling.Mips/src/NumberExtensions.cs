namespace Qkmaxware.Compiling.Targets.Mips;

public static class NumberExtensions {
    public static uint ReinterpretUint(this int i) {
        return BitConverter.ToUInt32(BitConverter.GetBytes(i));
    }
    public static uint ReinterpretUint(this float i) {
        return BitConverter.ToUInt32(BitConverter.GetBytes(i));
    }
    public static uint SignExtend(this uint pattern, int sign_bit) {
        var sign_bit_mask = 1 << sign_bit;      // eg: 00001000
        var mask = 0U;                          // eg: 00001111
        for (var i = 0; i < sign_bit; i++) {
            mask = (mask | 1U) << 1;
        }
        var result = 0U;
        if ((pattern & sign_bit_mask) != 0) {
            // Sign bit is 1, extend 1 to the left of the sign bit
            result = pattern | (~mask);         // eg: 00001000 | 11110000 = 11111000
        } else {
            // Sign bit is 0, extend 0 to the left of the sign bit
            result = pattern & mask;            // eg: 00001000 * 11110000 = 00001000
        }
        return result;
    } 

    public static ushort HighHalf(this uint value) {
        return (ushort)(value >> 16);
    }

    public static uint ClearHighHalf(this uint value) {
        return value & (0b00000000_00000000_11111111_11111111U);
    }

    public static byte LowByte(this uint value) {
        return (byte)(value & 0b11111111);  
    }

    public static ushort LowHalf(this uint value) {
        return (ushort)(value & 0b11111111_11111111U);
    }
    public static uint ClearLowHalf(this uint value) {
        return value & (~(0b00000000_00000000_11111111_11111111U));
    }
}
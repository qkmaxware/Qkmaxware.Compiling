using System;

namespace Qkmaxware.Compiling.Mips;

/// <summary>
/// Abstraction of an index for a register
/// </summary>
public struct RegisterIndex {
    private int val;

    public RegisterIndex(int ind) {
        this.val = Math.Max(0, Math.Min(31, ind));
    }

    public override string ToString() => "$" + this.val.ToString();

    public static explicit operator RegisterIndex (int index) {
        return new RegisterIndex(index);
    }
    public static explicit operator RegisterIndex (uint index) {
        return new RegisterIndex((int)index);
    }

    public static implicit operator int (RegisterIndex ind) {
        return ind.val;
    }

    public static explicit operator uint (RegisterIndex ind) {
        return (uint)ind.val;
    }

    public static RegisterIndex NamedOrThrow(string s) {
        var r = Named(s);
        if (!r.HasValue)
            throw new ArgumentException("No MIPS register with name " + s);
        return r.Value;
    }

    public static RegisterIndex? Named(string s) {
        switch (s) {
            // Indexed
            case "0":
                return new RegisterIndex(0);
                
            case "1":
                return new RegisterIndex(1);
                
            case "2":
                return new RegisterIndex(2);
                
            case "3":
                return new RegisterIndex(3);
                
            case "4":
                return new RegisterIndex(4);
                
            case "5":
                return new RegisterIndex(5);
                
            case "6":
                return new RegisterIndex(6);
                
            case "7":
                return new RegisterIndex(7);
                
            case "8":
                return new RegisterIndex(8);
                
            case "9":
                return new RegisterIndex(9);
                
            case "10":
                return new RegisterIndex(10);
                
            case "11":
                return new RegisterIndex(11);
                
            case "12":
                return new RegisterIndex(12);
                
            case "13":
                return new RegisterIndex(13);
                
            case "14":
                return new RegisterIndex(14);
                
            case "15":
                return new RegisterIndex(15);
                
            case "16":
                return new RegisterIndex(16);
                
            case "17":
                return new RegisterIndex(17);
                
            case "18":
                return new RegisterIndex(18);
                
            case "19":
                return new RegisterIndex(19);
                
            case "20":
                return new RegisterIndex(20);
                
            case "21":
                return new RegisterIndex(21);
                
            case "22":
                return new RegisterIndex(22);
                
            case "23":
                return new RegisterIndex(23);
                
            case "24":
                return new RegisterIndex(24);
                
            case "25":
                return new RegisterIndex(25);
                
            case "26":
                return new RegisterIndex(26);
                
            case "27":
                return new RegisterIndex(27);
                
            case "28":
                return new RegisterIndex(28);
                
            case "29":
                return new RegisterIndex(29);
                
            case "30":
                return new RegisterIndex(30);
                
            case "31":
                return new RegisterIndex(31);
                
            // Named
            case "zero":
                return new RegisterIndex(0);
                
            case "at":
                return new RegisterIndex(1);
                
            case "v0":
                return new RegisterIndex(2);
                
            case "v1":
                return new RegisterIndex(3);
                
            case "a0":
                return new RegisterIndex(4);
                
            case "a1":
                return new RegisterIndex(5);
                
            case "a2":
                return new RegisterIndex(6);
                
            case "a3":
                return new RegisterIndex(7);
                
            case "t0":
                return new RegisterIndex(8);
                
            case "t1":
                return new RegisterIndex(9);
                
            case "t2":
                return new RegisterIndex(10);
                
            case "t3":
                return new RegisterIndex(11);
                
            case "t4":
                return new RegisterIndex(12);
                
            case "t5":
                return new RegisterIndex(13);
                
            case "t6":
                return new RegisterIndex(14);
                
            case "t7":
                return new RegisterIndex(15);
                
            case "s0":
                return new RegisterIndex(16);
                
            case "s1":
                return new RegisterIndex(17);
                
            case "s2":
                return new RegisterIndex(18);
                
            case "s3":
                return new RegisterIndex(19);
                
            case "s4":
                return new RegisterIndex(20);
                
            case "s5":
                return new RegisterIndex(21);
                
            case "s6":
                return new RegisterIndex(22);
                
            case "s7":
                return new RegisterIndex(23);
                
            case "t8":
                return new RegisterIndex(24);
                
            case "t9":
                return new RegisterIndex(25);
                
            case "k0":
                return new RegisterIndex(26);
                
            case "k1":
                return new RegisterIndex(27);
                
            case "gp":
                return new RegisterIndex(28);
                
            case "sp":
                return new RegisterIndex(29);
                
            case "fp":
                return new RegisterIndex(30);
                
            case "s8":
                return new RegisterIndex(30);
                
            case "ra":
                return new RegisterIndex(31);
                
            // IDK
            default:
                return null;
        }
    }
}
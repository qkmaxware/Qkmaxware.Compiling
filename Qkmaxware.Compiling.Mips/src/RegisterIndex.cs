using System;

namespace Qkmaxware.Compiling.Targets.Mips;

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

    public override bool Equals(object? obj) {
        if (obj == null || GetType() != obj.GetType()) {
            return false;
        }
        return ((RegisterIndex)obj).val == this.val;
    }

    public override int GetHashCode() {
        return this.val.GetHashCode();
    }

    public static RegisterIndex NamedOrThrow(string s) {
        var r = Named(s);
        if (!r.HasValue)
            throw new ArgumentException("No MIPS register with name " + s);
        return r.Value;
    }

    public static readonly RegisterIndex Zero = new RegisterIndex(0);
    public static readonly RegisterIndex At = new RegisterIndex(1);
    public static readonly RegisterIndex V0 = new RegisterIndex(2);
    public static readonly RegisterIndex V1 = new RegisterIndex(3);
    public static readonly RegisterIndex A0 = new RegisterIndex(4);
    public static readonly RegisterIndex A1 = new RegisterIndex(5);
    public static readonly RegisterIndex A2 = new RegisterIndex(6);
    public static readonly RegisterIndex A3 = new RegisterIndex(7);
    public static readonly RegisterIndex T0 = new RegisterIndex(8);
    public static readonly RegisterIndex T1 = new RegisterIndex(9);
    public static readonly RegisterIndex T2 = new RegisterIndex(10);
    public static readonly RegisterIndex T3 = new RegisterIndex(11);
    public static readonly RegisterIndex T4 = new RegisterIndex(12);
    public static readonly RegisterIndex T5 = new RegisterIndex(13);
    public static readonly RegisterIndex T6 = new RegisterIndex(14);
    public static readonly RegisterIndex T7 = new RegisterIndex(15);
    public static readonly RegisterIndex S0 = new RegisterIndex(16);
    public static readonly RegisterIndex S1 = new RegisterIndex(17);
    public static readonly RegisterIndex S2 = new RegisterIndex(18);
    public static readonly RegisterIndex S3 = new RegisterIndex(19);
    public static readonly RegisterIndex S4 = new RegisterIndex(20);
    public static readonly RegisterIndex S5 = new RegisterIndex(21);
    public static readonly RegisterIndex S6 = new RegisterIndex(22);
    public static readonly RegisterIndex S7 = new RegisterIndex(23);
    public static readonly RegisterIndex T8 = new RegisterIndex(24);
    public static readonly RegisterIndex T9 = new RegisterIndex(25);
    public static readonly RegisterIndex K0 = new RegisterIndex(26);
    public static readonly RegisterIndex K1 = new RegisterIndex(27);
    public static readonly RegisterIndex GP = new RegisterIndex(28);
    public static readonly RegisterIndex SP = new RegisterIndex(29);
    public static readonly RegisterIndex FP = new RegisterIndex(30);
    public static readonly RegisterIndex S8 = new RegisterIndex(30);
    public static readonly RegisterIndex RA = new RegisterIndex(31);

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
                
            // Named FPU registers
            case "f0":
                return new RegisterIndex(0);
                
            case "f1":
                return new RegisterIndex(1);
                
            case "f2":
                return new RegisterIndex(2);
                
            case "f3":
                return new RegisterIndex(3);
                
            case "f4":
                return new RegisterIndex(4);
                
            case "f5":
                return new RegisterIndex(5);
                
            case "f6":
                return new RegisterIndex(6);
                
            case "f7":
                return new RegisterIndex(7);
                
            case "f8":
                return new RegisterIndex(8);
                
            case "f9":
                return new RegisterIndex(9);
                
            case "f10":
                return new RegisterIndex(10);
                
            case "f11":
                return new RegisterIndex(11);
                
            case "f12":
                return new RegisterIndex(12);
                
            case "f13":
                return new RegisterIndex(13);
                
            case "f14":
                return new RegisterIndex(14);
                
            case "f15":
                return new RegisterIndex(15);
                
            case "f16":
                return new RegisterIndex(16);
                
            case "f17":
                return new RegisterIndex(17);
                
            case "f18":
                return new RegisterIndex(18);
                
            case "f19":
                return new RegisterIndex(19);
                
            case "f20":
                return new RegisterIndex(20);
                
            case "f21":
                return new RegisterIndex(21);
                
            case "f22":
                return new RegisterIndex(22);
                
            case "f23":
                return new RegisterIndex(23);
                
            case "f24":
                return new RegisterIndex(24);
                
            case "f25":
                return new RegisterIndex(25);
                
            case "f26":
                return new RegisterIndex(26);
                
            case "f27":
                return new RegisterIndex(27);
                
            case "f28":
                return new RegisterIndex(28);
                
            case "f29":
                return new RegisterIndex(29);
                
            case "f30":
                return new RegisterIndex(30);
                
            case "f31":
                return new RegisterIndex(31);

            // IDK
            default:
                return null;
        }
    }
}
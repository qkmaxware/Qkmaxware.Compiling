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
}
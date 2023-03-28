using System;

namespace Qkmaxware.Compiling.Mips;

public struct RegisterIndex {
    private int val;

    public RegisterIndex(int ind) {
        this.val = Math.Max(0, Math.Min(31, ind));
    }

    public override string ToString() => "$" + this.val.ToString();

    public static explicit operator RegisterIndex (int index) {
        return new RegisterIndex(index);
    }

    public static implicit operator int (RegisterIndex ind) {
        return ind.val;
    }
}
namespace Qkmaxware.Compiling.Mips;

/// <summary>
/// Extension methods used to allow easier interaction with registers
/// </summary>
public static class RegisterExtensions {
    #region 32bit integer registers
    public static uint ReadAsUInt32(this Register<uint> reg) {
        return reg.Read();
    }
    public static uint ReadAsUInt32(this Register<float> reg) {
        return BitConverter.ToUInt32(BitConverter.GetBytes(reg.Read()));
    }

    public static int ReadAsInt32(this Register<uint> reg) {
        return BitConverter.ToInt32(BitConverter.GetBytes(reg.Read()));
    }

    public static bool ReadAsBool(this Register<uint> reg) {
        return reg.Read() != 0;
    }

    public static void WriteUInt32(this Register<uint> reg, uint value) {
        reg.Write(value);
    }

    public static void WriteUInt32(this Register<float> reg, uint value) {
        reg.Write(BitConverter.ToSingle(BitConverter.GetBytes(value)));
    }
    public static void ConvertUInt32(this Register<float> reg, uint value) {
        reg.Write((float)value);
    }

    public static void WriteInt32(this Register<uint> reg, int value) {
        reg.Write(BitConverter.ToUInt32(BitConverter.GetBytes(value)));
    }

    public static void WriteBool(this Register<uint> reg, bool value) {
        reg.Write(value ? 1U : 0U);
    }
    #endregion
}
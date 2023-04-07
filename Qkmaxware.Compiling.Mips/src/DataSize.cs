namespace Qkmaxware.Compiling.Mips;

/// <summary>
/// Data size
/// </summary>
public struct DataSize {
    private uint byte_count;

    public uint TotalBytes => byte_count;

    private DataSize(uint bytes) {
        this.byte_count = bytes;
    }

    public static DataSize Bytes(uint bytes) {
        return new DataSize(bytes);
    }

    public static DataSize MegaBytes(uint mb) {
        return Bytes(1000000 * mb);
    }

    public static DataSize MebiBytes(uint mb) {
        return Bytes(1_048_576 * mb);
    }

    public static DataSize GigaBytes(uint gb) {
        return Bytes(1000000000 * gb);
    }
    public static DataSize GibiBytes(uint gb) {
        return Bytes(1073741824 * gb);
    }
}
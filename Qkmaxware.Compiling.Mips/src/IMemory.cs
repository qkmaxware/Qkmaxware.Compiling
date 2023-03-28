namespace Qkmaxware.Compiling.Mips;

public interface IMemory {
    public uint LoadWord(uint address);
    public void StoreWord(uint address, uint value);
}

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

public class ByteArrayMemory : IMemory {
    private byte[] bytes;

    public DataSize Size {get; private set;}

    public ByteArrayMemory(DataSize size) {
        this.Size = size;
        this.bytes = new byte[size.TotalBytes]; 
    }

    public uint LoadWord(uint address) {
        var bs = new byte[] {
            bytes[address + 0],
            bytes[address + 1],
            bytes[address + 2],
            bytes[address + 3],
        };

        return System.BitConverter.ToUInt32(bs);
    }
    public void StoreWord(uint address, uint value) {
        var bytes = System.BitConverter.GetBytes(value);
        this.bytes[address + 0] = bytes[0];
        this.bytes[address + 1] = bytes[1];
        this.bytes[address + 2] = bytes[2];
        this.bytes[address + 3] = bytes[3];
    }
}
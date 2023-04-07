namespace Qkmaxware.Compiling.Mips.Hardware;

/// <summary>
/// Linear byte addressable memory
/// </summary>
public class ByteArrayMemory : IMemory {
    private byte[] bytes;

    public DataSize Size {get; private set;}

    public ByteArrayMemory(DataSize size) {
        this.Size = size;
        this.bytes = new byte[size.TotalBytes]; 
    }

    public byte LoadByte(uint address) {
        return this.bytes[address + 0];
    }

    public ushort LoadHalf(uint address) {
        var bs = new byte[] {
            bytes[address + 0],
            bytes[address + 1]
        };

        return System.BitConverter.ToUInt16(bs);
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


    public void StoreByte(uint address, byte value) {
        this.bytes[address + 0] = value;
    }
    public void StoreHalf(uint address, ushort value) {
        var bytes = System.BitConverter.GetBytes(value);
        this.bytes[address + 0] = bytes[0];
        this.bytes[address + 1] = bytes[1];
    }
    public void StoreWord(uint address, uint value) {
        var bytes = System.BitConverter.GetBytes(value);
        this.bytes[address + 0] = bytes[0];
        this.bytes[address + 1] = bytes[1];
        this.bytes[address + 2] = bytes[2];
        this.bytes[address + 3] = bytes[3];
    }
}
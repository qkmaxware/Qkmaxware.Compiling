namespace Qkmaxware.Compiling.Mips.Hardware;

/// <summary>
/// Generic memory interface
/// </summary>
public interface IMemory {
    public byte LoadByte(uint address);
    public ushort LoadHalf(uint address);
    public uint LoadWord(uint address);

    public void StoreByte(uint address, byte value);
    public void StoreHalf(uint address, ushort value);
    public void StoreWord(uint address, uint value);
}


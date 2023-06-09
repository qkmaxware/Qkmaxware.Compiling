namespace Qkmaxware.Compiling.Targets.Mips.Hardware;

/// <summary>
/// Register set for the MIPS 32 CPU
/// </summary>
public class CpuRegisterSet {
    private Register<uint>[] registers = new Register<uint>[]{
        new ConstRegister<uint>(0),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
        new RwRegister<uint>(),
    };
    public Register<uint> this[int index] {
        get {
            return this.registers[index];
        }
    }
    public Register<uint> Zero => registers[0];
    public Register<uint> At => registers[1];
    public Register<uint> V0 => registers[2];
    public Register<uint> V1 => registers[3];
    public Register<uint> A0 => registers[4];
    public Register<uint> A1 => registers[5];
    public Register<uint> A2 => registers[6];
    public Register<uint> A3 => registers[7];
    public Register<uint> T0 => registers[8];
    public Register<uint> T1 => registers[9];
    public Register<uint> T2 => registers[10];
    public Register<uint> T3 => registers[11];
    public Register<uint> T4 => registers[12];
    public Register<uint> T5 => registers[13];
    public Register<uint> T6 => registers[14];
    public Register<uint> T7 => registers[15];
    public Register<uint> S0 => registers[16];
    public Register<uint> S1 => registers[17];
    public Register<uint> S2 => registers[18];
    public Register<uint> S3 => registers[19];
    public Register<uint> S4 => registers[20];
    public Register<uint> S5 => registers[21];
    public Register<uint> S6 => registers[22];
    public Register<uint> S7 => registers[23];
    public Register<uint> T8 => registers[24];
    public Register<uint> T9 => registers[25];
    public Register<uint> K0 => registers[26];
    public Register<uint> K1 => registers[27];
    public Register<uint> GP => registers[28];
    public Register<uint> SP => registers[29];
    public Register<uint> FP => registers[30];
    public Register<uint> S8 => registers[30];
    public Register<uint> RA => registers[31];

    public Register<uint> HI {get; private set;} = new RwRegister<uint>();
    public Register<uint> LO {get; private set;} = new RwRegister<uint>();
}

/// <summary>
/// MIPS 32 CPU
/// </summary>
public class Cpu {
    public int PC;
    public CpuRegisterSet Registers {get; private set;} = new CpuRegisterSet();

    public override string ToString() {
        return $"PC: {PC} HILO: {this.Registers.HI.Read():X4}{this.Registers.LO.Read():X4} REG(s): {this.Registers[0].Read():X4},{this.Registers[1].Read():X4},{this.Registers[2].Read():X4},{this.Registers[3].Read():X4},{this.Registers[4].Read():X4},{this.Registers[5].Read():X4},{this.Registers[6].Read():X4},{this.Registers[7].Read():X4},{this.Registers[8].Read():X4},{this.Registers[9].Read():X4},{this.Registers[10].Read():X4},,{this.Registers[11].Read():X4},{this.Registers[12].Read():X4},{this.Registers[13].Read():X4},{this.Registers[14].Read():X4},{this.Registers[15].Read():X4},{this.Registers[16].Read():X4},{this.Registers[17].Read():X4},{this.Registers[18].Read():X4},{this.Registers[19].Read():X4},{this.Registers[20].Read():X4},{this.Registers[21].Read():X4},{this.Registers[22].Read():X4},{this.Registers[23].Read():X4},{this.Registers[24].Read():X4},{this.Registers[25].Read():X4},{this.Registers[26].Read():X4},{this.Registers[27].Read():X4},{this.Registers[28].Read():X4},{this.Registers[29].Read():X4},{this.Registers[30].Read():X4},{this.Registers[31].Read():X4}";
    }
}
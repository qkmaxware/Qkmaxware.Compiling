namespace Qkmaxware.Compiling.Targets.Mips.Hardware;

/// <summary>
/// Register set for the MIPS 32 FPU (Coprocessor 1)
/// </summary>
public class FpuRegisterSet {
    private Register<float>[] registers = new Register<float>[]{
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
        new RwRegister<float>(),
    };
    public Register<float> this[int index] {
        get {
            return this.registers[index];
        }
    }
}

/// <summary>
/// MIPS 32 FPU (Coprocessor 1)
/// </summary>
public class Fpu {
    public FpuRegisterSet Registers {get; private set;} = new FpuRegisterSet();

    public override string ToString() {
        return $"REG(s): {this.Registers[0].Read()},{this.Registers[1].Read()},{this.Registers[2].Read()},{this.Registers[3].Read()},{this.Registers[4].Read()},{this.Registers[5].Read()},{this.Registers[6].Read()},{this.Registers[7].Read()},{this.Registers[8].Read()},{this.Registers[9].Read()},{this.Registers[10].Read()},,{this.Registers[11].Read()},{this.Registers[12].Read()},{this.Registers[13].Read()},{this.Registers[14].Read()},{this.Registers[15].Read()},{this.Registers[16].Read()},{this.Registers[17].Read()},{this.Registers[18].Read()},{this.Registers[19].Read()},{this.Registers[20].Read()},{this.Registers[21].Read()},{this.Registers[22].Read()},{this.Registers[23].Read()},{this.Registers[24].Read()},{this.Registers[25].Read()},{this.Registers[26].Read()},{this.Registers[27].Read()},{this.Registers[28].Read()},{this.Registers[29].Read()},{this.Registers[30].Read()},{this.Registers[31].Read()}";
    }
}
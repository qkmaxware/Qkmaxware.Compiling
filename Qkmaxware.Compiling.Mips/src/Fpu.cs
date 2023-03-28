namespace Qkmaxware.Compiling.Mips;

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
}
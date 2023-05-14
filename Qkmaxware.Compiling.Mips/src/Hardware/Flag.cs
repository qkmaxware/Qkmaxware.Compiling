namespace Qkmaxware.Compiling.Targets.Mips.Hardware;

public class Flag {
    
    public bool IsSet {get; private set;}

    public void Set() {
        this.IsSet = true;
    }

    public void Unset() {
        this.IsSet = false;
    }

    public void SetIf(bool condition) {
        this.IsSet = condition;
    }
}
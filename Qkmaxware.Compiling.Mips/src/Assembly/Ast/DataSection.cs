namespace Qkmaxware.Compiling.Mips.Assembly;

public abstract class Data {
    public LabelToken VariableName {get; private set;}
    public DirectiveToken StorageClass {get; private set;}
    public bool IsArray {get; private set;}

    public Data(LabelToken name, DirectiveToken storage, bool isArray) {
        this.VariableName = name;
        this.StorageClass = storage;
        this.IsArray = isArray;
    }
}

public class Data<T> : Data {
    public T[] Values {get; private set;}
    public Data(LabelToken name, DirectiveToken storage, T value) : base(name, storage, false) {
        this.Values = new T[] {value};
    }
    public Data(LabelToken name, DirectiveToken storage, T[] values) : base(name, storage, true) {
        this.Values = values;
    }

    public override string ToString() {
        if (this.StorageClass.Value == ("ascii") && Values is byte[] ascii) {
            return $"{VariableName} .{StorageClass.Value} \"{System.Text.Encoding.ASCII.GetString(ascii)}\"";
        } else if (this.StorageClass.Value == ("asciiz") && Values is byte[] asciiz) {
            return $"{VariableName} .{StorageClass.Value} \"{System.Text.Encoding.ASCII.GetString(asciiz.SkipLast(1).ToArray())}\"";
        } else {
            return $"{VariableName} .{StorageClass.Value} {string.Join(',', Values)}";
        }
    }
}

public class DataSection : Section {
    public List<Data> Data {get; private set;} = new List<Data>();

    public override string ToString() {
        return ".data";
    }
}
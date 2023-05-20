namespace Qkmaxware.Compiling.Ir;

public class Namespace {
    HashSet<string> names = new HashSet<string>();

    public void Clear() {
        this.names.Clear();
    }

    public void Remove(string name) {
        this.names.Remove(name);
    }

    public bool Contains(string name) {
        return names.Contains(name);
    }

    public string MakeUnique(string desiredName) {
        // Make "unique" name
        var name = desiredName;
        int index = 0;
        while (names.Contains(name)) {
            name = desiredName + (++index);
        }
        return name;
    }

    public string Declare(string desiredName) {
        var name = MakeUnique(desiredName);
        this.names.Add(name);
        return name;
    }
}
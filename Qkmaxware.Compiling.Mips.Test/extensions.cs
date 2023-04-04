using System.IO;
using System.Reflection;
using System.Linq;

public static class Extensions {
    public static string ReadEmbeddedFile(this string filename) {
        var assembly = Assembly.GetExecutingAssembly();

        foreach (var asm in assembly.GetManifestResourceNames()) {
            if (asm.EndsWith(filename)) {
                using (Stream? stream = assembly.GetManifestResourceStream(asm)) {
                    if (stream == null) {
                        continue;
                    }
                    using (StreamReader reader = new StreamReader(stream)) {
                        string result = reader.ReadToEnd();
                        return result;
                    }
                }
            }
        }
        return string.Empty;
    }

    public static string GetLine(this string text, int line) {
        return text.Split('\n').ElementAtOrDefault(line) ?? string.Empty;
    }
}
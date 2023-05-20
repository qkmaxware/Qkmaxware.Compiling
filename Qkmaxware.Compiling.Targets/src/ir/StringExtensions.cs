using System.Text;

namespace Qkmaxware.Compiling;

public static class StringExtensions {
    public static string UnicodeEscape(this string s) {
        StringBuilder writer = new StringBuilder(s.Length);
        foreach (var c in s) {
            if( c > 127 ) {
                // This character is too big for ASCII
                writer.Append(@"\u");
                writer.Append(((int) c).ToString("x4"));
            }
            else {
                writer.Append(c);
            }
        }
        return writer.ToString();
    }
}
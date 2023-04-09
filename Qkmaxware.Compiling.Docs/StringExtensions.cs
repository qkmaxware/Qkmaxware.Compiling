public static class StringExtensions {
    public static IEnumerable<string> SplitN(this string str, double n) {
        int k = 0;
        var output = str
            .ToLookup(c => Math.Floor(k++ / n))
            .Select(e => new String(e.ToArray()));
        return output;
    }
}
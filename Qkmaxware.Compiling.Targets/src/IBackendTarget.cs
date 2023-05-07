namespace Qkmaxware.Compiling.Targets;

/// <summary>
/// Interface for a backend to transform an IR module to a particular compilation target module
/// </summary>
/// <typeparam name="T">Compilation target result type</typeparam>
public interface IBackendTargetModule<T> {
    /// <summary>
    /// Try to transform the given IR module and emit the results.
    /// </summary>
    /// <param name="module">IR module to transform to compilation target</param>
    /// <param name="output">the resulting emitted target</param>
    /// <returns>the transformed module if it was emitted correctly</returns>
    public T TryTransform(Ir.Module module);
}

/// <summary>
/// Interface for a backend that can transform an IR module to a particular compilation target and emit the results to file
/// </summary>
/// <typeparam name="T">Compilation target result type</typeparam>
public interface IBackendTargetFile {
    /// <summary>
    /// Try to transform the given IR module and emit the results to a file
    /// </summary>
    /// <param name="module">IR module to transform to compilation target</param>
    /// <param name="output">the resulting emitted target</param>
    /// <returns>path to the transformed module on the file-system if it was emitted correctly</returns>
    public FileInfo TryEmitToFile(Ir.Module module, string path_like);
}
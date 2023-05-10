using System.Text.RegularExpressions;
using Qkmaxware.Compiling.Targets.Mips.Assembly;
using Qkmaxware.Compiling.Targets.Mips.Hardware;

namespace Qkmaxware.Compiling.Targets.Mips.Bytecode;

/// <summary>
/// Interface for all MIPS bytecode instructions
/// </summary>
public interface IBytecodeInstruction {
    /// <summary>
    /// Encode the operation in MIPS32 bytecode
    /// </summary>
    /// <returns>encoded operation</returns>
    public uint Encode32();

    /// <summary>
    /// Execute the instruction on simulated hardware 
    /// </summary>
    /// <param name="cpu">Simulated MIPS CPU</param>
    /// <param name="fpu">Simulated MIPS Floating-point coprocessor</param>
    /// <param name="memory">Simulator linear byte-addressable memory</param>
    public void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io);

    /// <summary>
    /// The operands used by the instruction
    /// </summary>
    /// <returns>enumerable of all operands in the order in which they are specified</returns>
    public IEnumerable<uint> GetOperands();
}

public abstract class BaseBytecodeInstruction : IBytecodeInstruction {
    /// <summary>
    /// Encode the operation in MIPS32 bytecode
    /// </summary>
    /// <returns>encoded operation</returns>
    public abstract uint Encode32();

    /// <summary>
    /// Execute the instruction on simulated hardware 
    /// </summary>
    /// <param name="cpu">Simulated MIPS CPU</param>
    /// <param name="fpu">Simulated MIPS Floating-point coprocessor</param>
    /// <param name="memory">Simulator linear byte-addressable memory</param>
    public abstract void Invoke(Cpu cpu, Fpu fpu, IMemory memory, SimulatorIO io);

    private static Regex splitCamelCase = new Regex("(?<=[A-Z])(?=[A-Z][a-z0-9])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z0-9])(?=[^A-Za-z0-9])");

    /// <summary>
    /// Name of the instruction
    /// </summary>
    /// <returns>instruction's name</returns>
    public string InstructionName() => string.Join('.', splitCamelCase.Split(this.GetType().Name).Select(x => x.ToLower()));

    /// <summary>
    /// Name of the instruction
    /// </summary>
    /// <returns>instruction's name</returns>
    public static string InstructionName<T>() where T:IBytecodeInstruction => string.Join('.', splitCamelCase.Split(typeof(T).Name).Select(x => x.ToLower()));

    /// <summary>
    /// Print this instruction as MIPS assembly code
    /// </summary>
    /// <returns>assembly string</returns>
    public abstract string ToAssemblyString();

    /// <summary>
    /// The operands used by the instruction
    /// </summary>
    /// <returns>enumerable of all operands in the order in which they are specified</returns>
    public abstract IEnumerable<uint> GetOperands();

    /// <summary>
    /// The written format of this instruction in assembly
    /// </summary>
    /// <returns>description</returns>
    public virtual string AssemblyFormat() => string.Empty;

    /// <summary>
    /// Description of this instruction
    /// </summary>
    /// <returns>description</returns>
    public virtual string InstructionDescription() => string.Empty;

    #region Assembly Formats
    protected static bool IsAssemblyFormatDest<TInstr, TDest> (
        Mips.Assembly.IdentifierToken opcode, 
        List<Mips.Assembly.Token> args, 
        out TDest dest
    ) where TInstr:IBytecodeInstruction {
        #nullable disable
        dest = default(TDest);
        if (opcode.Value != InstructionName<TInstr>()) {
            return false;
        }

        // OPCODE $dest
        if (args.Count != 1) {
            throw new AssemblyException(opcode.Position, "Missing required argument(s)");
        }
        if (args[0] is not TDest destT) {
            throw new AssemblyException(args[0].Position, "Missing destination register");
        }
        dest = destT;
        return true;
        #nullable restore
    }
    protected static bool IsAssemblyFormatArg<TInstr, TArg> (
        Mips.Assembly.IdentifierToken opcode, 
        List<Mips.Assembly.Token> args, 
        out TArg arg
    ) where TInstr:IBytecodeInstruction {
        #nullable disable
        arg = default(TArg);
        if (opcode.Value != InstructionName<TInstr>()) {
            return false;
        }

        // OPCODE $dest
        if (args.Count != 1) {
            throw new AssemblyException(opcode.Position, "Missing required argument(s)");
        }
        if (args[0] is not TArg argT) {
            throw new AssemblyException(args[0].Position, "Missing operand");
        }
        arg = argT;
        return true;
        #nullable restore
    }
    protected static bool IsAssemblyFormatDestArg<TInstr, TDest, TArg> (
        Mips.Assembly.IdentifierToken opcode, 
        List<Mips.Assembly.Token> args, 
        out TDest dest, 
        out TArg arg
    ) where TInstr:IBytecodeInstruction {
        #nullable disable
        dest = default(TDest);
        arg  = default(TArg);
        if (opcode.Value != InstructionName<TInstr>()) {
            return false;
        }

        // OPCODE $dest, $arg
        if (args.Count != 3) {
            throw new AssemblyException(opcode.Position, "Missing required argument(s)");
        }
        if (args[0] is not TDest destT) {
            throw new AssemblyException(args[0].Position, "Missing destination register");
        }
        if (args[1] is not CommaToken) {
            throw new AssemblyException(args[1].Position, "Missing comma between arguments");
        }
        if (args[2] is not TArg argT) {
            throw new AssemblyException(args[2].Position, "Missing operand");
        }
        dest = destT;
        arg = argT;
        return true;
        #nullable restore
    }
    protected static bool IsAssemblyFormatLhsRhs<TInstr, TLhs, TRhs> (
        Mips.Assembly.IdentifierToken opcode, 
        List<Mips.Assembly.Token> args, 
        out TLhs lhs, 
        out TRhs rhs
    ) where TInstr:IBytecodeInstruction {
        #nullable disable
        lhs = default(TLhs);
        rhs  = default(TRhs);
        if (opcode.Value != InstructionName<TInstr>()) {
            return false;
        }

        // OPCODE $dest, $arg
        if (args.Count != 3) {
            throw new AssemblyException(opcode.Position, "Missing required argument(s)");
        }
        if (args[0] is not TLhs destT) {
            throw new AssemblyException(args[0].Position, "Missing left-hand side operand");
        }
        if (args[1] is not CommaToken) {
            throw new AssemblyException(args[1].Position, "Missing comma between arguments");
        }
        if (args[2] is not TRhs argT) {
            throw new AssemblyException(args[2].Position, "Missing right-hand side operand");
        }
        lhs = destT;
        rhs = argT;
        return true;
        #nullable restore
    }
    protected static bool IsAssemblyFormatDestLhsRhs<TInstr, TDest, TLhs, TRhs> (
        Mips.Assembly.IdentifierToken opcode, 
        List<Mips.Assembly.Token> args, 
        out TDest dest, 
        out TLhs lhs, 
        out TRhs rhs
    ) where TInstr:IBytecodeInstruction {
        #nullable disable
        dest = default(TDest);
        lhs  = default(TLhs);
        rhs  = default(TRhs);
        if (opcode.Value != InstructionName<TInstr>()) {
            return false;
        }

        // OPCODE $dest, $lhs, $rhs
        if (args.Count != 5) {
            throw new AssemblyException(opcode.Position, "Missing required argument(s)");
        }
        if (args[0] is not TDest destT) {
            throw new AssemblyException(args[0].Position, "Missing destination register");
        }
        if (args[1] is not CommaToken) {
            throw new AssemblyException(args[1].Position, "Missing comma between arguments");
        }
        if (args[2] is not TLhs lhsT) {
            throw new AssemblyException(args[2].Position, "Missing left-hand side operand");
        }
        if (args[3] is not CommaToken) {
            throw new AssemblyException(args[3].Position, "Missing comma between arguments");
        }
        if (args[4] is not TRhs rhsT) {
            throw new AssemblyException(args[4].Position, "Missing right-hand side operand");
        }
        dest = destT;
        lhs = lhsT;
        rhs = rhsT;
        return true;
        #nullable restore
    }
    protected static bool IsAssemblyFormatLhsOffset<TInstr, TLhs, TOffset> (
        Mips.Assembly.IdentifierToken opcode, 
        List<Mips.Assembly.Token> args, 
        out TLhs lhs, 
        out TOffset offset
    ) where TInstr:IBytecodeInstruction {
        #nullable disable
        lhs  = default(TLhs);
        offset = default(TOffset);
        if (opcode.Value != InstructionName<TInstr>()) {
            return false;
        }

        // OPCODE $lhs, offset
        if (args.Count != 3) {
            throw new AssemblyException(opcode.Position, "Missing required argument(s)");
        }
        if (args[0] is not TLhs destT) {
            throw new AssemblyException(args[0].Position, "Missing operand");
        }
        if (args[1] is not CommaToken) {
            throw new AssemblyException(args[1].Position, "Missing comma between arguments");
        }
        if (args[4] is not TOffset rhsT) {
            throw new AssemblyException(args[4].Position, "Missing offset");
        }
        lhs = destT;
        offset = rhsT;
        return true;
        #nullable restore
    }
    protected static bool IsAssemblyFormatLhsRhsOffset<TInstr, TLhs, TRhs, TOffset> (
        Mips.Assembly.IdentifierToken opcode, 
        List<Mips.Assembly.Token> args, 
        out TLhs lhs, 
        out TRhs rhs, 
        out TOffset offset
    ) where TInstr:IBytecodeInstruction {
        #nullable disable
        lhs  = default(TLhs);
        rhs  = default(TRhs);
        offset = default(TOffset);
        if (opcode.Value != InstructionName<TInstr>()) {
            return false;
        }

        // OPCODE $dest, $lhs, $rhs
        if (args.Count != 5) {
            throw new AssemblyException(opcode.Position, "Missing required argument(s)");
        }
        if (args[0] is not TLhs destT) {
            throw new AssemblyException(args[0].Position, "Missing left-hand side operand");
        }
        if (args[1] is not CommaToken) {
            throw new AssemblyException(args[1].Position, "Missing comma between arguments");
        }
        if (args[2] is not TRhs lhsT) {
            throw new AssemblyException(args[2].Position, "Missing right-hand side operand");
        }
        if (args[3] is not CommaToken) {
            throw new AssemblyException(args[3].Position, "Missing comma between arguments");
        }
        if (args[4] is not TOffset rhsT) {
            throw new AssemblyException(args[4].Position, "Missing offset");
        }
        lhs = destT;
        rhs = lhsT;
        offset = rhsT;
        return true;
        #nullable restore
    }
    protected static bool IsAssemblyFormatDestOffsetBase<TInstr, TDest, TBase, TOffset> (
        Mips.Assembly.IdentifierToken opcode, 
        List<Mips.Assembly.Token> args, 
        out TDest dest, 
        out TBase @base, 
        out TOffset offset
    ) where TInstr:IBytecodeInstruction {
        #nullable disable
        dest = default(TDest);
        @base  = default(TBase);
        offset  = default(TOffset);
        if (opcode.Value != InstructionName<TInstr>()) {
            return false;
        }

        // OPCODE $dest, offset(base)
        if (args.Count != 6) {
            throw new AssemblyException(opcode.Position, "Missing required argument(s)");
        }
        if (args[0] is not TDest destT) {
            throw new AssemblyException(args[0].Position, "Missing destination register");
        }
        if (args[1] is not CommaToken) {
            throw new AssemblyException(args[1].Position, "Missing comma between arguments");
        }
        if (args[2] is not TOffset offsetT) {
            throw new AssemblyException(args[2].Position, "Missing offset value");
        }
        if (args[3] is not OpenParenthesisToken) {
            throw new AssemblyException(args[3].Position, "Missing open parenthesis");
        }
        if (args[4] is not TBase baseT) {
            throw new AssemblyException(args[4].Position, "Missing base register");
        }
        if (args[5] is not CloseParenthesisToken) {
            throw new AssemblyException(args[5].Position, "Missing close parenthesis");
        }
        dest = destT;
        @offset = offsetT;
        @base = baseT;
        return true;
        #nullable restore
    }
    protected static bool IsAssemblyFormatSourceOffsetBase<TInstr, TSrc, TBase, TOffset> (
        Mips.Assembly.IdentifierToken opcode, 
        List<Mips.Assembly.Token> args, 
        out TSrc src, 
        out TBase @base, 
        out TOffset offset
    ) where TInstr:IBytecodeInstruction {
        #nullable disable
        src = default(TSrc);
        @base  = default(TBase);
        offset  = default(TOffset);
        if (opcode.Value != InstructionName<TInstr>()) {
            return false;
        }

        // OPCODE $dest, offset(base)
        if (args.Count != 6) {
            throw new AssemblyException(opcode.Position, "Missing required argument(s)");
        }
        if (args[0] is not TSrc destT) {
            throw new AssemblyException(args[0].Position, "Missing source register");
        }
        if (args[1] is not CommaToken) {
            throw new AssemblyException(args[1].Position, "Missing comma between arguments");
        }
        if (args[2] is not TOffset offsetT) {
            throw new AssemblyException(args[2].Position, "Missing offset value");
        }
        if (args[3] is not OpenParenthesisToken) {
            throw new AssemblyException(args[3].Position, "Missing open parenthesis");
        }
        if (args[4] is not TBase baseT) {
            throw new AssemblyException(args[4].Position, "Missing base register");
        }
        if (args[5] is not CloseParenthesisToken) {
            throw new AssemblyException(args[5].Position, "Missing close parenthesis");
        }
        src = destT;
        @offset = offsetT;
        @base = baseT;
        return true;
        #nullable restore
    }
    #endregion
}
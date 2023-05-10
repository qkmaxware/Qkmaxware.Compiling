using System;
using System.Collections.Generic;
using Qkmaxware.Compiling.Targets.Mips.Assembly;
using Qkmaxware.Compiling.Targets.Mips.Bytecode;

using Qkmaxware.Compiling.Targets.Mips.Bytecode.Instructions;

namespace Qkmaxware.Compiling.Targets.Mips;

class Counter {
    public uint Count {get; private set;}
    public Counter () {}

    public void Increment() => Count++;

    public static Counter operator + (Counter lhs, uint rhs) {
        lhs.Count += rhs;
        return lhs;
    }
}


/// <summary>
/// Assembler for MIPS assembly to MIPS bytecode
/// </summary>
public class Assembler {
    /// <summary>
    /// Parse the given assembly code to bytecode instructions
    /// </summary>
    /// <param name="assembly">code to parse</param>
    /// <returns>bytecode instructions</returns>
    public Bytecode.BytecodeProgram ParseAndAssemble(string assembly) {
        using var reader = new StringReader(assembly);
        return ParseAndAssemble(reader);
    }

    /// <summary>
    /// Parse the given assembly code to bytecode instructions
    /// </summary>
    /// <param name="reader">text reader in which assembly can be read from</param>
    /// <returns>bytecode instructions</returns>
    public Bytecode.BytecodeProgram ParseAndAssemble(TextReader reader) {
        // Tokenize
        Lexer lexer = new Lexer();
        var tokens = lexer.Tokenize(reader);

        // Parse
        Parser parser = new Parser();
        var ast = parser.Parse(tokens);

        // Assemble
        return Assemble(ast);
    }

    /// <summary>
    /// Assembly the given assembly program to an in-memory bytecode representation
    /// </summary>
    /// <param name="input">assembly program</param>
    /// <returns>bytecode program</returns>
    public Bytecode.BytecodeProgram Assemble(Assembly.AssemblyProgram input) {
        var output = new InMemoryBytecodeProgram();
        var memory_start = RegisterIndex.GP;
        var env = new AssemblerEnvironment();
        Counter memory = new Counter();

        // Handle creating the data in the data section first
        foreach (var section in input.DataSections) {
            // Encode the data as instructions
            // Store the address of the instruction in the label_addresses map
            foreach (var data in section.Data) {
                if (data is Data<int> int_data) {
                    switch (int_data.StorageClass.Value) {
                        case "byte":
                            foreach (var instr in EncodeBytes(memory, env, data.VariableName.Value, int_data.Values.Select(x => (byte)x))) {
                                output.Add(instr);
                                env.IncrementInstructionCount();
                            }
                            break;
                        case "half":
                            foreach (var instr in EncodeHalf(memory, env, data.VariableName.Value, int_data.Values.Select(x => (Int16)x))) {
                                output.Add(instr);
                                env.IncrementInstructionCount();
                            }
                            break;
                        case "word":
                            foreach (var instr in EncodeWord(memory, env, data.VariableName.Value, int_data.Values.Select(x => BitConverter.ToUInt32(BitConverter.GetBytes(x))))) {
                                output.Add(instr);
                                env.IncrementInstructionCount();
                            }
                            break;
                        default:
                            throw new ArgumentException($"Unable to binary encode data of type .{int_data.StorageClass.Value}");
                    }
                } else if (data is Data<float> real_data) {
                    switch (real_data.StorageClass.Value) {
                        case "single":
                            foreach (var instr in EncodeWord(memory, env, data.VariableName.Value, real_data.Values.Select(x => BitConverter.ToUInt32(BitConverter.GetBytes(x))))) {
                                output.Add(instr);
                                env.IncrementInstructionCount();
                            }
                            break;
                        default:
                            throw new ArgumentException($"Unable to binary encode data of type .{real_data.StorageClass.Value}");
                    }
                } else if (data is Data<byte> text_data) {
                    foreach (var instr in EncodeBytes(memory, env, data.VariableName.Value, text_data.Values)) {
                        output.Add(instr);
                        env.IncrementInstructionCount();
                    }
                } else {
                    throw new ArgumentException($"Unable to binary encode data of type .{data.StorageClass.Value}");
                }
            }
        }
        // Set the GP pointer to the end of the memory data (for usage with the heap etc)
        foreach (var instr in new Assembly.Instructions.Li {
            Destination = memory_start,
            Value = new ScalarConstantToken(0, BitConverter.ToInt32(BitConverter.GetBytes(memory.Count)))
        }.Assemble(env)) {
            output.Add(instr);
            env.IncrementInstructionCount();
        }


        // Handle invoking / jumping to main method
        foreach (var section in input.GlobalSections) {
            // Add instruction to jump to main method (if it exists)
            foreach (var label in section.Labels) {
                /*var @goto = new Bytecode.J {
                    Address = label
                };
                foreach (var code in @goto.Visit(emitter)) {
                    // Add all bytecode instructions to the program
                    output.Add(code);
                    env.IncrementInstructionCount();
                }*/
            }
        }

        // Handle writing code sections
        foreach (var section in input.TextSections) {
            // Loop over all assembly instructions
            foreach (var instr in section.Code) {
                // For each assembly instruction compile it into 0 or more bytecode instructions
                // Most instructions are 1-1 mappings from Assembly Instructions to Bytecode Instructions
                // Some pseudo-instructions will return more than one bytecode instruction
                // Others will return nothing as they are placeholders
                foreach (var bytecode in instr.Assemble(env)) {
                    output.Add(bytecode);
                    env.IncrementInstructionCount();
                }
            }
        } 

        // Error checking
        if (env.HasLabelsWithoutAddresses()) {
            throw new ArgumentException($"The labels { string.Join(',', env.LabelsAwaitingAddresses()) } are not bound to a memory location. Make sure all labels are declared.");
        }
    
        return output;
    }

    private IEnumerable<IBytecodeInstruction> EncodeBytes(Counter MemoryUsed, AssemblerEnvironment env, string label, IEnumerable<byte> integers) {
        // Save address in memory for the stored data
        var address = MemoryUsed.Count;
        env.SetLabelAddress(label, address);

        // Store the data
        foreach (var element in integers) {
            // Load value
            yield return new Ori {
                Immediate = element,
                Target = RegisterIndex.At
            };

            // Store value
            yield return new Sb {
                Target = RegisterIndex.At,      // Register containing the value to save
                Source = RegisterIndex.Zero,    // Register containing the "base" address
                Immediate = MemoryUsed.Count    // Current memory pointer
            };
            
            // Increment memory counter
            MemoryUsed += 1; // 1 byte
        }
    }

    private IEnumerable<IBytecodeInstruction> EncodeHalf(Counter MemoryUsed, AssemblerEnvironment env, string label, IEnumerable<Int16> integers) {
        // Save address in memory for the stored data
        var address = MemoryUsed.Count;
        env.SetLabelAddress(label, address);

        // Store the data
        foreach (var element in integers) {
            // Load value
            yield return new Ori {
                Immediate = BitConverter.ToUInt16(BitConverter.GetBytes(element)),
                Target = RegisterIndex.At
            };

            // Store value
            yield return new Sh {
                Target = RegisterIndex.At,      // Register containing the value to save
                Source = RegisterIndex.Zero,    // Register containing the "base" address
                Immediate = MemoryUsed.Count    // Current memory pointer
            };
            
            // Increment memory counter
            MemoryUsed += 2; // 2 bytes
        }
    }

    private IEnumerable<IBytecodeInstruction> EncodeWord(Counter MemoryUsed, AssemblerEnvironment env, string label, IEnumerable<UInt32> integers) {
        // Save address in memory for the stored data
        var address = MemoryUsed.Count;
        env.SetLabelAddress(label, address);

        // Store the data
        foreach (var element in integers) {
            // Load value
            yield return new Lui {
                Immediate = element >> 16,
                Target = RegisterIndex.At
            };
            yield return new Ori {
                Immediate = element & 0b11111111_11111111,
                Target = RegisterIndex.At
            };

            // Store value
            yield return new Sw {
                Target = RegisterIndex.At,      // Register containing the value to save
                Source = RegisterIndex.Zero,    // Register containing the "base" address
                Immediate = MemoryUsed.Count    // Current memory pointer
            };
            
            // Increment memory counter
            MemoryUsed += 4; // 4 bytes
        }
    }
}

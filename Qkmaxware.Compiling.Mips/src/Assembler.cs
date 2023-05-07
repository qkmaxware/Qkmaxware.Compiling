using System;
using System.Collections.Generic;
using Qkmaxware.Compiling.Targets.Mips.Assembly;
using Qkmaxware.Compiling.Targets.Mips.Bytecode;

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

        var instructions = new Counter();
        var label_address = new Dictionary<string, uint>();
        var emitter = new Assembly2BytecodeTransformer(instructions, label_address);
        var assembler_reserved = RegisterIndex.At;
        var memory_start = RegisterIndex.GP;
        
        // Handle creating the data in the data section first
        foreach (var section in input.DataSections) {
            // Encode the data as instructions
            // Store the address of the instruction in the label_addresses map
            foreach (var data in section.Data) {
                if (data is Data<int> int_data) {
                    switch (int_data.StorageClass.Value) {
                        case "byte":
                            foreach (var instr in emitter.EncodeBytes(data.VariableName.Value, int_data.Values.Select(x => (byte)x))) {
                                output.Add(instr);
                                instructions.Increment();
                            }
                            break;
                        case "half":
                            foreach (var instr in emitter.EncodeHalf(data.VariableName.Value, int_data.Values.Select(x => (Int16)x))) {
                                output.Add(instr);
                                instructions.Increment();
                            }
                            break;
                        case "word":
                            foreach (var instr in emitter.EncodeWord(data.VariableName.Value, int_data.Values.Select(x => BitConverter.ToUInt32(BitConverter.GetBytes(x))))) {
                                output.Add(instr);
                                instructions.Increment();
                            }
                            break;
                        default:
                            throw new ArgumentException($"Unable to binary encode data of type .{int_data.StorageClass.Value}");
                    }
                } else if (data is Data<float> real_data) {
                    switch (real_data.StorageClass.Value) {
                        case "single":
                            foreach (var instr in emitter.EncodeWord(data.VariableName.Value, real_data.Values.Select(x => BitConverter.ToUInt32(BitConverter.GetBytes(x))))) {
                                output.Add(instr);
                                instructions.Increment();
                            }
                            break;
                        default:
                            throw new ArgumentException($"Unable to binary encode data of type .{real_data.StorageClass.Value}");
                    }
                } else if (data is Data<byte> text_data) {
                    foreach (var instr in emitter.EncodeBytes(data.VariableName.Value, text_data.Values)) {
                        output.Add(instr);
                        instructions.Increment();
                    }
                } else {
                    throw new ArgumentException($"Unable to binary encode data of type .{data.StorageClass.Value}");
                }
            }
        }
        // Set the GP pointer to the end of the memory data (for usage with the heap etc)
        foreach (var instr in new Assembly.LoadImmediate{
            ResultRegister = memory_start,
            Constant = emitter.MemoryUsed.Count
        }.Visit(emitter)) {
            output.Add(instr);
            instructions.Increment();
        }


        // Handle invoking / jumping to main method
        foreach (var section in input.GlobalSections) {
            // Add instruction to jump to main method (if it exists)
            foreach (var label in section.Labels) {
                var @goto = new Assembly.JumpTo {
                    Address = label
                };
                foreach (var code in @goto.Visit(emitter)) {
                    // Add all bytecode instructions to the program
                    output.Add(code);
                    instructions.Increment();
                }
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
                var bytecode = instr.Visit(emitter);
                foreach (var code in bytecode) {
                    // Add all bytecode instructions to the program
                    output.Add(code);
                    instructions.Increment();
                }
            }
        } 

        // Error checking
        if (emitter.IsMissingLabelAddress) {
            throw new ArgumentException($"The labels { string.Join(',', emitter.UncomputedLabels) } are not bound to a memory location. Make sure all labels are declared.");
        }
    
        return output;
    }
}

delegate void AddressComputationThunk(uint computed);

internal class Assembly2BytecodeTransformer : IInstructionVisitor<IEnumerable<Bytecode.IBytecodeInstruction>> {

    private Dictionary<string, List<AddressComputationThunk>> awaiting_label_computation = new Dictionary<string, List<AddressComputationThunk>>();
    private Dictionary<string, uint> label_address = new Dictionary<string, uint>();

    private Counter count;
    private RegisterIndex AssemblerReserved = RegisterIndex.At;
    private RegisterIndex Zero = RegisterIndex.Zero;

    public bool IsMissingLabelAddress => awaiting_label_computation.Count > 0;
    public IEnumerable<string> UncomputedLabels => awaiting_label_computation.Keys;

    public Counter MemoryUsed = new Counter();

    public Assembly2BytecodeTransformer(Counter count, Dictionary<string, uint> labels) {
        this.count = count;
        this.label_address = labels;
    }

    public IEnumerable<Bytecode.IBytecodeInstruction> EncodeBytes(string label, IEnumerable<byte> integers) {
        // Save address in memory for the stored data
        var address = MemoryUsed.Count;
        if (label_address.ContainsKey(label))
            throw new ArgumentException($"Label '{label}' has been defined multiple times");
        label_address.Add(label, address);

        // Store the data
        foreach (var element in integers) {
            // Load value
            yield return new Bytecode.Llo {
                Immediate = element,
                Target = AssemblerReserved
            };

            // Store value
            yield return new Bytecode.Sb {
                Target = AssemblerReserved,     // Register containing the value to save
                Source = Zero,                  // Register containing the "base" address
                Immediate = MemoryUsed.Count   // Current memory pointer
            };
            
            // Increment memory counter
            MemoryUsed += 1; // 1 byte
        }
    }

    public IEnumerable<Bytecode.IBytecodeInstruction> EncodeHalf(string label, IEnumerable<Int16> integers) {
        // Save address in memory for the stored data
        var address = MemoryUsed.Count;
        if (label_address.ContainsKey(label))
            throw new ArgumentException($"Label '{label}' has been defined multiple times");
        label_address.Add(label, address);

        // Store the data
        foreach (var element in integers) {
            // Load value
            yield return new Bytecode.Llo {
                Immediate = BitConverter.ToUInt16(BitConverter.GetBytes(element)),
                Target = AssemblerReserved
            };

            // Store value
            yield return new Bytecode.Sh {
                Target = AssemblerReserved,     // Register containing the value to save
                Source = Zero,                  // Register containing the "base" address
                Immediate = MemoryUsed.Count   // Current memory pointer
            };
            
            // Increment memory counter
            MemoryUsed += 2; // 2 bytes
        }
    }

    public IEnumerable<Bytecode.IBytecodeInstruction> EncodeWord(string label, IEnumerable<UInt32> integers) {
        // Save address in memory for the stored data
        var address = MemoryUsed.Count;
        if (label_address.ContainsKey(label))
            throw new ArgumentException($"Label '{label}' has been defined multiple times");
        label_address.Add(label, address);

        // Store the data
        foreach (var element in integers) {
            // Load value
            yield return new Bytecode.Lhi {
                Immediate = element >> 16,
                Target = AssemblerReserved
            };
            yield return new Bytecode.Llo {
                Immediate = element & 0b11111111_11111111,
                Target = AssemblerReserved
            };

            // Store value
            yield return new Bytecode.Sw {
                Target = AssemblerReserved,     // Register containing the value to save
                Source = Zero,                  // Register containing the "base" address
                Immediate = MemoryUsed.Count   // Current memory pointer
            };
            
            // Increment memory counter
            MemoryUsed += 4; // 4 bytes
        }
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.LabelMarker marker) {
        // Mark this location to swap instructions waiting for this label
        uint computed_address = current();
        if (label_address.ContainsKey(marker.Name))
            throw new ArgumentException($"Label '{marker.Name}' has been defined multiple times");
        label_address.Add(marker.Name, computed_address);

        List<AddressComputationThunk>? to_inject;
        if (awaiting_label_computation.TryGetValue(marker.Name, out to_inject)) {
            foreach (var instr in to_inject) {
                instr(computed_address);
            }
            awaiting_label_computation.Remove(marker.Name);
        }

        // Doesn't return any new instructions YAY!
        yield break;
    }

    private void resolve(AddressLikeToken address, AddressComputationThunk thunk) {
        if (address is ScalarConstantToken literal) {
            thunk((uint)literal.IntegerValue);
        } else {
            var label = address.Value;
            if (label_address.ContainsKey(label)) {
                thunk(label_address[label]);
            } else {
                // Needs to be computed when we eventually compute this label
                if (this.awaiting_label_computation.ContainsKey(label)) {
                    this.awaiting_label_computation[label].Add(thunk);
                } else {
                    this.awaiting_label_computation[label] = new List<AddressComputationThunk>{thunk};
                }
            }
        }
    }
    private uint current() {
        return count.Count << 2; // Current instruction index * 4 bytes per word
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.AddSigned instr) {
        yield return new Bytecode.Add {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.AddSignedImmediate instr) {
        yield return new Bytecode.Addi {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperand
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.SubtractSigned instr) {
        yield return new Bytecode.Sub {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.SubtractSignedImmediate instr) {
        // There is no subtract immediate, its just an add with a negative.
        // See https://chortle.ccsu.edu/assemblytutorial/Chapter-13/ass13_12.html
        yield return new Bytecode.Addiu {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = BitConverter.ToUInt32(BitConverter.GetBytes(-instr.RhsOperand))
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.AddUnsigned instr) {
        yield return new Bytecode.Addu {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.AddUnsignedImmediate instr) {
        yield return new Bytecode.Addiu {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperand
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.SubtractUnsigned instr) {
        yield return new Bytecode.Subu {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.SubtractUnsignedImmediate instr) {
        // There is no subtract immediate, its just an add with a negative.
        // See https://chortle.ccsu.edu/assemblytutorial/Chapter-13/ass13_12.html
        yield return new Bytecode.Addiu {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = BitConverter.ToUInt32(BitConverter.GetBytes(-((int)instr.RhsOperand)))
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.MultiplySignedWithOverflow instr) {
        yield return new Bytecode.Mult {
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.MultiplyUnsignedWithOverflow instr) {
        yield return new Bytecode.Multu {
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.DivideSignedWithRemainder instr) {
        yield return new Bytecode.Div {
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.DivideUnsignedWithRemainder instr) {
        yield return new Bytecode.Divu {
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.SetOnLessThan instr) {
        yield return new Bytecode.Slt {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.SetOnLessThanImmediate instr) {
        yield return new Bytecode.Slti {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.Constant
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.BranchGreaterThan0 instr) {
        var branch = new Bgtz {
            Source = instr.LhsOperandRegister
        };
        var instr_addr = current();
        resolve(instr.Address, (label) => {
            branch.AddressOffset = (int)(((long)label - instr_addr + 4) >> 2);
        });
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.BranchLessThanOrEqual0 instr) {
        var branch = new Blez {
            Source = instr.LhsOperandRegister
        };
        var instr_addr = current();
        resolve(instr.Address, (label) => {
            branch.AddressOffset = (int)(((long)label - instr_addr + 4) >> 2);
        });
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.BranchOnGreater instr) {
        // Subtract the 2
        var sub = new Sub {
            Destination = AssemblerReserved, // At register is reserved for assembler usage like this
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister,
        };
        yield return sub;
        // If registers[destination] > 0 then lhs > rhs
        var branch = new Bgtz {
            Source = sub.Destination
        };
        var instr_addr = current();
        resolve(instr.Address, (label) => {
            branch.AddressOffset = (int)(((long)label - instr_addr + 4) >> 2);
        });
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.BranchOnGreaterOrEqual instr) {
        // Branch on Greater
        foreach (var instrs in Accept(new Assembly.BranchOnGreater{
            LhsOperandRegister = instr.LhsOperandRegister,
            RhsOperandRegister = instr.RhsOperandRegister,
            Address = instr.Address
        })) {
            yield return instrs;
        }
        // Branch on Equal
        var branch = new Beq {
            Source = instr.LhsOperandRegister,
            Target = instr.RhsOperandRegister,
        };
        var instr_addr = current();
        resolve(instr.Address, (label) => {
            branch.AddressOffset = (int)(((long)label - instr_addr + 4) >> 2);
        });
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.BranchOnLess instr) {
        // Subtract the 2
        var sub = new Sub {
            Destination = AssemblerReserved, // At register is reserved for assembler usage like this
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister,
        };
        yield return sub;

        // Make sure the 2 are not equal first
        var eqb = new Beq {
            Source = instr.LhsOperandRegister,
            Target = instr.RhsOperandRegister,
        };
        eqb.AddressOffset = 4 >> 2; // Skip a single instruction ahead (jump past the <= check)
        yield return eqb;

        // If registers[destination] <= 0 then lhs < rhs since we know the 2 are not equal from above
        var branch = new Blez {
            Source = sub.Destination
        };
        var instr_addr = current();
        resolve(instr.Address, (label) => {
            branch.AddressOffset = (int)(((long)label - instr_addr + 4) >> 2);
        });
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.BranchOnLessOrEqual instr) {
        // Branch on Less
        foreach (var instrs in Accept(new Assembly.BranchOnLess{
            LhsOperandRegister = instr.LhsOperandRegister,
            RhsOperandRegister = instr.RhsOperandRegister,
            Address = instr.Address
        })) {
            yield return instrs;
        }
        // Branch on Equal
        var branch = new Beq {
            Source = instr.LhsOperandRegister,
            Target = instr.RhsOperandRegister,
        };
        var instr_addr = current();
        resolve(instr.Address, (label) => {
            branch.AddressOffset = (int)(((long)label - instr_addr + 4) >> 2);
        });
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.BranchOnEqual instr) {
        var branch = new Beq {
            Source = instr.LhsOperandRegister,
            Target = instr.RhsOperandRegister,
        };
        var instr_addr = current();
        resolve(instr.Address, (label) => {
            branch.AddressOffset = (int)(((long)label - instr_addr + 4) >> 2);
        });
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.BranchOnNotEqual instr) {
        var branch = new Bne {
            Source = instr.LhsOperandRegister,
            Target = instr.RhsOperandRegister,
        };
        var instr_addr = current();
        resolve(instr.Address, (label) => {
            branch.AddressOffset = (int)(((long)label - instr_addr + 4) >> 2);
        });
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.And instr) {
        yield return new Bytecode.And {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.Or instr) {
        yield return new Bytecode.Or {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.Nor instr) {
        yield return new Bytecode.Nor {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.Xor instr) {
        yield return new Bytecode.Xor {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.AndImmediate instr) {
        yield return new Bytecode.Andi {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperand
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.OrImmediate instr) {
        yield return new Bytecode.Ori {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperand
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.XorImmediate instr) {
        yield return new Bytecode.Xori {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperand
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.ShiftLeftLogical instr) {
        yield return new Bytecode.Sllv {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.ShiftRightLogical instr) {
        yield return new Bytecode.Srlv {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.LoadWord instr) {
        yield return new Bytecode.Lw {
            Target = instr.ResultRegister,
            Source = instr.BaseRegister,
            Immediate = instr.Offset,
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.StoreWord instr) {
        yield return new Bytecode.Sw {
            Target = instr.SourceRegister,
            Source = instr.BaseRegister,
            Immediate = instr.Offset,
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.LoadUpperImmediate instr) {
        // Only load the upper bits 
        var target = instr.ResultRegister;
        var value = instr.Constant;
        yield return new Lhi {
            Target = target,
            Immediate = value.HighHalf(),
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.LoadAddress instr) {
        if (instr.Label == null)
            throw new ArgumentException("Label is undefined");

        var token = new LabelToken(0, instr.Label);

        // Load immediate splits into 2 instructions one to load the high bits, the other to load the low bits
        var target = instr.ResultRegister;
        var lhi = new Lhi {
            Target = target,
        };
        var llo = new Llo {
            Target = target,
        };
        var instr_addr = current();
        resolve(token, (label) => {
            lhi.Immediate = BitConverter.ToUInt32(BitConverter.GetBytes(label)).HighHalf();
        });
        resolve(token, (label) => {
            llo.Immediate = BitConverter.ToUInt32(BitConverter.GetBytes(label)).LowHalf();
        });
        yield return lhi;
        yield return llo;
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.LoadImmediate instr) {
        // Load immediate splits into 2 instructions one to load the high bits, the other to load the low bits
        var target = instr.ResultRegister;
        var value = instr.Constant;
        yield return new Lhi {
            Target = target,
            Immediate = value.HighHalf(),
        };
        yield return new Llo {
            Target = target,
            Immediate = value.LowHalf()
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.MoveFromHi instr) {
        yield return new Bytecode.Mfhi {
            Destination = instr.ResultRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.MoveFromLo instr) {
        yield return new Bytecode.Mflo {
            Destination = instr.ResultRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.Move instr) {
        var at = AssemblerReserved;
        
        // Cache current hi
        yield return new Mfhi {
            Destination = at
        };

        // Copy to hi
        yield return new Mthi {
            Source = instr.SourceRegister
        };
        // Copy from hi
        yield return new Mfhi {
            Destination = instr.ResultRegister
        };

        // Restore cached hi value
        yield return new Mthi {
            Source = at
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.JumpTo instr) {
        var jump = new J {};
        var instr_addr = current();
        resolve(instr.Address, (label) => {
            jump.AddressOffset = (int)(((long)label - instr_addr + 4) >> 2);
        });
        yield return jump;
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.JumpAndLink instr) {
        var jump = new Jal {};
        var instr_addr = current();
        resolve(instr.Address, (label) => {
            jump.AddressOffset = (int)(((long)label - instr_addr + 4) >> 2);
        });
        yield return jump;
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.JumpRegister instr) {
        var jump = new Jr {};
        jump.Source = instr.Register;
        yield return jump;
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.Syscall instr) {
        yield return new Bytecode.Syscall {};
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.LoadIntoCoprocessor1 instr) {
        yield return new Bytecode.Lwc1 {
            Target = instr.ResultRegister,
            Source = instr.BaseRegister,
            Immediate = instr.Offset,
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.StoreFromCoprocessor1 instr) {
        yield return new Bytecode.Swc1 {
            Target = instr.SourceRegister,
            Source = instr.BaseRegister,
            Immediate = instr.Offset,
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.MoveFromCoprocessor1 instr) {
        yield return new Bytecode.Mfc1 {
            FpuRegister = instr.FpuRegister,
            CpuRegister = instr.CpuRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.MoveToCoprocessor1 instr) {
        yield return new Bytecode.Mtc1 {
            FpuRegister = instr.FpuRegister,
            CpuRegister = instr.CpuRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.AddSingle instr) {
        yield return new Bytecode.AddS {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.SubtractSingle instr) {
        yield return new Bytecode.SubS {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.MultiplySingle instr) {
        yield return new Bytecode.MulS {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.DivideSingle instr) {
        yield return new Bytecode.DivS {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction> Accept(Assembly.AbsoluteValueSingle instr) {
        yield return new Bytecode.AbsS {
            Destination = instr.ResultRegister,
            Source = instr.SourceRegister,
        };
    }
}
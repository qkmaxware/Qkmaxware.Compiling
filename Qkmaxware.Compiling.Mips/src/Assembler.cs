using System;
using System.Collections.Generic;
using Qkmaxware.Compiling.Mips.Assembly;
using Qkmaxware.Compiling.Mips.Bytecode;

namespace Qkmaxware.Compiling.Mips;

class Counter {
    public uint Count {get; private set;}
    public Counter () {}

    public void Increment() => Count++;
}

/// <summary>
/// Assembler for MIPS assembly to MIPS bytecode
/// </summary>
public class Assembler {
    /// <summary>
    /// Assemble an assembly program and write the bytecode to the given writer
    /// </summary>
    /// <param name="input">assembly program</param>
    /// <param name="writer">bytecode emitter</param>
    public void AssembleTo(Assembly.AssemblyProgram input, IBytecodeWriter writer) {
        var instructions = new Counter();
        var label_address = new Dictionary<string, uint>();
        var emitter = new Assembly2BytecodeTransformer(instructions, label_address);
        
        // Handle creating the data in the data section first
        foreach (var section in input.DataSections) {
            // Encode the data as instructions
            // Store the address of the instruction in the label_addresses map
        }

        // Handle invoking / jumping to main method
        foreach (var section in input.GlobalSections) {
            // Add instruction to jump to main method (if it exists)
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
                    if (code != null) {
                        writer.Encode(code);
                        instructions.Increment();
                    }
                }
            }
        } 

        // Error checking
        if (emitter.IsMissingLabelAddress) {
            throw new ArgumentException("One or more labels used are not bound to a memory location. Make sure all labels are declared.");
        }
    }

    /// <summary>
    /// Assembly the given assembly program to an in-memory bytecode representation
    /// </summary>
    /// <param name="input">assembly program</param>
    /// <returns>bytecode program</returns>
    public Bytecode.BytecodeProgram Assemble(Assembly.AssemblyProgram input) {    
        var output = new InMemoryBytecodeProgram();

        this.AssembleTo(input, output); // In-memory programs can be written to
    
        return output;
    }
}

internal class Assembly2BytecodeTransformer : IInstructionVisitor<IEnumerable<Bytecode.IBytecodeInstruction?>> {

    private Dictionary<string, List<Bytecode.IBytecodeInstruction>> awaiting_label_computation = new Dictionary<string, List<IBytecodeInstruction>>();
    private Dictionary<string, uint> label_address = new Dictionary<string, uint>();

    private Counter count;

    public bool IsMissingLabelAddress => label_address.Count > 0;

    public Assembly2BytecodeTransformer(Counter count, Dictionary<string, uint> labels) {
        this.count = count;
        this.label_address = labels;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.LabelMarker marker) {
        // Mark this location to swap instructions waiting for this label
        uint computed_address = current();
        label_address.Add(marker.Name, computed_address);

        List<Bytecode.IBytecodeInstruction>? to_inject;
        if (awaiting_label_computation.TryGetValue(marker.Name, out to_inject)) {
            foreach (var instr in to_inject) {
                instr.InjectResolvedAddress(computed_address);
            }
            awaiting_label_computation.Remove(marker.Name);
        }

        // Doesn't return any new instructions YAY!
        yield break;
    }

    private uint resolve(IBytecodeInstruction instr, AddressLikeToken address) {
        if (address is ScalarConstantToken literal) {
            return (uint)literal.IntegerValue;
        } else {
            var label = address.Value;
            if (label_address.ContainsKey(label)) {
                return label_address[label];
            } else {
                // Needs to be computed when we eventually compute this label
                this.awaiting_label_computation[label].Add(instr);
                return 0U; // A temp label until we replace it
            }
        }
    }
    private uint current() {
        return count.Count;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.AddSigned instr) {
        yield return new Bytecode.Add {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.AddSignedImmediate instr) {
        yield return new Bytecode.Addi {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperand
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.SubtractSigned instr) {
        yield return new Bytecode.Sub {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.SubtractSignedImmediate instr) {
        // There is no subtract immediate, its just an add with a negative.
        // See https://chortle.ccsu.edu/assemblytutorial/Chapter-13/ass13_12.html
        yield return new Bytecode.Addiu {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = BitConverter.ToUInt32(BitConverter.GetBytes(-instr.RhsOperand))
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.AddUnsigned instr) {
        yield return new Bytecode.Addu {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.AddUnsignedImmediate instr) {
        yield return new Bytecode.Addiu {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperand
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.SubtractUnsigned instr) {
        yield return new Bytecode.Subu {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.SubtractUnsignedImmediate instr) {
        // There is no subtract immediate, its just an add with a negative.
        // See https://chortle.ccsu.edu/assemblytutorial/Chapter-13/ass13_12.html
        yield return new Bytecode.Addiu {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = BitConverter.ToUInt32(BitConverter.GetBytes(-((int)instr.RhsOperand)))
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.MultiplySignedWithOverflow instr) {
        yield return new Bytecode.MultSigned {
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.MultiplyUnsignedWithOverflow instr) {
        yield return new Bytecode.MultUnsigned {
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.DivideSignedWithRemainder instr) {
        yield return new Bytecode.DivSigned {
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.DivideUnsignedWithRemainder instr) {
        yield return new Bytecode.DivUnsigned {
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.SetOnLessThan instr) {
        yield return new Bytecode.Slt {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.SetOnLessThanImmediate instr) {
        yield return new Bytecode.Slti {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.Constant
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchGreaterThan0 instr) {
        var branch = new Bgtz {
            Source = instr.LhsOperandRegister
        };
        branch.AddressOffset = (int)(resolve(branch, instr.Address) - (current() + 4)) >> 2;
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchLessThanOrEqual0 instr) {
        var branch = new Blez {
            Source = instr.LhsOperandRegister
        };
        branch.AddressOffset = (int)(resolve(branch, instr.Address) - (current() + 4)) >> 2;
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchOnGreater instr) {
        // Subtract the 2
        var sub = new Sub {
            Destination = RegisterIndex.NamedOrThrow("at"), // At register is reserved for assembler usage like this
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister,
        };
        yield return sub;
        // If registers[destination] > 0 then lhs > rhs
        var branch = new Bgtz {
            Source = sub.Destination
        };
        branch.AddressOffset = (int)(resolve(branch, instr.Address) - (current() + 4)) >> 2;
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchOnGreaterOrEqual instr) {
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
        branch.AddressOffset = (int)(resolve(branch, instr.Address) - (current() + 4)) >> 2;
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchOnLess instr) {
        // Subtract the 2
        var sub = new Sub {
            Destination = RegisterIndex.NamedOrThrow("at"), // At register is reserved for assembler usage like this
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
        branch.AddressOffset = (int)(resolve(branch, instr.Address) - (current() + 4)) >> 2;
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchOnLessOrEqual instr) {
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
        branch.AddressOffset = (int)(resolve(branch, instr.Address) - (current() + 4)) >> 2;
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchOnEqual instr) {
        var branch = new Beq {
            Source = instr.LhsOperandRegister,
            Target = instr.RhsOperandRegister,
        };
        branch.AddressOffset = (int)(resolve(branch, instr.Address) - (current() + 4)) >> 2;
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchOnNotEqual instr) {
        var branch = new Bne {
            Source = instr.LhsOperandRegister,
            Target = instr.RhsOperandRegister,
        };
        branch.AddressOffset = (int)(resolve(branch, instr.Address) - (current() + 4)) >> 2;
        yield return branch;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.And instr) {
        yield return new Bytecode.And {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.Or instr) {
        yield return new Bytecode.Or {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.AndImmediate instr) {
        yield return new Bytecode.Andi {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperand
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.OrImmediate instr) {
        yield return new Bytecode.Ori {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperand
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.ShiftLeftLogical instr) {
        yield return new Bytecode.Sllv {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.ShiftRightLogical instr) {
        yield return new Bytecode.Srlv {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.LoadWord instr) {
        yield return new Bytecode.Lw {
            Target = instr.ResultRegister,
            Source = instr.BaseRegister,
            Immediate = instr.Offset,
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.StoreWord instr) {
        yield return new Bytecode.Sw {
            Target = instr.SourceRegister,
            Source = instr.BaseRegister,
            Immediate = instr.Offset,
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.LoadUpperImmediate instr) {
        // Only load the upper bits 
        var target = instr.ResultRegister;
        var value = instr.Constant;
        yield return new Lhi {
            Target = target,
            Immediate = value.HighHalf(),
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.LoadAddress instr) {
        if (instr.Label == null)
            throw new ArgumentException("Label is undefined");

        var load = new Bytecode.Lw {
            Target = instr.ResultRegister,
            Source = new RegisterIndex(0),
        };
        load.Immediate = (resolve(load, new LabelToken(0, instr.Label)) - (current() + 4)) >> 2;
        yield return load;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.LoadImmediate instr) {
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

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.MoveFromHi instr) {
        yield return new Bytecode.Mfhi {
            Destination = instr.ResultRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.MoveFromLo instr) {
        yield return new Bytecode.Mflo {
            Destination = instr.ResultRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.Move instr) {
        var at = RegisterIndex.NamedOrThrow("at");
        
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

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.JumpTo instr) {
        var jump = new J {};
        jump.AddressOffset = (int)(resolve(jump, instr.Address) - (current() + 4)) >> 2;
        yield return jump;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.JumpAndLink instr) {
        var jump = new Jal {};
        jump.AddressOffset = (int)(resolve(jump, instr.Address) - (current() + 4)) >> 2;
        yield return jump;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.JumpRegister instr) {
        var jump = new Jr {};
        jump.Source = instr.Register;
        yield return jump;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.Syscall instr) {
        yield return new Bytecode.Syscall {};
    }
}
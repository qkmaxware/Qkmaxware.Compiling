using System;
using System.Collections.Generic;
using Qkmaxware.Compiling.Mips.Assembly;
using Qkmaxware.Compiling.Mips.Bytecode;

namespace Qkmaxware.Compiling.Mips;

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
        var label_address = new Dictionary<LabelMarker, uint>();
        var emitter = new Assembly2BytecodeTransformer(label_address);
        
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
                    if (code != null)
                        writer.Encode(code);
                }
            }
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

class Assembly2BytecodeTransformer : IInstructionVisitor<IEnumerable<Bytecode.IBytecodeInstruction?>> {

    private Dictionary<LabelMarker, List<Bytecode.IBytecodeInstruction>> awaiting_label_computation = new Dictionary<LabelMarker, List<IBytecodeInstruction>>();
    private Dictionary<LabelMarker, uint> label_address = new Dictionary<LabelMarker, uint>();

    public Assembly2BytecodeTransformer(Dictionary<LabelMarker, uint> labels) {
        this.label_address = labels;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.LabelMarker marker) {
        // Mark this location to swap instructions waiting for this label
        uint computed_address = 0; // TODO actually compute this
        label_address.Add(marker, computed_address);

        List<Bytecode.IBytecodeInstruction>? to_inject;
        if (awaiting_label_computation.TryGetValue(marker, out to_inject)) {
            foreach (var instr in to_inject) {
                // TODO replace the address in question with the computed one
            }
            awaiting_label_computation.Remove(marker);
        }

        // Doesn't return any new instructions YAY!
        yield return null;
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.AddSigned instr) {
        yield return new Bytecode.AddSigned {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.AddSignedImmediate instr) {
        yield return new Bytecode.AddSignedImmediate {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperand
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.SubtractSigned instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.SubtractSignedImmediate instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.AddUnsigned instr) {
        yield return new Bytecode.AddUnsigned {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.AddUnsignedImmediate instr) {
        yield return new Bytecode.AddUnsignedImmediate {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperand
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.SubtractUnsigned instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.SubtractUnsignedImmediate instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.MultiplyWithoutOverflow instr) {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.SetOnLessThanImmediate instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchOnGreater instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchGreaterThan0 instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchLessThanOrEqual0 instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchOnGreaterOrEqual instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchOnLess instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchOnLessOrEqual instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchOnEqual instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.BranchOnNotEqual instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.And instr) {
        yield return new Bytecode.And {
            Destination = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperandRegister
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.Or instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.AndImmediate instr) {
        yield return new Bytecode.Andi {
            Target = instr.ResultRegister,
            LhsOperand = instr.LhsOperandRegister,
            RhsOperand = instr.RhsOperand
        };
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.OrImmediate instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.ShiftLeftLogical instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.ShiftRightLogical instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.LoadWord instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.StoreWord instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.LoadUpperImmediate instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.LoadAddress instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.LoadImmediate instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.MoveFromHi instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.MoveFromLo instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.Move instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.JumpTo instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.JumpRegister instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.JumpAndLink instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Assembly.Syscall instr) {
        throw new NotImplementedException();
    }
}
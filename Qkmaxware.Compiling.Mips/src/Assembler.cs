using System;
using System.Collections.Generic;
using Qkmaxware.Compiling.Mips.Assembly;
using Qkmaxware.Compiling.Mips.Bytecode;

namespace Qkmaxware.Compiling.Mips;

/// <summary>
/// Assembler for MIPS assembly to MIPS bytecode
/// </summary>
public class Assembler {
    public void AssembleTo(Assembly.AssemblyProgram input, IBytecodeWriter writer) {
        var emitter = new Assembly2BytecodeTransformer();
        
        // Handle creating the data in the data section first
        foreach (var section in input.DataSections) {

        }

        // Handle invoking / jumping to main method
        foreach (var section in input.GlobalSections) {

        }

        // Handle writing code sections
        foreach (var section in input.TextSections) {
            // Most instructions are 1-1 mappings from Assembly Instructions to Bytecode Instructions
            foreach (var instr in section.Code) {
                var bytecode = instr.Visit(emitter);
                foreach (var code in bytecode) {
                    if (code != null)
                        writer.Encode(code);
                }
            }
        } 
    }

    public Bytecode.BytecodeProgram Assemble(Assembly.AssemblyProgram input) {    
        var output = new InMemoryBytecodeProgram();

        this.AssembleTo(input, output);
    
        return output;
    }
}

class Assembly2BytecodeTransformer : IInstructionVisitor<IEnumerable<Bytecode.IBytecodeInstruction?>> {

    private Dictionary<LabelMarker, List<Bytecode.IBytecodeInstruction>> awaiting_label_computation = new Dictionary<LabelMarker, List<IBytecodeInstruction>>();
    private Dictionary<LabelMarker, uint> label_address = new Dictionary<LabelMarker, uint>();

    public IEnumerable<IBytecodeInstruction?> Accept(LabelMarker marker) {
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

    public IEnumerable<IBytecodeInstruction?> Accept(AddSignedImmediate instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(SubtractSigned instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(SubtractSignedImmediate instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(AddUnsigned instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(AddUnsignedImmediate instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(SubtractUnsigned instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(SubtractUnsignedImmediate instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(MultiplyWithoutOverflow instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(MultiplyWithOverflow instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(DivideWithRemainder instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(SetOnLessThan instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(SetOnLessThanImmediate instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(BranchOnGreater instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(BranchGreaterThan0 instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(BranchLessThanOrEqual0 instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(BranchOnGreaterOrEqual instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(BranchOnLess instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(BranchOnLessOrEqual instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(BranchOnEqual instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(BranchOnNotEqual instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(And instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Or instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(AndImmediate instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(OrImmediate instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(ShiftLeftLogical instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(ShiftRightLogical instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(LoadWord instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(StoreWord instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(LoadUpperImmediate instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(LoadAddress instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(LoadImmediate instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(MoveFromHi instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(MoveFromLo instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Move instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(JumpTo instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(JumpRegister instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(JumpAndLink instr) {
        throw new NotImplementedException();
    }

    public IEnumerable<IBytecodeInstruction?> Accept(Syscall instr) {
        throw new NotImplementedException();
    }
}
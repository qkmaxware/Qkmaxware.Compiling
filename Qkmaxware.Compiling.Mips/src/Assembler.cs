using System;
using System.Collections.Generic;
using Qkmaxware.Compiling.Mips.InstructionSet;

namespace Qkmaxware.Compiling.Mips;

public class Assembler {
    public void Assemble(IEnumerable<IAssembleable> assembly) {
        Dictionary<Label, uint> address_map = new Dictionary<Label, uint>();
        
        foreach (var instruction in assembly) {

        }
    }
}
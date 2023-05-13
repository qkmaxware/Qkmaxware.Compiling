using Qkmaxware.Compiling.Targets.Mips.Assembly;

namespace Qkmaxware.Compiling.Targets.Mips;

public class AssemblerEnvironment {

    public delegate void AddressComputationThunk(uint computed);

    private Dictionary<string,uint> label_addresses = new Dictionary<string, uint>();
    private Dictionary<string, List<AddressComputationThunk>> awaiting_label_computation = new Dictionary<string, List<AddressComputationThunk>>();

    public void ResolveLabelAddressOnceComputed(string label, AddressComputationThunk thunk) {
        if (label_addresses.ContainsKey(label)) {
            thunk(label_addresses[label]);
        } else {
            // Needs to be computed when we eventually compute this label
            if (this.awaiting_label_computation.ContainsKey(label)) {
                this.awaiting_label_computation[label].Add(thunk);
            } else {
                this.awaiting_label_computation[label] = new List<AddressComputationThunk>{thunk};
            }
        }
    }

    uint instruction_count = 0;

    public uint CurrentInstructionAddress() {
        return instruction_count << 2;
    }

    internal void IncrementInstructionCount() {
        instruction_count++;
    }

    public void SetLabelAddress(string label, uint address) {
        // Mark this location to swap instructions waiting for this label
        if (label_addresses.ContainsKey(label))
            throw new ArgumentException($"Label '{label}' has been defined multiple times");
        label_addresses.Add(label, address);

        List<AddressComputationThunk>? to_inject;
        if (awaiting_label_computation.TryGetValue(label, out to_inject)) {
            foreach (var instr in to_inject) {
                instr(address);
            }
            awaiting_label_computation.Remove(label);
        }
    }

    public bool HasLabelsWithoutAddresses() => this.awaiting_label_computation.Count > 0;
    public int LabelsAwaitingAddresses() => this.awaiting_label_computation.Count;

}
namespace Qkmaxware.Compiling.Targets.Mips.Assembly;

/// <summary>
/// Visitor pattern for assembly instructions
/// </summary>
public interface IInstructionVisitor {
    public void Accept(LabelMarker marker);
    public void Accept(AddSigned instr);
    public void Accept(AddSignedImmediate instr);
    public void Accept(SubtractSigned instr);
    public void Accept(SubtractSignedImmediate instr);
    public void Accept(AddUnsigned instr);
    public void Accept(AddUnsignedImmediate instr);
    public void Accept(SubtractUnsigned instr);
    public void Accept(SubtractUnsignedImmediate instr);
    public void Accept(MultiplySignedWithOverflow instr);
    public void Accept(MultiplyUnsignedWithOverflow instr);
    public void Accept(DivideSignedWithRemainder instr);
    public void Accept(DivideUnsignedWithRemainder instr);

    public void Accept(SetOnLessThan instr);
    public void Accept(SetOnLessThanImmediate instr);

    public void Accept(BranchOnGreater instr);
    public void Accept(BranchGreaterThan0 instr);
    public void Accept(BranchLessThanOrEqual0 instr);
    public void Accept(BranchOnGreaterOrEqual instr);
    public void Accept(BranchOnLess instr);
    public void Accept(BranchOnLessOrEqual instr);
    public void Accept(BranchOnEqual instr);
    public void Accept(BranchOnNotEqual instr);

    public void Accept(And instr);
    public void Accept(Or instr);
    public void Accept(Nor instr);
    public void Accept(Xor instr);
    public void Accept(AndImmediate instr);
    public void Accept(OrImmediate instr);
    public void Accept(XorImmediate instr);
    public void Accept(ShiftLeftLogical instr);
    public void Accept(ShiftRightLogical instr);

    public void Accept(LoadWord instr);
    public void Accept(StoreWord instr);
    public void Accept(LoadUpperImmediate instr);
    public void Accept(LoadAddress instr);
    public void Accept(LoadImmediate instr);
    public void Accept(MoveFromHi instr);
    public void Accept(MoveFromLo instr);
    public void Accept(Move instr);

    public void Accept(JumpTo instr);
    public void Accept(JumpRegister instr);
    public void Accept(JumpAndLink instr);

    public void Accept(Syscall instr);

    public void Accept(LoadIntoCoprocessor1 instr);
    public void Accept(StoreFromCoprocessor1 instr);
    public void Accept(MoveToCoprocessor1 instr);
    public void Accept(MoveFromCoprocessor1 instr);
    public void Accept(AbsoluteValueSingle instr);
    public void Accept(AddSingle instr);
    public void Accept(SubtractSingle instr);
    public void Accept(MultiplySingle instr);
    public void Accept(DivideSingle instr);
}

/// <summary>
/// Visitor pattern for assembly instructions
/// </summary>
public interface IInstructionVisitor<T> {
    public T Accept(LabelMarker marker);
    public T Accept(AddSigned instr);
    public T Accept(AddSignedImmediate instr);
    public T Accept(SubtractSigned instr);
    public T Accept(SubtractSignedImmediate instr);
    public T Accept(AddUnsigned instr);
    public T Accept(AddUnsignedImmediate instr);
    public T Accept(SubtractUnsigned instr);
    public T Accept(SubtractUnsignedImmediate instr);
    public T Accept(MultiplySignedWithOverflow instr);
    public T Accept(MultiplyUnsignedWithOverflow instr);
    public T Accept(DivideSignedWithRemainder instr);
    public T Accept(DivideUnsignedWithRemainder instr);

    public T Accept(SetOnLessThan instr);
    public T Accept(SetOnLessThanImmediate instr);

    public T Accept(BranchOnGreater instr);
    public T Accept(BranchGreaterThan0 instr);
    public T Accept(BranchLessThanOrEqual0 instr);
    public T Accept(BranchOnGreaterOrEqual instr);
    public T Accept(BranchOnLess instr);
    public T Accept(BranchOnLessOrEqual instr);
    public T Accept(BranchOnEqual instr);
    public T Accept(BranchOnNotEqual instr);

    public T Accept(And instr);
    public T Accept(Or instr);
    public T Accept(Nor instr);
    public T Accept(Xor instr);
    public T Accept(XorImmediate instr);
    public T Accept(AndImmediate instr);
    public T Accept(OrImmediate instr);
    public T Accept(ShiftLeftLogical instr);
    public T Accept(ShiftRightLogical instr);

    public T Accept(LoadWord instr);
    public T Accept(StoreWord instr);
    public T Accept(LoadUpperImmediate instr);
    public T Accept(LoadAddress instr);
    public T Accept(LoadImmediate instr);
    public T Accept(MoveFromHi instr);
    public T Accept(MoveFromLo instr);
    public T Accept(Move instr);

    public T Accept(JumpTo instr);
    public T Accept(JumpRegister instr);
    public T Accept(JumpAndLink instr);

    public T Accept(Syscall instr);

    public T Accept(LoadIntoCoprocessor1 instr);
    public T Accept(StoreFromCoprocessor1 instr);
    public T Accept(MoveToCoprocessor1 instr);
    public T Accept(MoveFromCoprocessor1 instr);
    public T Accept(AbsoluteValueSingle instr);
    public T Accept(AddSingle instr);
    public T Accept(SubtractSingle instr);
    public T Accept(MultiplySingle instr);
    public T Accept(DivideSingle instr);
}
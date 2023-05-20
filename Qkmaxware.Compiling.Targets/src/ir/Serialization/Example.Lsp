; Example of what serialization might look like in a LISP like notation
; Advantages: Quite short and instructions match the whole "tuple" idea 
; Disadvantages: Lots of parenthesis, no good way to match arguments to reference instances

(Module 
    (Globals
        (pi F32)
    )
    (Programs
        (square
            (Arguments
                (number F32)
            )
            (Locals
                (return F32)
            )
            (Blocks
                (block_1412
                    (Multiply return number number)
                    (Return return)
                    (Exit)
                )
            )
        )
        (area
            (Arguments
                (radius F32)
            )
            (Locals
                (radius^2 F32)
                (return F32)
            )
            (Blocks
                (block_1412
                    (CallFunction 0 radius^2 radius)
                    (Multiply return pi radius^2)
                    (Return return)
                    (Exit)
                )
            )
        )
    )
)
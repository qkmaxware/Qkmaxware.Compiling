# Instruction Tuples
Read more about IR [here](https://cs.lmu.edu/~ray/notes/ir/).

<table class="code-first">
<tbody><tr>
  <th>Tuple
  </th><th>Rendered as...
  </th><th>Description
</th></tr>
<tr>
  <td nowrap="nowrap">(COPY,x,y)
  </td><td nowrap="nowrap">y := x
  </td><td>Simple copy
</td></tr>
<tr>
  <td nowrap="nowrap">(COPY_FROM_DEREF,x,y)
  </td><td nowrap="nowrap">y := *x
  </td><td>Copy the contents of memory at address x into y
</td></tr>
<tr>
  <td nowrap="nowrap">(COPY_TO_DEREF,x,y)
  </td><td nowrap="nowrap">*y := x
  </td><td>Copy x into the memory at address y
</td></tr>
<tr>
  <td nowrap="nowrap">(COPY_FROM_OFS,x,y,z)
  </td><td nowrap="nowrap">z := *(x+y)
  </td><td>Copy the contents of memory at address x+y into z
</td></tr>
<tr>
  <td nowrap="nowrap">(COPY_TO_OFS,x,y,z)
  </td><td nowrap="nowrap">*(y+z) := x
  </td><td>Copy x into the memory at address y+z
</td></tr>
<tr>
  <td nowrap="nowrap">(ADD,x,y,z)
  </td><td nowrap="nowrap">z := x + y
  </td><td>Sum
</td></tr>
<tr>
  <td nowrap="nowrap">(SUB,x,y,z)
  </td><td nowrap="nowrap">z := x - y
  </td><td>Difference
</td></tr>
<tr>
  <td nowrap="nowrap">(MUL,x,y,z)
  </td><td nowrap="nowrap">z := x * y
  </td><td>Product
</td></tr>
<tr>
  <td nowrap="nowrap">(DIV,x,y,z)
  </td><td nowrap="nowrap">z := x / y
  </td><td>Quotient
</td></tr>
<tr>
  <td nowrap="nowrap">(MOD,x,y,z)
  </td><td nowrap="nowrap">z := x mod y
  </td><td>Modulo
</td></tr>
<tr>
  <td nowrap="nowrap">(REM,x,y,z)
  </td><td nowrap="nowrap">z := x rem y
  </td><td>Remainder
</td></tr>
<tr>
  <td nowrap="nowrap">(POWER,x,y,z)
  </td><td nowrap="nowrap">z := pow x, y
  </td><td>Exponentiation
</td></tr>
<tr>
  <td nowrap="nowrap">(SHL,x,y,z)
  </td><td nowrap="nowrap">z := x &lt;&lt; y
  </td><td>Left shift 
</td></tr>
<tr>
  <td nowrap="nowrap">(SHR,x,y,z)
  </td><td nowrap="nowrap">z := x &gt;&gt; y
  </td><td>Logical right shift
</td></tr>
<tr>
  <td nowrap="nowrap">(SAR,x,y,z)
  </td><td nowrap="nowrap">z := x &gt;&gt;&gt; y
  </td><td>Arithmetic right shift
</td></tr>
<tr>
  <td nowrap="nowrap">(AND,x,y,z)
  </td><td nowrap="nowrap">z := x &amp; y
  </td><td>Bitwise conjunction
</td></tr>
<tr>
  <td nowrap="nowrap">(OR,x,y,z)
  </td><td nowrap="nowrap">z := x | y
  </td><td>Bitwise disjunction
</td></tr>
<tr>
  <td nowrap="nowrap">(XOR,x,y,z)
  </td><td nowrap="nowrap">z := x xor y
  </td><td>Bitwise exclusive or
</td></tr>
<tr>
  <td nowrap="nowrap">(NOT,x,y)
  </td><td nowrap="nowrap">y := !x
  </td><td>Logical complement
</td></tr>
<tr>
  <td nowrap="nowrap">(NEG,x,y)
  </td><td nowrap="nowrap">y := -x
  </td><td>Negation
</td></tr>
<tr>
  <td nowrap="nowrap">(COMP,x,y)
  </td><td nowrap="nowrap">y := ~x
  </td><td>Bitwise complement
</td></tr>
<tr>
  <td nowrap="nowrap">(ABS,x,y)
  </td><td nowrap="nowrap">y := abs x
  </td><td>Absolute value
</td></tr>
<tr>
  <td nowrap="nowrap">(SIN,x,y)
  </td><td nowrap="nowrap">y := sin x
  </td><td>Sine
</td></tr>
<tr>
  <td nowrap="nowrap">(COS,x,y)
  </td><td nowrap="nowrap">y := cos x
  </td><td>Cosine
</td></tr>
<tr>
  <td nowrap="nowrap">(ATAN,x,y,z)
  </td><td nowrap="nowrap">z := atan x,y
  </td><td>Arctangent
</td></tr>
<tr>
  <td nowrap="nowrap">(LN,x,y)
  </td><td nowrap="nowrap">y := ln x
  </td><td>Natural logarithm
</td></tr>
<tr>
  <td nowrap="nowrap">(SQRT,x,y)
  </td><td nowrap="nowrap">y := sqrt x
  </td><td>Square root
</td></tr>
<tr>
  <td nowrap="nowrap">(INC x)
  </td><td nowrap="nowrap">inc x
  </td><td>Increment
</td></tr>
<tr>
  <td nowrap="nowrap">(DEC,x)
  </td><td nowrap="nowrap">dec x
  </td><td>Decrement
</td></tr>
<tr>
  <td nowrap="nowrap">(INC_DEREF x)
  </td><td nowrap="nowrap">inc *x
  </td><td>Increment a memory location given its address
</td></tr>
<tr>
  <td nowrap="nowrap">(DEC_DEREF,x)
  </td><td nowrap="nowrap">dec *x
  </td><td>Decrement a memory location given its address
</td></tr>
<tr>
  <td nowrap="nowrap">(LT,x,y,z)
  </td><td nowrap="nowrap">z := x &lt; y
  </td><td>1 if x is less than y,0 otherwise
</td></tr>
<tr>
  <td nowrap="nowrap">(LE,x,y,z)
  </td><td nowrap="nowrap">z := x &lt;= y
  </td><td>1 if x is less than or equal to y; 0 otherwise
</td></tr>
<tr>
  <td nowrap="nowrap">(EQ,x,y,z)
  </td><td nowrap="nowrap">z := x == y
  </td><td>1 if x is equal to y; 0 otherwise
</td></tr>
<tr>
  <td nowrap="nowrap">(NE,x,y,z)
  </td><td nowrap="nowrap">z := x != y
  </td><td>1 if x is not equal to y; 0 otherwise
</td></tr>
<tr>
  <td nowrap="nowrap">(GE,x,y,z)
  </td><td nowrap="nowrap">z := x &gt;= y
  </td><td>1 if x is greater than or equal to y; 0 otherwise
</td></tr>
<tr>
  <td nowrap="nowrap">(GT,x,y,z)
  </td><td nowrap="nowrap">z := x &gt; y
  </td><td>1 if x is greater than y; 0 otherwise
</td></tr>
<tr>
  <td nowrap="nowrap">(LABEL,L)
  </td><td nowrap="nowrap">L:
  </td><td>Label
</td></tr>
<tr>
  <td nowrap="nowrap">(JUMP,L)
  </td><td nowrap="nowrap">goto L
  </td><td>Unconditional jump to a label
</td></tr>
<tr>
  <td nowrap="nowrap">(JZERO,x,L)
  </td><td nowrap="nowrap">if x == 0 goto L
  </td><td>Jump if zero / Jump if false
</td></tr>
<tr>
  <td nowrap="nowrap">(JNZERO,x,L)
  </td><td nowrap="nowrap">if x != 0 goto L
  </td><td>Jump if zero / Jump if true
</td></tr>
<tr>
  <td nowrap="nowrap">(JLT,x,y,L)
  </td><td nowrap="nowrap">if x &lt; y goto L
  </td><td>Jump if less / Jump if not greater or equal
</td></tr>
<tr>
  <td nowrap="nowrap">(JLE,x,y,L)
  </td><td nowrap="nowrap">if x &lt;= y goto L
  </td><td>Jump if less or equal / Jump if not greater
</td></tr>
<tr>
  <td nowrap="nowrap">(JEQ,x,y,L)
  </td><td nowrap="nowrap">if x == y goto L
  </td><td>Jump if equal
</td></tr>
<tr>
  <td nowrap="nowrap">(JNE,x,y,L)
  </td><td nowrap="nowrap">if x != y goto L
  </td><td>Jump if not equal
</td></tr>
<tr>
  <td nowrap="nowrap">(JGE,x,y,L)
  </td><td nowrap="nowrap">if x &gt;= y goto L
  </td><td>Jump if greater or equal / Jump if not less
</td></tr>
<tr>
  <td nowrap="nowrap">(JGT,x,y,L)
  </td><td nowrap="nowrap">if x &gt; y goto L
  </td><td>Jump if greater / Jump if not less
</td></tr>
<tr>
  <td nowrap="nowrap">(IJE,x,y,L)
  </td><td nowrap="nowrap">if ++x == y goto L
  </td><td>Increment and jump if equal. Useful at end of for-loops that count up to a given value.
</td></tr>
<tr>
  <td nowrap="nowrap">(DJNZ,x,L)
  </td><td nowrap="nowrap">if --x != 0 goto L
  </td><td>Decrement and jump if not zero. Great for loops that count down to zero.
</td></tr>
<tr>
  <td nowrap="nowrap">(CALLP,f)
  </td><td nowrap="nowrap">call f
  </td><td>Call procedure (non-value returning function) f
</td></tr>
<tr>
  <td nowrap="nowrap">(CALLF,f,x)
  </td><td nowrap="nowrap">x := call f
  </td><td>Call function f, catching return value in x
</td></tr>
<tr>
  <td nowrap="nowrap">(RETP)
  </td><td nowrap="nowrap">ret
  </td><td>Return from procedure
</td></tr>
<tr>
  <td nowrap="nowrap">(RETF,x)
  </td><td nowrap="nowrap">ret x
  </td><td>Return x from this function
</td></tr>
<tr>
  <td nowrap="nowrap">(INT_TO_STRING,x,y)
  </td><td nowrap="nowrap">y := to_string x
  </td><td>String representation of an integer (one can add a new
  tuple to take a base)
</td></tr>
<tr>
  <td nowrap="nowrap">(FLOAT_TO_STRING,x,y)
  </td><td nowrap="nowrap">y := to_string x
  </td><td>String representation of a float (one can add a new
  tuple taking a format string)
</td></tr>
<tr>
  <td nowrap="nowrap">(BOOL_TO_STRING,x,y)
  </td><td nowrap="nowrap">y := to_string x
  </td><td>String representation of a boolean (localized?)
</td></tr>
<tr>
  <td nowrap="nowrap">(CHAR_TO_STRING,x,y)
  </td><td nowrap="nowrap">y := to_string x
  </td><td>String representation of a character
</td></tr>
<tr>
  <td nowrap="nowrap">(ALLOC,x,y)
  </td><td nowrap="nowrap">y := alloc x
  </td><td>Allocate x bytes of memory, returning the address in y
  (or 0 if the memory could not be allocated).
</td></tr>
<tr>
  <td nowrap="nowrap">(TO_FLOAT,x,y)
  </td><td nowrap="nowrap">y := to_float x
  </td><td>Integer to real
</td></tr>
<tr>
  <td nowrap="nowrap">(NULL_CHECK,x)
  </td><td nowrap="nowrap">assert x != 0
  </td><td>Do something if x is null
</td></tr>
<tr>
  <td nowrap="nowrap">(ASSERT_POSITIVE,x)
  </td><td nowrap="nowrap">assert x &gt; 0
  </td><td>Do something if x is not positive
</td></tr>
<tr>
  <td nowrap="nowrap">(BOUND,x,y,z)
  </td><td nowrap="nowrap">assert y &lt;= x &lt; z
  </td><td>Do something if x is not between y (inclusive) and
  x (inclusive)
</td></tr>
<tr>
  <td nowrap="nowrap">(NO_OP)
  </td><td nowrap="nowrap">nop
  </td><td>Do nothing
</td></tr>
<tr>
  <td nowrap="nowrap">(EXIT)
  </td><td nowrap="nowrap">exit
  </td><td>terminate the program
</td></tr>
</tbody></table>

The operands to a tuple are:
- labels
- subroutine references
- simple, non-structured, values (literals, variables, and temporaries)
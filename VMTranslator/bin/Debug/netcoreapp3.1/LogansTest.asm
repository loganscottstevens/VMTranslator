// Map SP
@256
D=A
@SP
M=D
// Map LCL
@300
D=A
@LCL
M=D
// Map ARG
@400
D=A
@ARG
M=D
// Map THIS
@3000
D=A
@THIS
M=D
// Map THAT
@3010
D=A
@THAT
M=D
// C_PUSH constant 1
@1
D=A
@SP
A=M
M=D
@SP
M=M+1
// C_PUSH constant 2
@2
D=A
@SP
A=M
M=D
@SP
M=M+1
// ADD
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M+D
@SP
A=M
M=D
@SP
M=M+1
// C_POP static 2
@SP
M=M-1
A=M
D=M
@test1.2
M=D
// C_PUSH constant 5
@5
D=A
@SP
A=M
M=D
@SP
M=M+1
// C_PUSH constant 2
@2
D=A
@SP
A=M
M=D
@SP
M=M+1
// C_POP local 1
@SP
M=M-1
A=M
D=M
@LCL
A=M
A=A+1
M=D
// C_PUSH local 1
@LCL
A=M
A=A+1
D=M
@SP
A=M
M=D
@SP
M=M+1
// C_POP static 3
@SP
M=M-1
A=M
D=M
@test2.3
M=D
// INFINITE LOOP
(LOOP)
@LOOP
0;JMP

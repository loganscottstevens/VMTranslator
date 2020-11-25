#region Author Info
// ===============================
// AUTHOR          :Logan Stevens
// CREATE DATE     :11/9/20
//================================ 
#endregion
using System.Collections.Generic;
using System.IO;

namespace VMTranslator
{
    class CodeWriter
    {
        #region Properties
        private StreamWriter outputFile { get; set; }
        private int LabelIndex { get; set; } = 0;
        public string OutputFileDir { get; private set; }
        public string FileName { get; set; }
        #endregion

        private static Dictionary<string,string> aSMPointers = new Dictionary<string, string>()
                {
                    {"local", "LCL" },
                    {"argument", "ARG" },
                    {"this", "THIS" },
                    {"that", "THAT" },
                    {"temp", "R5" },
                    {"pointer", "THIS" }
                };

    #region Constructors
    public CodeWriter(string fileName)
        {
            OutputFileDir = fileName;
            outputFile = new StreamWriter(fileName);
            WriteInit();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Description: Writes the vm arithmetic command in symbolic Hack assembly code<br/>
        /// Precondition: Must be initialized<br/>
        /// Postcondition: Arithmetic is written to output .asm file
        /// </summary>
        /// <param name="command"></param>
        public void WriteArithmetic(string command)
        {
            switch (command)
            {
                case "add":
                    outputFile.WriteLine(
                        "// ADD\n" +
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "D=M\n" +
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "D=M+D\n" +
                        "@SP\n" +
                        "A=M\n" +
                        "M=D\n" +
                        "@SP\n" +
                        "M=M+1");
                    break;
                case "sub":
                    outputFile.WriteLine(
                        "// SUB\n" +
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "D=M\n" +
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "D=M-D\n" +
                        "@SP\n" +
                        "A=M\n" +
                        "M=D\n" +
                        "@SP\n" +
                        "M=M+1");
                    break;
                case "eq":
                    outputFile.WriteLine(
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "D=M\n" +
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "D=M-D\n" +
                        $"@TRUE{LabelIndex}\n" +
                        "D;JEQ\n" +
                        "@SP\n" +
                        "A=M\n" +
                        "M=0\n" +
                        $"@ENDEQ{LabelIndex}\n" +
                        "0;JMP\n" +
                        $"(TRUE{LabelIndex})\n" +
                        "@SP\n" +
                        "A=M\n" +
                        "M=-1\n" +
                        $"(ENDEQ{LabelIndex})\n" +
                        "@SP\n" +
                        "M=M+1");
                    LabelIndex++;
                    break;
                case "gt":
                    outputFile.WriteLine(
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "D=M\n" +
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "D=M-D\n" +
                        $"@TRUE{LabelIndex}\n" +
                        "D;JGT\n" +
                        "@SP\n" +
                        "A=M\n" +
                        "M=0\n" +
                        $"@ENDGT{LabelIndex}\n" +
                        "0;JMP\n" +
                        $"(TRUE{LabelIndex})\n" +
                        "@SP\n" +
                        "A=M\n" +
                        "M=-1\n" +
                        $"(ENDGT{LabelIndex})\n" +
                        "@SP\n" +
                        "M=M+1");
                    LabelIndex++;
                    break;
                case "lt":
                    outputFile.WriteLine(
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "D=M\n" +
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "D=M-D\n" +
                        $"@TRUE{LabelIndex}\n" +
                        "D;JLT\n" +
                        "@SP\n" +
                        "A=M\n" +
                        "M=0\n" +
                        $"@ENDLT{LabelIndex}\n" +
                        "0;JMP\n" +
                        $"(TRUE{LabelIndex})\n" +
                        "@SP\n" +
                        "A=M\n" +
                        "M=-1\n" +
                        $"(ENDLT{LabelIndex})\n" +
                        "@SP\n" +
                        "M=M+1");
                    LabelIndex++;
                    break;
                case "neg":
                    outputFile.WriteLine(
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "M=-M\n" +
                        "@SP\n" +
                        "M=M+1");
                    break;
                case "and":
                    outputFile.WriteLine(
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "D=M\n" +
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "D=D&M\n" +
                        "@SP\n" +
                        "A=M\n" +
                        "M=D\n" +
                        "@SP\n" +
                        "M=M+1");
                    break;
                case "or":
                    outputFile.WriteLine(
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "D=M\n" +
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "D=D|M\n" +
                        "@SP\n" +
                        "A=M\n" +
                        "M=D\n" +
                        "@SP\n" +
                        "M=M+1");
                    break;
                case "not":
                    outputFile.WriteLine(
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "M=!M\n" +
                        "@SP\n" +
                        "M=M+1");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Description: Writes the vm push or pop command in symbolic Hack assembly code<br/>
        /// Precondition: Must be initialized<br/>
        /// Postcondition: The push/pop logic is written to output .asm file
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="segment"></param>
        /// <param name="index"></param>
        public void WritePushPop(CommandType commandType, string segment, int index)
        {
            outputFile.WriteLine($"// {commandType} {segment} {index}");
            if (commandType == CommandType.C_PUSH)
            {
                switch (segment)
                {
                    case "constant":
                        outputFile.WriteLine
                        (
                            "@" + index + "\n" +
                            "D=A\n" +
                            "@SP\n" +
                            "A=M\n" +
                            "M=D\n" +
                            "@SP\n" +
                            "M=M+1"
                        );

                        break;
                    case "temp":
                    case "pointer":
                        outputFile.WriteLine
                            (
                            "@" + LookupSegmentASM(segment));
                        for (int i = 0; i < index; i++)
                        {
                            outputFile.WriteLine("A=A+1");
                        }
                        outputFile.WriteLine
                            (
                            "D=M\n" +
                            "@SP\n" +
                            "A=M\n" +
                            "M=D\n" +
                            "@SP\n" +
                            "M=M+1"
                            );
                        break;
                    case "static":
                        outputFile.WriteLine(
                            $"@{FileName}.{index}\n" +
                            "D=M\n" +
                            "@SP\n" +
                            "A=M\n" +
                            "M=D\n" +
                            "@SP\n" +
                            "M=M+1"
                              );
                        break;
                    default:
                        outputFile.WriteLine(
                            $"@{LookupSegmentASM(segment)}\n" +
                            "A=M");
                        for (int i = 0; i < index; i++)
                        {
                            outputFile.WriteLine("A=A+1");
                        }
                        outputFile.WriteLine
                            (
                            "D=M\n" +
                            "@SP\n" +
                            "A=M\n" +
                            "M=D\n" +
                            "@SP\n" +
                            "M=M+1"
                            );
                        break;
                }
            }
            else     // CommandType == C_POP
            {
                if (segment != "static")
                {
                    outputFile.WriteLine(
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "D=M\n" +
                        "@" + LookupSegmentASM(segment));
                }
                else
                {
                    outputFile.WriteLine
                    (
                    "@SP\n" +
                    "M=M-1\n" +
                    "A=M\n" +
                    "D=M\n" +
                    $"@{FileName}.{index}"
                    );
                }
                if (segment != "temp" && segment != "pointer" && segment != "static")
                {
                    outputFile.WriteLine("A=M");
                }
                if (segment != "static")
                {
                    for (int i = 0; i < index; i++)
                    {
                        outputFile.WriteLine
                        (
                            "A=A+1"
                        );
                    }
                }
                outputFile.WriteLine
                (
                    "M=D"
                );

            }
        }

        /// <summary>
        /// Description: Writes assembly code that effects the VM initialization.
        /// </summary>
        public void WriteInit()
        {
            MapMemorySegments();
            WriteCall("Sys.init", 0);
        }

        /// <summary>
        /// Description: Writes assembly code that effects the label command.<br/>
        /// Precondition: Must be initialized.<br/>
        /// Postcondition: Label is written to output .asm file
        /// </summary>
        /// <param name="label"></param>
        public void WriteLabel(string label)
        {
            outputFile.WriteLine(
                $"// LABEL {label}\n" +
                $"({label})");
        }

        /// <summary>
        /// Description: Writes assembly code that effects the goto command.<br/>
        /// Precondition: Must be initialized.<br/>
        /// Postcondition: Goto command is written to output .asm file
        /// </summary>
        /// <param name="label"></param>
        public void WriteGoto(string label)
        {
            outputFile.WriteLine(
                $"// GOTO {label}\n" +
                $"@{label}\n" +
                "0;JMP");
        }

        /// <summary>
        /// Description: Writes assembly code that effects the if-goto command.<br/>
        /// Precondition: Must be initialized.<br/>
        /// Postcondition: If-Goto command is written to output .asm file
        /// </summary>
        /// <param name="label"></param>
        public void WriteIf(string label)
        {
            outputFile.WriteLine(
                $"// IF-GOTO {label}\n" +
                "@SP\n" +
                "M=M-1\n" +
                "A=M\n" +
                "D=M\n" +
                $"@{label}\n" +
                "D;JNE");
        }

        /// <summary>
        /// Description: Writes assembly code that effects the function command.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="numLocals"></param>
        public void WriteFunction(string functionName, int numLocals)
        {
            WriteLabel(functionName);
            for (int i = 0; i < numLocals; i++)
            {
                WritePushPop(CommandType.C_PUSH, "constant", 0);
            }
        }

        /// <summary>
        /// Description: Writes assembly code that effects the call command.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="numArgs"></param>
        public void WriteCall(string functionName, int numArgs)
        {
            outputFile.WriteLine(
                "// PUSH RETURN ADDRESS\n" +
                $"@{FileName}$ret.{LabelIndex}\n" +
                "D=A\n" +
                "@SP\n" +
                "A=M\n" +
                "M=D\n" +
                "@SP\n" +
                "M=M+1\n" +
                "// PUSH LCL\n" +
                "@LCL\n" +
                "D=M\n" +
                "@SP\n" +
                "A=M\n" +
                "M=D\n" +
                "@SP\n" +
                "M=M+1\n" +
                "// PUSH ARG\n" +
                "@ARG\n" +
                "D=M\n" +
                "@SP\n" +
                "A=M\n" +
                "M=D\n" +
                "@SP\n" +
                "M=M+1\n" +
                "// PUSH THIS\n" +
                "@THIS\n" +
                "D=M\n" +
                "@SP\n" +
                "A=M\n" +
                "M=D\n" +
                "@SP\n" +
                "M=M+1\n" +
                "// PUSH THAT\n" +
                "@THAT\n" +
                "D=M\n" +
                "@SP\n" +
                "A=M\n" +
                "M=D\n" +
                "@SP\n" +
                "M=M+1\n" +
                "// ARG = SP-5-NARGS\n" +
                $"@{numArgs}\n" +
                "D=A\n" +
                "@5\n" +
                "D=D+A\n" +
                "@SP\n" +
                "D=M-D\n" +
                "@ARG\n" +
                "M=D\n" +
                "// LCL = SP\n" +
                "@SP\n" +
                "D=M\n" +
                "@LCL\n" +
                "M=D");
            WriteGoto(functionName);
            WriteLabel($"{FileName}$ret.{LabelIndex}");
            LabelIndex++;
        }

        /// <summary>
        /// Description: Writes assembly code that effects the return command.
        /// </summary>
        public void WriteReturn()
        {
            outputFile.WriteLine(
                "// WRITE RETURN\n" +
                "@LCL\n" +
                "D=M\n" +
                $"@endFrame.{LabelIndex}\n" +
                "M=D\n" +
                $"@endFrame.{LabelIndex}\n" +
                "D=M\n" +
                "@5\n" +
                "D=D-A\n" +
                "A=D\n" +
                "D=M\n" +
                $"@returnAddress.{LabelIndex}\n" +
                "M=D");
            WritePushPop(CommandType.C_POP, "argument", 0);
            outputFile.WriteLine(
                "D=A\n" +
                "@SP\n" +
                "M=D+1\n" +
                $"@endFrame.{LabelIndex}\n" +
                "A=M-1\n" +
                "D=M\n" +
                "@THAT\n" +
                "M=D\n" +
                "@2\n" +
                "D=A\n" +
                $"@endFrame.{LabelIndex}\n" +
                "A=M-D\n" +
                "D=M\n" +
                "@THIS\n" +
                "M=D\n" +
                "@3\n" +
                "D=A\n" +
                $"@endFrame.{LabelIndex}\n" +
                "A=M-D\n" +
                "D=M\n" +
                "@ARG\n" +
                "M=D\n" +
                "@4\n" +
                "D=A\n" +
                $"@endFrame.{LabelIndex}\n" +
                "A=M-D\n" +
                "D=M\n" +
                "@LCL\n" +
                "M=D\n" +
                $"@returnAddress.{LabelIndex}\n" +
                "A=M\n" +
                "0;JMP");
            LabelIndex++;
        }

        /// <summary>
        /// Description: Closes codewriter<br/>
        /// Precondition: Must be initialized<br/>
        /// Postcondition: Output streamwriter is closed
        /// </summary>
        public void Close()
        {
            outputFile.Close();
        }
        #endregion

        #region Private Helper Methods
        /// <summary>
        /// Description: Looks up the corresponding segment hack symbol<br/>
        /// Precondition: None<br/>
        /// Postcondition: Hack symbol is looked up
        /// </summary>
        /// <param name="segment"></param>
        /// <returns>Hack symbol corresponding to given vm segment</returns>
        private static string LookupSegmentASM(string segment)
        {
            aSMPointers.TryGetValue(segment, out string value);
            return value;
        }

        /// <summary>
        /// Description: Maps the memory segments to particular base addresses in the beginning of the output asm file<br/>
        /// Precondition: Must be initialized<br/>
        /// Postcondition: Memory segments are mapped to output file
        /// </summary>
        private void MapMemorySegments()
        {
            outputFile.WriteLine(
                "// Map SP\n" +
                "@256\n" +
                "D=A\n" +
                "@SP\n" +
                "M=D\n" +
                "// Map LCL\n" +
                "@300\n" +
                "D=A\n" +
                "@LCL\n" +
                "M=D\n" +
                "// Map ARG\n" +
                "@400\n" +
                "D=A\n" +
                "@ARG\n" +
                "M=D\n" +
                "// Map THIS\n" +
                "@3000\n" +
                "D=A\n" +
                "@THIS\n" +
                "M=D\n" +
                "// Map THAT\n" +
                "@3010\n" +
                "D=A\n" +
                "@THAT\n" +
                "M=D"
                );
        } 
        #endregion
    }
}

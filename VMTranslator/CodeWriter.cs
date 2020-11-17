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

        #region Constructors
        public CodeWriter(string fileName)
        {
            OutputFileDir = fileName;
            outputFile = new StreamWriter(fileName);
            MapMemorySegments();
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
                    outputFile.WriteLine
                        (
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
                        "M=M+1"
                        );
                    break;
                case "sub":
                    outputFile.WriteLine
                        (
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
                        "M=M+1"
                        );
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
                    "M=M+1"
                        );
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
                    "M=M+1"
                        );
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
                    "M=M+1"
                        );
                    LabelIndex++;
                    break;
                case "neg":
                    outputFile.WriteLine(
                        "@SP\n" +
                        "M=M-1\n" +
                        "A=M\n" +
                        "M=-M\n" +
                        "@SP\n" +
                        "M=M+1\n"
                    );
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
                        "M=M+1\n"
                    );
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
                        outputFile.WriteLine
                            (
                            "@" + LookupSegmentASM(segment) + "\n" +
                            "A=M"
                            );
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
                    outputFile.WriteLine
                    (

                    "@SP\n" +
                    "M=M-1\n" +
                    "A=M\n" +
                    "D=M\n" +
                    "@" + LookupSegmentASM(segment)
                    );
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
        /// Description: Closes codewriter<br/>
        /// Precondition: Must be initialized<br/>
        /// Postcondition: Output streamwriter is closed
        /// </summary>
        public void Close()
        {
            outputFile.WriteLine(
                "// INFINITE LOOP\n" +
                "(LOOP)\n" +
                "@LOOP\n" +
                "0;JMP");
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
            new Dictionary<string, string>()
                {
                    {"local", "LCL" },
                    {"argument", "ARG" },
                    {"this", "THIS" },
                    {"that", "THAT" },
                    {"temp", "R5" },
                    {"pointer", "THIS" }
                }.TryGetValue(segment, out string value);
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

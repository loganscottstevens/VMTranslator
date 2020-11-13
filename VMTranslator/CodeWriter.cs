using System;
using System.Collections.Generic;
using System.IO;

namespace VMTranslator
{
    class CodeWriter
    {
        // FIELDS
        private StreamWriter outputFile;
        private int LabelIndex { get; set; } = 0;
        public string OutputFileDir { get; private set; }
        public string FileName { get; set; }

        // CONSTRUCTORS
        public CodeWriter(string fileName)
        {
            OutputFileDir = fileName;
            outputFile = new StreamWriter(fileName);
            MapMemorySegments();
        }

        // METHODS
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
                    Console.WriteLine("ErRoR:::::: default has been reached in the write arithmetic method.");
                    break;
            }
        }

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

        public void TerminateWithLoop()
        {
            outputFile.WriteLine(
                "// INFINITE LOOP\n" +
                "(LOOP)\n" +
                "@LOOP\n" +
                "0;JMP");
        }

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

        public void Close()
        {
            outputFile.Close();
        }
    }
}

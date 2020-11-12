using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VMTranslator
{
    class CodeWriter
    {
        // FIELDS
        private StreamWriter outputFile;

        // CONSTRUCTORS
        public CodeWriter(string fileName)
        {
            outputFile = new StreamWriter(fileName);
        }

        // METHODS
        public void WriteArithmetic(string command)
        {
            switch (command)
            {
                case "add":
                    outputFile.WriteLine
                        (
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
                        "M=M-1"
                        );
                    break;
                default:
                    break;
            }
        }

        public void WritePushPop(CommandType commandType, string segment, int index)
        {
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
                    default:
                        break;
                }
            }
            else     // CommandType == C_POP
            {
                outputFile.WriteLine
                (
                    "@SP\n" +
                    "M=M-1\n" +
                    "A=M\n" +
                    "D=M\n" +
                    "@" + LookupSegmentASM(segment) + "\n" +
                    "A=M"
                );
                for (int i = 0; i < index; i++)
                {
                    outputFile.WriteLine
                    (
                        "M=M+1"
                    );
                }
                outputFile.WriteLine
                (
                    "M=D"
                );

            }
        }

        private static string LookupSegmentASM(string segment)
        {
                new Dictionary<string, string>()
                {
                    {"local", "LCL" }
                }.TryGetValue(segment,out string value);
            return value;
        }

        public void Close()
        {
            outputFile.Close();
        }
    }
}

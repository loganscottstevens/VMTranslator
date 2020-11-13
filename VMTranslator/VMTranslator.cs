using System;
using System.IO;
using System.Text.RegularExpressions;

namespace VMTranslator
{
    class VMTranslator
    {
        // METHODS
        static void Main()
        {
            Regex assemblyFileRegex = new Regex("\\w+\\.asm");
            string filePath = @"C:\Users\logan\Documents\School\CS220\nand2tetris\nand2tetris\projects\07\MemoryAccess\BasicTest\BasicTest.vm";
            // in case of one file argument, this turns it into a directory
            if (assemblyFileRegex.IsMatch(filePath))
            {
                filePath = Directory.GetCurrentDirectory();
                Console.WriteLine(filePath);
                Console.ReadLine();
            }

            try
            {
                //in case directory is a file, this gets it's directory
                if (File.GetAttributes(filePath) != FileAttributes.Directory)
                {
                    filePath = filePath.Remove(filePath.LastIndexOf('\\'));
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("No such file or directory: " + filePath);
                Environment.Exit(-1);
            }

            CodeWriter codeWriter = new CodeWriter(filePath + @"\translation.asm");
            Console.WriteLine(codeWriter.outputFileDir);

            string[] files = Directory.GetFiles(filePath, "*.vm");
            foreach (string file in files)
            {
                Parser parser = new Parser(file);
                while (parser.HasMoreCommands())
                {
                    parser.Advance();
                    //Console.WriteLine(parser.ToString());
                    //Console.ReadLine();
                    switch (parser.CommandType)
                    {
                        case CommandType.C_ARITHMETIC:
                            codeWriter.WriteArithmetic(parser.Arg1);
                            break;
                        case CommandType.C_PUSH:
                        case CommandType.C_POP:
                            codeWriter.WritePushPop(parser.CommandType, parser.Arg1, parser.Arg2);
                            break;
                        case CommandType.C_LABEL:
                            break;
                        case CommandType.C_GOTO:
                            break;
                        case CommandType.C_IF:
                            break;
                        case CommandType.C_FUNCTION:
                            break;
                        case CommandType.C_RETURN:
                            break;
                        case CommandType.C_CALL:
                            break;
                        default:
                            break;
                    }
                }
            }
            codeWriter.TerminateWithLoop();
            codeWriter.Close();
        }
    }
}

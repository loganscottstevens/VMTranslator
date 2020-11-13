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
            Regex vMFileRegex = new Regex("\\w+\\.vm");
            string filePath =
                @"C:\Users\logan\Desktop\LogansTest\test1.vm";
            string fileName = "noname";
            // in case of one file argument, this turns it into a directory
            Console.WriteLine(Path.GetFileName(filePath));
            Console.ReadLine();
            if (vMFileRegex.IsMatch(filePath))
            {
                filePath = Directory.GetCurrentDirectory();
                Console.WriteLine("regex match" + filePath);
                Console.ReadLine();
            }


            try
            {
                //in case directory is a file, this gets its directory
                if (File.GetAttributes(filePath) != FileAttributes.Directory)
                {
                    fileName = Path.GetFileName(filePath) + ".asm";
                    //fileName = filePath.Remove(filePath.LastIndexOf('.')).Substring(filePath.LastIndexOf('\\') + 1) + ".asm";
                    filePath = filePath.Remove(filePath.LastIndexOf('\\'));
                }
                else
                {
                    fileName = Path.GetFileName(filePath) + ".asm";
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("No such file or directory: " + filePath);
                Environment.Exit(-1);
            }

            CodeWriter codeWriter = new CodeWriter(filePath + @$"\{fileName}");
            Console.WriteLine(codeWriter.OutputFileDir);

            string[] files = Directory.GetFiles(filePath, "*.vm");
            foreach (string file in files)
            {
                codeWriter.FileName = Path.GetFileNameWithoutExtension(file);
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

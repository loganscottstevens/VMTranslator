#region Author Info
// ===============================
// AUTHOR          :Logan Stevens
// CREATE DATE     :11/9/20
//================================ 
#endregion
#region Program Algorithm
/****************************************
* Program Algorithm
* 1. Get filepath from user
* 2. IF path is valid directory, create
*    output file with foldername.asm
*    ELSE IF path is a file, create 
*    output file with same name.asm 
* 3. Initialize codewriter with output 
*    file path
* 4. FOREACH file in the directory
*    create a parser for that file,
*    parse the file, and use codewriter
*    to write the output code
* 5. Close the output file with an infi
*    nite loop
* **************************************/ 
#endregion

using System;
using System.IO;

namespace VMTranslator
{
    class VMTranslator
    {
        static void Main()
        {
            #region FileIO
            Console.WriteLine("Enter directory or filename:");
            string filePath = Path.GetFullPath(Console.ReadLine());
            string[] files = { };
            CodeWriter codeWriter = null;
            if (File.Exists(filePath) && Path.GetExtension(filePath) == ".vm")
            {
                codeWriter = new CodeWriter($"{Path.ChangeExtension(filePath, "asm")}");
                files = new string[] { filePath };
            }
            else if (Directory.Exists(filePath) && Directory.GetFiles(filePath, "*.vm").Length > 0)
            {
                codeWriter = new CodeWriter(@$"{filePath}\{Path.GetFileNameWithoutExtension(filePath)}.asm");
                files = Directory.GetFiles(filePath, "*.vm");
            }
            else
            {
                Console.WriteLine($"No .vm files found in path:\n{filePath}\nExiting...");
                Environment.Exit(0);
            }
            #endregion

            #region MainProgram
            foreach (string file in files)
            {
                codeWriter.FileName = $"{Path.GetFileNameWithoutExtension(file)}";
                Parser parser = new Parser(file);
                while (parser.HasMoreCommands())
                {
                    parser.Advance();
                    Console.WriteLine(parser.ToString());
                    Console.ReadLine();
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
                            codeWriter.WriteLabel(parser.Arg1);
                            break;
                        case CommandType.C_GOTO:
                            codeWriter.WriteGoto(parser.Arg1);
                            break;
                        case CommandType.C_IF:
                            codeWriter.WriteIf(parser.Arg1);
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
            codeWriter.Close();
            #endregion
        }
    }
}

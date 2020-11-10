using System;
using System.IO;
using System.Text.RegularExpressions;

namespace VMTranslator
{
    class VMTranslator
    {
        // FIELDS
        private CodeWriter codeWriter;
        private Parser parser;

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

            string[] files = Directory.GetFiles(filePath, "*.vm");
            foreach (string file in files)
            {
                Parser parser = new Parser(file);
                while (parser.HasMoreCommands())
                {
                    parser.Advance();
                    Console.WriteLine(parser.ToString());
                    Console.ReadLine();
                }
            }
        }
    }
}

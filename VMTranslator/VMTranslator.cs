using System;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;

namespace VMTranslator
{
    class VMTranslator
    {
        // FIELDS
        private CodeWriter codeWriter;
        private Parser parser;

        // METHODS
        static void Main(string[] args)
        {
            CodeWriter codeWriter = new CodeWriter("test.asm");
            Regex assemblyFileRegex = new Regex("\\w+\\.asm");
            string directoryOrFile = @"C:\Users\logan\Documents\Coding Workspaces\VMTranslator\VMTranslator\test.asm";
            FileAttributes attributes = File.GetAttributes(directoryOrFile);
            if (attributes == FileAttributes.Directory)
            {
                string[] files = Directory.GetFiles(directoryOrFile, "*.vm");
                foreach (string file in files)
                {
                    Parser parser = new Parser(file);
                    while (parser.HasMoreCommands())
                    {
                        parser.Advance();
                    }
                }
            }
        }
    }
}

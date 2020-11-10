using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace VMTranslator
{
    class Parser
    {
        // FIELDS
        public string Arg1 { get; set; }

        public int Arg2 { get; set; }

        private string currentLine;

        private StreamReader inputFile;

        // CONSTRUCTOR
        public Parser(string fileName)
        {
            inputFile = new StreamReader(fileName);
        }

        // METHODS
        public void Advance()
        {
            do
            {
                Regex regex = new Regex("^\\s*|//.*$");
                currentLine = inputFile.ReadLine();
                currentLine = regex.Replace(currentLine, string.Empty).Trim();
            } while (currentLine == string.Empty);
            
            if (currentLine == null)
            {
                Console.WriteLine("string currentLine is null, end of file reached");
            }
        }

        public bool HasMoreCommands()
        {
            return !inputFile.EndOfStream;
        }
    }
}

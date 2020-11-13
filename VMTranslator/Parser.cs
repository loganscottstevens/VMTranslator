using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace VMTranslator
{
    class Parser
    {
        private string currentLine;
        private StreamReader inputFile;

        // PROPERTIES
        public CommandType CommandType { get; private set; }
        public string Arg1 { get; private set; }
        public int Arg2 { get; private set; }

        // STATIC VARIABLES
        // Regular expression for any string which is either a comment or white space
        private static Regex whiteSpaceOrComments = new Regex("^\\s*$|\\s*//.*");

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
                currentLine = inputFile.ReadLine();
                if (currentLine == null)
                {
                    Console.WriteLine("string currentLine is null, end of file reached.");
                    Console.ReadLine();
                    return;
                }

                // Remove comments and empty lines
                currentLine = whiteSpaceOrComments.Replace(currentLine, string.Empty).Trim();
            } while (currentLine == string.Empty);

            string[] args = currentLine.Split(" ");
            if (args.Length == 1)
            {
                CommandType = CommandType.C_ARITHMETIC;
                Arg1 = args[0];
                Arg2 = -1;

            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (i == 0)
                    {
                        CommandType = LookupCommandType(args[i]);
                    }
                    else if (i == 1)
                    {
                        Arg1 = args[i];
                    }
                    else if (i == 2)
                    {
                        Arg2 = int.Parse(args[i]);
                    }
                }
            }
        }

        public bool HasMoreCommands()
        {
            return !inputFile.EndOfStream;
        }

        public string ToString()
        {
            return
                $"CurrentLine = {currentLine}\n" +
                $"CommandType = {CommandType}\n" +
                $"Arg1 = {Arg1}\n" +
                $"Arg2 = {Arg2}\n";
        }

        public static CommandType LookupCommandType(string command)
        {
            Dictionary<string, CommandType> keyValuePairs = new Dictionary<string, CommandType>()
            {
                {"push", CommandType.C_PUSH },
                {"pop", CommandType.C_POP },
                {"add", CommandType.C_ARITHMETIC},
                {"sub", CommandType.C_ARITHMETIC }

            };
            keyValuePairs.TryGetValue(command, out CommandType value);
            return value;
        }
    }
}

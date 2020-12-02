#region Author Info
// ===============================
// AUTHOR          :Logan Stevens
// CREATE DATE     :11/9/20
//================================ 
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace VMTranslator
{
    class Parser
    {
        #region Properties
        private string CurrentLine { get; set; }
        private StreamReader InputFile { get; set; }
        public CommandType CommandType { get; private set; }
        public string Arg1 { get; private set; }
        public int Arg2 { get; private set; } 
        #endregion

        #region Static Variables
        private static readonly Regex whiteSpaceOrComments = new Regex("^\\s*$|\\s*//.*");

        private static readonly Dictionary<string, CommandType> commandTypesDictionary =
            new Dictionary<string, CommandType>()
            {
                {"push", CommandType.C_PUSH },
                {"pop", CommandType.C_POP },
                {"add", CommandType.C_ARITHMETIC},
                {"sub", CommandType.C_ARITHMETIC },
                {"eq", CommandType.C_ARITHMETIC },
                {"gt", CommandType.C_ARITHMETIC },
                {"lt", CommandType.C_ARITHMETIC },
                {"neg", CommandType.C_ARITHMETIC },
                {"and", CommandType.C_ARITHMETIC },
                {"or", CommandType.C_ARITHMETIC },
                {"not", CommandType.C_ARITHMETIC },
                {"label", CommandType.C_LABEL },
                {"goto", CommandType.C_GOTO },
                {"if-goto", CommandType.C_IF },
                {"function", CommandType.C_FUNCTION },
                {"call", CommandType.C_CALL },
                {"return", CommandType.C_RETURN }
            };
        #endregion

        #region Constructor
        public Parser(string fileName)
        {
            InputFile = new StreamReader(fileName);
        } 
        #endregion

        #region Public Methods
        /// <summary>
        /// Description: Advances parser by one line in the input file<br/>
        /// Precondition: Parser has been initialized with a valid filepath to .vm file<br/>
        /// Postcondition: Properties are updated<br/>
        /// </summary>
        public void Advance()
        {
            do
            {
                CurrentLine = InputFile.ReadLine();
                if (CurrentLine == null)
                {
                    Console.WriteLine("string currentLine is null, end of file reached.");
                    Console.ReadLine();
                    return;
                }
                CurrentLine = whiteSpaceOrComments.Replace(CurrentLine, string.Empty).Trim();
            } while (CurrentLine == string.Empty);

            string[] args = CurrentLine.Split(" ");
            CommandType = LookupCommandType(args[0]);
            switch (CommandType)
            {
                // One-argument commands
                case CommandType.C_ARITHMETIC:
                    Arg1 = args[0];
                    Arg2 = -1;
                    break;
                case CommandType.C_RETURN:
                    Arg1 = "No Arg1";
                    Arg2 = -1;
                    break;
                // Two-argument commands
                case CommandType.C_LABEL:
                case CommandType.C_IF:
                case CommandType.C_GOTO:
                    Arg1 = args[1];
                    Arg2 = -1;
                    break;
                // Three-argument commands
                case CommandType.C_PUSH:
                case CommandType.C_POP:
                case CommandType.C_FUNCTION:
                case CommandType.C_CALL:
                    Arg1 = args[1];
                    Arg2 = int.Parse(args[2]);
                    break;
                // Error
                default:
                    Console.WriteLine("Error: No command type found.");
                    break;
            }
        }

        /// <summary>
        /// Description: Determines if input file has more lines to parse<br/>
        /// Precondition: InputFile in not null<br/>
        /// Postcondition: End of file can be determined<br/>
        /// </summary>
        /// <returns>True if there are more commands in input file</returns>
        public bool HasMoreCommands()
        {
            return !InputFile.EndOfStream;
        }

        /// <summary>
        /// Description: Shows parser's current state as a string<br/>
        /// Precondition: A parser must be instantiated<br/>
        /// Postcondition: A string representation is returned<br/>
        /// </summary>
        /// <returns>string of the current command's lexical elements</returns>
        public string ToString()
        {
            return
                $"CurrentLine = {CurrentLine}\n" +
                $"CommandType = {CommandType}\n" +
                $"Arg1 = {Arg1}\n" +
                $"Arg2 = {Arg2}\n";
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Description: Looks up command type from vm file command segment<br/>
        /// Precondition: A parser must be instantiated<br/>
        /// Postcondition: A CommandType is returned<br/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns>CommandType enumeration of given string</returns>
        public static CommandType LookupCommandType(string command)
        {
            commandTypesDictionary.TryGetValue(command, out CommandType value); 
            return value;
        }
        #endregion
    }
}

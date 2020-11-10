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

        }

        public void WritePushPop(CommandType commandType, string segment, int index)
        {

        }

        public void Close()
        {
            outputFile.Close();
        }
    }
}

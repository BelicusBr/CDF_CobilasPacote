using System;

namespace Cobilas.IO.CobilasPackage.CLI {
    internal struct cmd_Debug_InPut {
        public string InPut;
        public ConsoleColor Color;

        public cmd_Debug_InPut(ConsoleColor color, string input) {
            this.Color = color;
            this.InPut = input;
        }

        public cmd_Debug_InPut(ConsoleColor color, char input) :
            this(color, new string(new char[] { input })) { }

        public cmd_Debug_InPut(string input) :
            this(ConsoleColor.Gray, input) { }
        
        public cmd_Debug_InPut(char input) :
            this(ConsoleColor.Gray, input) { }
    }
}

namespace Cobilas.IO.CobilasPackage.CLI {
    internal struct CommandOutput {
        public string Command;
        public string Output;
        public bool IsOutput;
        public bool FinalCommand;

        public CommandOutput(string Command, string Output, bool IsOutput, bool FinalCommand) {
            this.Command = Command;
            this.Output = Output;
            this.IsOutput = IsOutput;
            this.FinalCommand = FinalCommand;
        }

        public CommandOutput(string Command) :
            this(Command, null, false, false) { }
    }
}

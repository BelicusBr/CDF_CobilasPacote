using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Cobilas.IO.CobilasPackage.CLI {

    /*
     * help
     * exit/sair
     * clear/limpar
     * cmd
     * cmd init
     * cmd init pack : {nome do pack}
     * cmd init entry : {caminho do arquivo} : {caminho relativo}
     * cmd focus
     * cmd focus pack {nome do pack}/{indice}
     * cmd show
     * cmd show pack
     * cmd show packs
     * cmd show entries
     * cmd delete
     * cmd delete pack : {nome do pack}/{indice}
     * cmd delete entry {caminho relativo}/{indice}
     * cmd rename
     * cmd rename pack : {nome do pack} : {novo nome do pack}
     * cmd rename entry : {caminho relativo} : {novo caminho relativo}
     * cmd in
     * cmd in pack : {caminho do arquivo pack}
     * cmd out
     * cmd out pack : {nome do pack} : {caminho do arquivo pack}
     * cmd out entry : {caminho relativo} : {caminho do arquivo de saida}
     */

    class Program {
        private static bool exit = false;
        private static List<CobilasPackage> packs = new List<CobilasPackage>();
        private static CobilasPackage focused;
        private static int focusedIndex;
        private static KeyPaths RootCommand = new KeyPaths("Root", new KeyPaths[] {
            new KeyPaths("help", (Action)root_help),
            new KeyPaths("exit/sair", new Action(() => { exit = true; })),
            new KeyPaths("clear/limpar", (Action)Console.Clear),
            new KeyPaths("cmd", new KeyPaths[] {
                new KeyPaths("init", new KeyPaths[] {
                    new KeyPaths("pack", (Action<string>)root_cmd_init_pack),
                    new KeyPaths("entry", (Action<string>)root_cmd_init_entry)
                }),
                new KeyPaths("focus", new KeyPaths[] {
                    new KeyPaths("pack", (Action<string>)root_cmd_focus_pack)
                }),
                new KeyPaths("show", new KeyPaths[] {
                    new KeyPaths("pack", (Action)root_cmd_show_pack),
                    new KeyPaths("packs", (Action)root_cmd_show_packs),
                    new KeyPaths("entries", (Action)root_cmd_show_entries)
                }),
                new KeyPaths("delete",  new KeyPaths[] {
                    new KeyPaths("pack", (Action<string>)root_cmd_delete_pack),
                    new KeyPaths("entry", (Action<string>)root_cmd_delete_entry)
                }),
                new KeyPaths("rename",  new KeyPaths[] {
                    new KeyPaths("pack", (Action<string>)root_cmd_rename_pack),
                    new KeyPaths("entry", (Action<string>)root_cmd_rename_entry)
                }),
                new KeyPaths("in",  new KeyPaths[] {
                    new KeyPaths("pack", (Action<string>)root_cmd_in_pack)
                }),
                new KeyPaths("out",  new KeyPaths[] {
                    new KeyPaths("pack", (Action<string>)root_cmd_out_pack),
                    new KeyPaths("entry", (Action<string>)root_cmd_out_entry)
                })
            })
        });

        static void Main(string[] args) {

            Console.Title = "CLI cobila packages";
            Console.Write("Date {0} ", DateTime.Today.ToShortDateString());
            Console.Write("Time {0}\n", DateTime.Now.ToLongTimeString());
            Console.WriteLine("help");

            try
            {

            if (args.Length != 0)
                foreach (var item in args)
                    CommandLineRunner(CommandLineInterpreter(item));

            while (!exit && args.Length == 0)
                CommandLineRunner(CommandLineInterpreter(Console.ReadLine()));
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                Console.ReadLine();
            }
        }

        //================================metodos in/out
        private static void root_cmd_in_pack(string path) {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = File.OpenRead(path.Trim()))
                packs.Add((CobilasPackage)formatter.Deserialize(stream));
        }

        private static void root_cmd_out_pack(string arg) {
            try {
                string[] itens = arg.Split(':');
                if (ConteinsPack(itens[0].Trim())) {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream stream = File.Create(itens[1].Trim()))
                        formatter.Serialize(stream, packs[IndexOfPack(itens[0].Trim())]);
                } else Console.WriteLine($"package {itens[0].Trim()} does not exist!");
            } catch {
                Console.WriteLine($"argument [{arg}] is invalid!");
            }
        }

        private static void root_cmd_out_entry(string arg) {
            try {
                if (focused == null) {
                    Console.WriteLine("no focused pack");
                    return;
                }
                string[] itens = arg.Split(':');
                if (focused.Contains(itens[0].Trim())) {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream stream = File.Create(itens[1].Trim()))
                        formatter.Serialize(stream, focused[itens[0].Trim()]);
                }
                else Console.WriteLine($"entry {itens[0].Trim()} does not exist!");
            }
            catch
            {
                Console.WriteLine($"argument [{arg}] is invalid!");
            }
        }
        //======================================================

        private static void CommandLineRunner(List<CommandOutput> args) {
            KeyPaths temp = RootCommand;
            for (int I = 0; I < args.Count; I++) {
                temp = temp[args[I].Command];
                if (args[I].FinalCommand) {
                    if (args[I].IsOutput) temp.action.DynamicInvoke(args[I].Output);
                    else temp.action.DynamicInvoke(null);
                }
            }
        }

        private static List<CommandOutput> CommandLineInterpreter(string arg) {
            List<CommandOutput> res = new List<CommandOutput>();
            string[] comm;
            string cml;
            string _value;
            Split(arg, out cml, out _value);
            comm = cml.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int I = 0; I < comm.Length; I++)
                res.Add(new CommandOutput(comm[I].Trim()));

            CommandOutput jut = res[res.Count - 1];
            jut.FinalCommand = true;
            jut.IsOutput = _value != null;
            jut.Output = _value != null ? _value : null;
            res[res.Count - 1] = jut;

            return res;
        }

        private static void Split(string arg, out string CML, out string value) {
            int sharedCommandIndex = arg.IndexOf(':');
            if (sharedCommandIndex == -1) {
                CML = arg;
                value = null;
            } else {
                CML = arg.Remove(sharedCommandIndex).Trim();
                value = arg.Remove(0, sharedCommandIndex + 1).Trim();
            }
        }

        private static void root_cmd_focus_pack(string arg) {
            int res;
            if (int.TryParse(arg.Trim(), out res)) {
                if (res > -1 && res < packs.Count) {
                    focused = packs[res];
                    focusedIndex = res;
                } else Console.WriteLine($"the index {res} is outside the bounds of the array");
            } else {
                if (!ConteinsPack(arg.Trim())) {
                    focused = packs[res = IndexOfPack(arg.Trim())];
                    focusedIndex = res;
                } else Console.WriteLine($"package {arg.Trim()} does not exist!");
            }
        }

        private static void root_cmd_init_pack(string arg) {
            if (arg.Contains(":")) {
                Console.WriteLine($"package name {arg} cannot contain \":\"");
                return;
            }
            if (string.IsNullOrEmpty(arg)) {
                Console.WriteLine("package name cannot be empty");
                return;
            }
            packs.Add(new CobilasPackage(arg.Trim()));
        }

        private static void root_cmd_init_entry(string arg) {
            try {
                if (focused == null) {
                    Console.WriteLine("no focused pack");
                    return;
                }
                string[] itens = arg.Split(':');
                if (!focused.Contains(itens[1].Trim()))
                    focused.Add(itens[1].Trim(), File.ReadAllBytes(itens[0].Trim()));
                else Console.WriteLine($"input {itens[1].Trim()} already exists!");
            } catch {
                Console.WriteLine($"argument [{arg}] is invalid!");
            }
        }

        private static void root_cmd_show_pack() {
            if (focused == null) {
                Console.WriteLine("no focused pack");
                return;
            }
            Console.WriteLine($"pack name: {focused.Name}");
            Console.WriteLine($"pack index: {focusedIndex}");
        }

        private static void root_cmd_show_packs() {
            if (packs.Count == 0) {
                Console.WriteLine("empty pack record");
                return;
            }
            for (int I = 0; I < packs.Count; I++)
                Console.WriteLine($"pack index[{I}] name:{packs[I].Name}");
        }

        private static void root_cmd_show_entries() {
            if (focused == null) {
                Console.WriteLine("no focused pack");
                return;
            }
            for (int I = 0; I < focused.Count; I++)
                Console.WriteLine($"entry index[{I}] name:{focused[I].RelativePath}");
        }

        private static void root_cmd_delete_pack(string arg) {
            int res;
            if (int.TryParse(arg.Trim(), out res)) {
                if (res > -1 && res < packs.Count) packs.RemoveAt(res);
                else Console.WriteLine($"the index {res} is outside the bounds of the array");
            } else {
                if (!ConteinsPack(arg.Trim())) packs.RemoveAt(IndexOfPack(arg.Trim()));
                else Console.WriteLine($"package {arg.Trim()} does not exist!");
            }
        }

        private static void root_cmd_delete_entry(string arg) {
            if (focused == null) {
                Console.WriteLine("no focused pack");
                return;
            }
            int res;
            if (int.TryParse(arg.Trim(), out res)) {
                if (res > -1 && res < focused.Count) focused.Remove(res);
                else Console.WriteLine($"the index {res} is outside the bounds of the array");
            } else {
                if (!focused.Contains(arg.Trim())) focused.Remove(arg.Trim());
                else Console.WriteLine($"entry {arg.Trim()} does not exist!");
            }
        }

        private static void root_cmd_rename_pack(string arg) {
            try {
                string[] itens = arg.Split(':');
                if (ConteinsPack(itens[0].Trim()))
                    packs[IndexOfPack(itens[0].Trim())].Rename(itens[1].Trim());
                else Console.WriteLine($"package {itens[0].Trim()} does not exist!");
            } catch {
                Console.WriteLine($"argument [{arg}] is invalid!");
            }
        }

        private static void root_cmd_rename_entry(string arg) {
            try {
                if (focused == null) {
                    Console.WriteLine("no focused pack");
                    return;
                }
                string[] itens = arg.Split(':');
                if (focused.Contains(itens[0].Trim()))
                    focused[itens[0].Trim()].Rename(itens[1].Trim());
                else Console.WriteLine($"entry {itens[0].Trim()} does not exist!");
            } catch {
                Console.WriteLine($"argument [{arg}] is invalid!");
            }
        }

        private static void root_help() {
            switch (CultureInfo.CurrentCulture.Name) {
                case "pt-BR":
                    break;
                case "en-US":
                    break;
                default:
                    break;
            }
        }

        private static void HelpWriteLine(string CommandType, string msm) {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(CommandType);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" => ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(msm);
            Console.ResetColor();
            Console.Write('\n');
        }

        private static int IndexOfPack(string name) {
            for (int I = 0; I < packs.Count; I++)
                if (packs[I].Name == name)
                    return I;
            return -1;
        }

        private static bool ConteinsPack(string name) {
            foreach (var item in packs)
                if (item.Name == name)
                    return true;
            return false;
        }

        private struct CommandOutput {
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

        private struct KeyPaths {
            public string CommandType;
            public KeyPaths[] cellars;
            public Delegate action;

            public KeyPaths this[string CommandType] {
                get {
                    if (cellars != null)
                        foreach (var item in cellars)
                            if (item.IsKey(CommandType))
                                return item;
                    return new KeyPaths();
                }
            }

            public KeyPaths(string CommandType, Delegate action, KeyPaths[] cellars) {
                this.CommandType = CommandType;
                this.action = action;
                this.cellars = cellars;
            }

            public KeyPaths(string CommandType, Delegate action) :
                this(CommandType, action, (KeyPaths[])null) { }

            public KeyPaths(string CommandType, KeyPaths[] cellars) :
                this(CommandType, (Delegate)null, cellars) { }

            public KeyPaths(string CommandType) :
                this(CommandType, (Delegate)null) { }

            public bool HasAction()
                => action != null;

            public bool IsKey(string CommandType) {
                foreach (var item in this.CommandType.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
                    if (item == CommandType)
                        return true;
                return false;
            }
        }
    }
}

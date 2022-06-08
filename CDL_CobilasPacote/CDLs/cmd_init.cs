using System;
using System.IO;

namespace Cobilas.IO.CobilasPackage.CLI {
    internal struct cmd_init {
        //pack
        //entry

        internal static Action<string> f_root_cmd_init_pack => root_cmd_init_pack;
        internal static Action<string> f_root_cmd_init_entry => root_cmd_init_entry;

        private static void root_cmd_init_pack(string arg) {
            if (arg.Contains(":")) {
                Console.WriteLine($"package name {arg} cannot contain \":\"");
                return;
            }
            if (string.IsNullOrEmpty(arg)) {
                Console.WriteLine("package name cannot be empty");
                return;
            }
            Program.packs.Add(new CobilasPackage(arg.Trim()));
        }

        private static void root_cmd_init_entry(string arg) {
            try {
                if (Program.focused == null) {
                    Console.WriteLine("no focused pack");
                    return;
                }
                string[] itens = arg.Split(':');
                if (!Program.focused.Contains(itens[1].Trim()))
                    Program.focused.Add(itens[1].Trim(), File.ReadAllBytes(itens[0].Trim()));
                else Console.WriteLine($"input {itens[1].Trim()} already exists!");
            } catch {
                Console.WriteLine($"argument [{arg}] is invalid!");
            }
        }
    }
}

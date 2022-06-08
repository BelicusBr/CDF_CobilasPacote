using System;

namespace Cobilas.IO.CobilasPackage.CLI {
    internal struct cmd_rename {
        //pack
        //entry

        internal static Action<string> f_root_cmd_rename_pack => root_cmd_rename_pack;
        internal static Action<string> f_root_cmd_rename_entry => root_cmd_rename_entry;

        private static void root_cmd_rename_pack(string arg) {
            try {
                string[] itens = arg.Split(':');
                if (Program.ConteinsPack(itens[0].Trim()))
                    Program.packs[Program.IndexOfPack(itens[0].Trim())].Rename(itens[1].Trim());
                else Console.WriteLine($"package {itens[0].Trim()} does not exist!");
            } catch {
                Console.WriteLine($"argument [{arg}] is invalid!");
            }
        }

        private static void root_cmd_rename_entry(string arg) {
            try {
                if (Program.focused == null) {
                    Console.WriteLine("no focused pack");
                    return;
                }
                string[] itens = arg.Split(':');
                if (Program.focused.Contains(itens[0].Trim()))
                    Program.focused[itens[0].Trim()].Rename(itens[1].Trim());
                else Console.WriteLine($"entry {itens[0].Trim()} does not exist!");
            } catch {
                Console.WriteLine($"argument [{arg}] is invalid!");
            }
        }
    }
}

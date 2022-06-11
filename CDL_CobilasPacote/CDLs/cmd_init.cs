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
                cmd_Debug.PackageNameInvalid(arg);
                //cmd_Debug.MsmSysLine(cmd_Debug.IsPT_BR ?
                //    new string[] { "O nome do pacote ", $"@{arg}", " não pode conter ", "@\":\"" } :
                //    new string[] { "Package name ", $"@{arg}", " cannot contain ", "@\":\"" });
                return;
            }
            if (string.IsNullOrEmpty(arg)) {
                cmd_Debug.PackageNameEmpty();
                //cmd_Debug.MsmSysLine(cmd_Debug.IsPT_BR ? 
                //    "O nome do pacote não pode ser vazio!" : 
                //    "package name cannot be empty!");
                return;
            }
            if (Program.ConteinsPack(arg.Trim())) {
                cmd_Debug.PackageExists(arg.Trim());
                //cmd_Debug.MsmSysLine(cmd_Debug.IsPT_BR ?
                //    $"O pacote {arg.Trim()} já existe!" :
                //    $"Package {arg.Trim()} exists!");
            } else {
                Console.WriteLine($"Add:{arg.Trim()}");
                Program.packs.Add(new CobilasPackage(arg.Trim()));
            }
        }

        private static void root_cmd_init_entry(string arg) {
            try {
                if (Program.focused == null) {
                    cmd_Debug.NoFocusedPack();
                    return;
                }
                string[] itens = arg.Split(':');
                if (!Program.focused.Contains(itens[1].Trim()))
                    Program.focused.Add(itens[1].Trim(), File.ReadAllBytes(itens[0].Trim()));
                else {
                    cmd_Debug.EntryExists(itens[1].Trim());
                    //cmd_Debug.MsmSysLine(cmd_Debug.IsPT_BR ?
                    //    new string[] { $"Entrada ", $"@{itens[1].Trim()}", " já existe!" } :
                    //    new string[] { $"Entry ", $"@{itens[1].Trim()}", " already exists!" });
                }
            } catch {
                cmd_Debug.ArgumentInvalid(arg);
                //cmd_Debug.MsmArgError(cmd_Debug.IsPT_BR ?
                //    new string[] { "O argumento ", $"@[{arg}]", " é inválido!" } :
                //    new string[] { "argument ", $"@[{arg}]", " is invalid!" });
            }
        }
    }
}

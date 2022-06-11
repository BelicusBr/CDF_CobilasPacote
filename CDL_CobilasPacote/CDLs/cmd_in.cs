using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Cobilas.IO.CobilasPackage.CLI {
    internal struct cmd_in {
        //pack
        //changeContent

        internal static Action<string> f_root_cmd_in_pack => root_cmd_in_pack;
        internal static Action<string> f_cmd_in_file_changeContent => cmd_in_file_changeContent;

        private static void root_cmd_in_pack(string path) {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = File.OpenRead(path.Trim()))
                Program.packs.Add((CobilasPackage)formatter.Deserialize(stream));
        }

        private static void cmd_in_file_changeContent(string arg) {
            try {
                if (Program.focused == null) {
                    cmd_Debug.NoFocusedPack();
                    return;
                }
                string[] itens = arg.Split(':');
                if (Program.focused.Contains(itens[1].Trim()))
                    Program.focused[itens[1].Trim()].ChangeContent(File.ReadAllBytes(itens[0].Trim()));
                else {
                    cmd_Debug.EntryDoesNotExist(itens[1].Trim());
                    //cmd_Debug.MsmSysLine(cmd_Debug.IsPT_BR ?
                    //    new string[] { $"A entrada ", $"@{itens[1].Trim()}", " não existe!" } :
                    //    new string[] { $"entry ", $"@{itens[1].Trim()}", " does not exist!" });
                }
            } catch {
                cmd_Debug.ArgumentInvalid(arg);
                //cmd_Debug.MsmArgError(cmd_Debug.IsPT_BR ?
                //    new string[] { $"O argumento ", $"@[{arg}]", " é inválido!" } :
                //    new string[] { $"argument ", $"@[{arg}]", " is invalid!" });
            }
        }
    }
}

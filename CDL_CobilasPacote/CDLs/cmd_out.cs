using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace Cobilas.IO.CobilasPackage.CLI {
    internal struct cmd_out {
        //pack
        //entry
        //info

        internal static Action<string> f_root_cmd_out_pack => root_cmd_out_pack;
        internal static Action<string> f_root_cmd_out_entry => root_cmd_out_entry;
        internal static Action<string> f_root_cmd_out_info => root_cmd_out_info;

        private static void root_cmd_out_info(string arg) {
            try {
                using (FileStream stream = File.Create(arg.Trim())) {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine("<?xml version=\"1.0\" encoding=\"utf - 8\" ?>");
                    builder.AppendLine("<info>");

                    if (Program.focused != null)
                        builder.AppendLine($"\t<focused name=\"{Program.focused.Name}\" index=\"{Program.focusedIndex}\"/>");

                    builder.AppendLine("\t<packs>");
                    for (int I = 0; I < Program.packs.Count; I++) {
                        builder.AppendLine($"\t\t<pack name=\"{Program.packs[I].Name}\" index=\"{I}\">");
                        for (int J = 0; J < Program.packs[I].Count; J++) {
                            builder.AppendLine($"\t\t\t<entry name=\"{Program.packs[I][J].RelativePath}\" index=\"{J}\">");
                        }
                        builder.AppendLine($"\t\t</pack>");
                    }
                    builder.AppendLine("\t</packs>");

                    builder.AppendLine("</info>");

                    byte[] cont = Encoding.UTF8.GetBytes(builder.ToString());
                    stream.Write(cont, 0, cont.Length);
                }
            } catch (Exception e) {
                cmd_Debug.MsmError("there was a failure to print the information");
                Console.WriteLine();
                cmd_Debug.MsmError(e.ToString());
            }
        }

        private static void root_cmd_out_pack(string arg) {
            try {
                string[] itens = arg.Split(':');
                if (Program.ConteinsPack(itens[0].Trim())) {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream stream = File.Create(itens[1].Trim()))
                        formatter.Serialize(stream, Program.packs[Program.IndexOfPack(itens[0].Trim())]);
                } else cmd_Debug.PackDoesNotExist(itens[0].Trim());
                    //cmd_Debug.MsmSysLine($"package ", $"@{itens[0].Trim()}", " does not exist!");
            } catch {
                cmd_Debug.ArgumentInvalid(arg);
                //cmd_Debug.MsmArgError($"argument ", $"@[{arg}]", " is invalid!");
            }
        }

        private static void root_cmd_out_entry(string arg) {
            try {
                if (Program.focused == null) {
                    cmd_Debug.NoFocusedPack();
                    //cmd_Debug.MsmSysLine("no focused pack");
                    return;
                }
                string[] itens = arg.Split(':');
                if (Program.focused.Contains(itens[0].Trim())) {
                    using (FileStream stream = File.Create(Path.Combine(itens[1].Trim(), itens[0].Trim()))) {
                        ItemFile fileTemp = Program.focused[itens[0].Trim()];
                        stream.Write(fileTemp.Content, 0, fileTemp.Count);
                    }
                } else cmd_Debug.EntryDoesNotExist(arg.Trim());
                    //cmd_Debug.MsmSysLine($"entry ", $"@{itens[0].Trim()}", " does not exist!");
            } catch {
                cmd_Debug.ArgumentInvalid(arg);
                //cmd_Debug.MsmArgError($"argument ", $"@[{arg}]", " is invalid!");
            }
        }
    }
}

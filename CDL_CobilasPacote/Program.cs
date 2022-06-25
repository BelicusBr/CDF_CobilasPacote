using System;
using System.Collections.Generic;

namespace Cobilas.IO.CobilasPackage.CLI {

    // #* => show info pack or entry
    // #@ => show debug system
    internal class Program {
        private static bool exit = false;
        internal static List<CobilasPackage> packs = new List<CobilasPackage>();
        internal static CobilasPackage focused;
        internal static int focusedIndex;
        private static KeyPaths RootCommand = new KeyPaths("Root", new KeyPaths[] {
            new KeyPaths("help/ajuda", root_help.f_root_help),
            new KeyPaths("exit/sair", new Action(() => { exit = true; })),
            new KeyPaths("clear/limpar", (Action)Clear),
            new KeyPaths("cmd", new KeyPaths[] {
                new KeyPaths("init", new KeyPaths[] {
                    new KeyPaths("pack", cmd_init.f_root_cmd_init_pack),
                    new KeyPaths("entry", cmd_init.f_root_cmd_init_entry)
                }),
                new KeyPaths("focus", new KeyPaths[] {
                    new KeyPaths("pack", cmd_focus.f_root_cmd_focus_pack)
                }),
                new KeyPaths("show", new KeyPaths[] {
                    new KeyPaths("pack", cmd_show.f_root_cmd_show_pack),
                    new KeyPaths("packs", cmd_show.f_root_cmd_show_packs),
                    new KeyPaths("entries", cmd_show.f_root_cmd_show_entries)
                }),
                new KeyPaths("delete",  new KeyPaths[] {
                    new KeyPaths("pack", cmd_delete.f_root_cmd_delete_pack),
                    new KeyPaths("entry", cmd_delete.f_root_cmd_delete_entry)
                }),
                new KeyPaths("rename",  new KeyPaths[] {
                    new KeyPaths("pack", cmd_rename.f_root_cmd_rename_pack),
                    new KeyPaths("entry", cmd_rename.f_root_cmd_rename_entry)
                }),
                new KeyPaths("in",  new KeyPaths[] {
                    new KeyPaths("pack", cmd_in.f_root_cmd_in_pack),
                    new KeyPaths("entry", new KeyPaths[] {
                        new KeyPaths("changeContent", cmd_in.f_cmd_in_file_changeContent)
                    })
                }),
                new KeyPaths("out",  new KeyPaths[] {
                    new KeyPaths("pack", cmd_out.f_root_cmd_out_pack),
                    new KeyPaths("entry", cmd_out.f_root_cmd_out_entry),
                    new KeyPaths("info", cmd_out.f_root_cmd_out_info)
                })
            })
        });
        //external commands
        static void Main(string[] args) {
            bool sfxPortableConfirm = false;
            Console.Title = "CLI cobila packages (version:0.1.32)";
            cmd_Debug.MsmDateTime();
            cmd_Debug.MsmTipHelp();

            try {
                if (args.Length != 0) {
                    cmd_Debug.MsmSysLine("External commands");
                    foreach (var item in args) {
                        if (!sfxPortableConfirm)
                            if (item.Contains("<")) {
                                cmd_Debug.MsmSysLine("Portable sfx file");
                                sfxPortableConfirm = true;
                            }
                        Console.WriteLine(item);
                        CommandLineRunner(CommandLineInterpreter(item.Trim('<', '>')));
                    }
                } else cmd_Debug.MsmSysLine("Internal commands");

                while (!exit && args.Length == 0) {
                    string line = Console.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        cmd_Debug.MsmSysLine($"empty argument is not valid!");
                    else CommandLineRunner(CommandLineInterpreter(line));
                }
            } catch (Exception e) {
                cmd_Debug.MsmError(e.ToString());
                _ = Console.ReadLine();
            }
        }

        private static void Clear() {
            Console.Clear();
            cmd_Debug.MsmDateTime();
            cmd_Debug.MsmTipHelp();
        }

        private static void CommandLineRunner(List<CommandOutput> args) {
            KeyPaths temp = RootCommand;
            for (int I = 0; I < args.Count; I++) {
                if (!temp.Contains(args[I].Command)) {
                    cmd_Debug.MsmSysLine(
                        cmd_Debug.IsPT_BR ?
                        new string[] { "O comando ", $"@{args[I].Command}", " é inválido!" } :
                        new string[] { "Command ", $"@{args[I].Command}", " is invalid!" }
                        );
                    break;
                }
                temp = temp[args[I].Command];
                if (args[I].FinalCommand) {
                    if (args[I].IsOutput) temp.action.DynamicInvoke(args[I].Output);
                    else temp.action.DynamicInvoke(null);
                }
            }
        }

        private static List<CommandOutput> CommandLineInterpreter(string arg) {
            List<CommandOutput> res = new List<CommandOutput>();
            try {
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
            } catch (Exception e) {
                if (string.IsNullOrEmpty(arg))
                    cmd_Debug.MsmSysLine("empty argument is not valid!");
                else cmd_Debug.MsmArgError("argument ", $"@[{arg}]", " is not valid!");

                cmd_Debug.MsmError(e.ToString());
            }
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

        internal static int IndexOfPack(string name) {
            for (int I = 0; I < packs.Count; I++)
                if (packs[I].Name == name)
                    return I;
            return -1;
        }

        internal static bool ConteinsPack(string name) {
            foreach (var item in packs)
                if (item.Name == name)
                    return true;
            return false;
        }
    }
}

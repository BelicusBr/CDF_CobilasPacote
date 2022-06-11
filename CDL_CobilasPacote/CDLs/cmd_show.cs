using System;

namespace Cobilas.IO.CobilasPackage.CLI {
    internal struct cmd_show {
        //pack
        //packs
        //entries

        internal static Action f_root_cmd_show_pack => root_cmd_show_pack;
        internal static Action f_root_cmd_show_packs => root_cmd_show_packs;
        internal static Action f_root_cmd_show_entries => root_cmd_show_entries;

        private static void root_cmd_show_pack() {
            if (Program.focused == null) {
                cmd_Debug.NoFocusedPack();
                //cmd_Debug.MsmSysLine("no focused pack");
                return;
            }
            MsmSysLine("focused", Program.focusedIndex, Program.focused.Name);
        }

        private static void root_cmd_show_packs() {
            if (Program.packs.Count == 0) {
                cmd_Debug.MsmSysLine("empty pack record");
                return;
            }
            for (int I = 0; I < Program.packs.Count; I++)
                MsmSysLine("pack", I, Program.packs[I].Name);
        }

        private static void root_cmd_show_entries() {
            if (Program.focused == null) {
                cmd_Debug.NoFocusedPack();
                //cmd_Debug.MsmSysLine("no focused pack");
                return;
            }
            for (int I = 0; I < Program.focused.Count; I++)
                MsmSysLine("entry", I, Program.focused[I].RelativePath);
        }

        private static void MsmSysLine(string type, int index, string value) {
            cmd_Debug.Msm("#* ", ConsoleColor.DarkCyan);
            cmd_Debug.Msm(type, ConsoleColor.DarkGreen);
            cmd_Debug.Msm(" index[", ConsoleColor.Green);
            cmd_Debug.Msm(index.ToString());
            cmd_Debug.Msm("]", ConsoleColor.Green);
            cmd_Debug.Msm(" name:", ConsoleColor.Green);
            cmd_Debug.Msm(value);
            cmd_Debug.Msm("\n");
            Console.ResetColor();
        }
    }
}

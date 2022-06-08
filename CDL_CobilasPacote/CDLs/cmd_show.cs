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
                Console.WriteLine("no focused pack");
                return;
            }
            Console.WriteLine($"pack name: {Program.focused.Name}");
            Console.WriteLine($"pack index: {Program.focusedIndex}");
        }

        private static void root_cmd_show_packs() {
            if (Program.packs.Count == 0) {
                Console.WriteLine("empty pack record");
                return;
            }
            for (int I = 0; I < Program.packs.Count; I++)
                Console.WriteLine($"pack index[{I}] name:{Program.packs[I].Name}");
        }

        private static void root_cmd_show_entries() {
            if (Program.focused == null) {
                Console.WriteLine("no focused pack");
                return;
            }
            for (int I = 0; I < Program.focused.Count; I++)
                Console.WriteLine($"entry index[{I}] name:{Program.focused[I].RelativePath}");
        }
    }
}

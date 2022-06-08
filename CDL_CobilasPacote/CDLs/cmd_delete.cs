using System;

namespace Cobilas.IO.CobilasPackage.CLI {
    internal struct cmd_delete {
        //pack
        //entry
        internal static Action<string> f_root_cmd_delete_pack => root_cmd_delete_pack;
        internal static Action<string> f_root_cmd_delete_entry => root_cmd_delete_entry;

        private static void root_cmd_delete_pack(string arg) {
            int res;
            if (int.TryParse(arg.Trim(), out res)) {
                if (res > -1 && res < Program.packs.Count) Program.packs.RemoveAt(res);
                else Console.WriteLine($"the index {res} is outside the bounds of the array");
            } else {
                if (!Program.ConteinsPack(arg.Trim())) Program.packs.RemoveAt(Program.IndexOfPack(arg.Trim()));
                else Console.WriteLine($"package {arg.Trim()} does not exist!");
            }
        }

        private static void root_cmd_delete_entry(string arg) {
            if (Program.focused == null) {
                Console.WriteLine("no focused pack");
                return;
            }
            int res;
            if (int.TryParse(arg.Trim(), out res)) {
                if (res > -1 && res < Program.focused.Count) Program.focused.Remove(res);
                else Console.WriteLine($"the index {res} is outside the bounds of the array");
            } else {
                if (!Program.focused.Contains(arg.Trim())) Program.focused.Remove(arg.Trim());
                else Console.WriteLine($"entry {arg.Trim()} does not exist!");
            }
        }
    }
}

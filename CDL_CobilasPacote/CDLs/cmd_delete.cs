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
                else cmd_Debug.IndexOutsideTheMatrix(res.ToString());
                    //cmd_Debug.MsmSysLine($"the index ", $"@{res}", " is outside the bounds of the array");
            } else {
                if (!Program.ConteinsPack(arg.Trim())) Program.packs.RemoveAt(Program.IndexOfPack(arg.Trim()));
                else cmd_Debug.PackDoesNotExist(arg);
                    //cmd_Debug.MsmSysLine($"package ", $"@{arg.Trim()}", " does not exist!");
            }
        }

        private static void root_cmd_delete_entry(string arg) {
            if (Program.focused == null) {
                cmd_Debug.NoFocusedPack();
                return;
            }
            int res;
            if (int.TryParse(arg.Trim(), out res)) {
                if (res > -1 && res < Program.focused.Count) Program.focused.Remove(res);
                else cmd_Debug.IndexOutsideTheMatrix(res.ToString());
                //cmd_Debug.MsmSysLine($"the index ", $"@{res}", " is outside the bounds of the array");
            } else {
                if (!Program.focused.Contains(arg.Trim())) Program.focused.Remove(arg.Trim());
                else cmd_Debug.EntryDoesNotExist(arg.Trim());
                    //cmd_Debug.MsmSysLine($"entry ", $"@{arg.Trim()}", " does not exist!");
            }
        }
    }
}

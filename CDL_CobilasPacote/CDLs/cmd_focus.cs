using System;

namespace Cobilas.IO.CobilasPackage.CLI {
    internal struct cmd_focus {
        //pack
        internal static Action<string> f_root_cmd_focus_pack => root_cmd_focus_pack;

        private static void root_cmd_focus_pack(string arg) {
            int res;
            if (int.TryParse(arg.Trim(), out res)) {
                if (res > -1 && res < Program.packs.Count) {
                    Program.focused = Program.packs[res];
                    Program.focusedIndex = res;
                } else cmd_Debug.IndexOutsideTheMatrix(res.ToString());
                //cmd_Debug.MsmSysLine($"the index ", $"@{res}", " is outside the bounds of the array!");
            } else {
                if (!Program.ConteinsPack(arg.Trim())) {
                    Program.focused = Program.packs[res = Program.IndexOfPack(arg.Trim())];
                    Program.focusedIndex = res;
                } else cmd_Debug.PackDoesNotExist(arg);
                //cmd_Debug.MsmSysLine($"package ", $"@{arg.Trim()}", " does not exist!");
            }
        }
    }
}

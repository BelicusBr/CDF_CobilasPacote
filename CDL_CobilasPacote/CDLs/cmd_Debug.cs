using System;
using System.Globalization;

namespace Cobilas.IO.CobilasPackage.CLI {
    internal struct cmd_Debug {

        public static bool IsPT_BR => CultureInfo.CurrentCulture.Name == "pt-BR";

        public static void Msm(params cmd_Debug_InPut[] inPuts) {
            foreach (var item in inPuts) {
                Console.ForegroundColor = item.Color;
                Console.Write(item.InPut);
            }
            Console.ResetColor();
        }

        public static void Msm(string msm, ConsoleColor color)
            => Msm(new cmd_Debug_InPut(color, msm));

        public static void Msm(string msm)
            => Msm(msm, ConsoleColor.Gray);

        public static void MsmLine(string msm, ConsoleColor color)
            => Msm(
                new cmd_Debug_InPut(color, msm),
                new cmd_Debug_InPut('\n')
                );

        public static void MsmLine(string msm)
            => MsmLine(msm, ConsoleColor.Gray);

        public static void MsmSys(string msm) {
            Msm("#@ ", ConsoleColor.DarkCyan);
            Msm(msm, ConsoleColor.DarkGray);
        }

        public static void MsmSysLine(string msm) {
            Msm("#@ ", ConsoleColor.DarkCyan);
            MsmLine(msm, ConsoleColor.DarkGray);
        }
        //help to show command list
        public static void MsmTipHelp() {
            Msm("#@ ", ConsoleColor.DarkCyan);
            Msm("help/ajuda ", ConsoleColor.Gray);
            MsmLine(IsPT_BR ? "para mostrar uma lista de comando!" : "to show command list!", ConsoleColor.DarkGray);
        }

        /// <param name="format">| => separator</param>
        public static void MsmArgError(params string[] arg)
            => MsmSysLine(ConsoleColor.Gray, ConsoleColor.Red, arg);

        public static void MsmError(string msm) {
            Msm("#@ ", ConsoleColor.DarkCyan);
            MsmLine(msm, ConsoleColor.Red);
        }

        public static void MsmDateTime() {
            Msm("#@ ", ConsoleColor.DarkCyan);
            Msm(IsPT_BR ? "Data:" : "Date:", ConsoleColor.Green);
            Msm(DateTime.Today.ToShortDateString());
            Msm(" ");
            Msm(IsPT_BR ? "Hora:" : "Time:", ConsoleColor.Green);
            MsmLine(DateTime.Now.ToLongTimeString());
        }

        /// <param name="args">@ => used to modify the color</param>
        public static void MsmSysLine(params string[] args)
            => MsmSysLine(ConsoleColor.DarkGray, ConsoleColor.Gray, args);

        /// <param name="args">@ => used to modify the color</param>
        public static void MsmSysLine(ConsoleColor withModifier, ConsoleColor noModifier, params string[] args) {
            cmd_Debug_InPut[] inPuts = new cmd_Debug_InPut[args.Length + 2];
            inPuts[0] = new cmd_Debug_InPut(ConsoleColor.DarkCyan, "#@ ");

            for (int I = 0; I < args.Length; I++) {
                if (args[I][0] == '@')
                    inPuts[I + 1] = new cmd_Debug_InPut(noModifier, args[I].Remove(0, 1));
                else inPuts[I + 1] = new cmd_Debug_InPut(withModifier, args[I]);
            }

            inPuts[inPuts.Length - 1] = new cmd_Debug_InPut(ConsoleColor.Gray, '\n');
            Msm(inPuts);
        }

        public static void NoFocusedPack()
            => MsmSysLine(IsPT_BR ? "pack não focado!" : "no focused pack!");

        public static void EntryExists(string arg)
            => MsmSysLine(IsPT_BR ?
                        new string[] { $"Entrada ", $"@{arg}", " já existe!" } :
                        new string[] { $"Entry ", $"@{arg}", " already exists!" });

        public static void PackageExists(string arg)
            => MsmSysLine(IsPT_BR ?
                    new string[] { $"O pacote ", $"@{arg}", " já existe!" } :
                    new string[] { $"Package ", $"@{arg}", " exists!" });

        public static void PackageNameInvalid(string arg)
            => MsmSysLine(IsPT_BR ?
                    new string[] { "O nome do pacote ", $"@{arg}", " não pode conter ", "@\":\"" } :
                    new string[] { "Package name ", $"@{arg}", " cannot contain ", "@\":\"" });

        public static void PackageNameEmpty()
            => MsmSysLine(IsPT_BR ?
                    "O nome do pacote não pode ser vazio!" :
                    "package name cannot be empty!");

        public static void IndexOutsideTheMatrix(string arg)
            => MsmSysLine(IsPT_BR ?
                new string[] { $"O indice ", $"@{arg}", " está fora dos limites da matriz" } :
                new string[] { $"the index ", $"@{arg}", " is outside the bounds of the array" } );

        public static void PackDoesNotExist(string arg)
            => MsmSysLine(IsPT_BR ?
                new string[] { $"O pacote ", $"@{arg.Trim()}", " não existe!" } :
                new string[] { $"package ", $"@{arg.Trim()}", " does not exist!" });

        public static void EntryDoesNotExist(string arg)
            => MsmSysLine(IsPT_BR ?
                        new string[] { $"A entrada ", $"@{arg}", " não existe!" } :
                        new string[] { $"entry ", $"@{arg}", " does not exist!" });

        public static void ArgumentInvalid(string arg)
            => MsmArgError(IsPT_BR ?
                    new string[] { $"O argumento ", $"@[{arg}]", " é inválido!" } :
                    new string[] { $"argument ", $"@[{arg}]", " is invalid!" });
    }
}

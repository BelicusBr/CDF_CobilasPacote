using System;

namespace Cobilas.IO.CobilasPackage.CLI {
    /*
     * help/ajuda => mostra uma lista de comandos
     * exit/sair => sair do cmd
     * clear/limpar => limpar a tela do cmd
     * cmd => inicializa uma linha de comando
     * cmd init pack : {nome do pack} => inicia um novo pacote
     * cmd init entry : {caminho do arquivo} : {caminho relativo} => inicia uma nova entrada
     * cmd focus pack : {nome do pack}/{indice} => foca no pacote para ser manipulado
     * cmd show pack => mostra o pacote que está focado
     * cmd show packs => mostra os pacotes que estão registrados
     * cmd show entries => mostra as entradas que estão alocadas no pacote que está focado
     * cmd delete pack : {nome do pack}/{indice} => deletar o pacote
     * cmd delete entry : {caminho relativo}/{indice} => deleta a entrada do pacote que está focado
     * cmd rename pack : {nome do pack} : {novo nome do pack} => renomeá um pacote
     * cmd rename entry : {caminho do arquivo} : {caminho relativo} => renomeá uma entrada do pacote que está focado
     * cmd in pack : {caminho do arquivo pack} => deserializa um arquivo de pacote
     * cmd in entry changeContent : {caminho do arquivo} : {caminho relativo} => modifica o conteudo da entrada de um pacote que está focado
     * cmd out pack : {nome do pack} : {caminho do arquivo pack} => serializa um pacote num arquivo de pacote
     * cmd out entry : {caminho relativo} : {caminho do diretório de saída} => serializa a entrada num diretório especificado
     * cmd out info : {caminho do arquivo de saída} => gera uma saida com informações em .xml
     */
    internal struct root_help {

        internal static Action f_root_help => m_root_help;

        private static void m_root_help() {
            if(cmd_Debug.IsPT_BR) root_help_pt_BR();
            else root_help_en_US();
        }

        private static void root_help_pt_BR() {
            HelpWriteLine("help/ajuda", "mostra uma lista de comandos");
            HelpWriteLine("exit/sair", "sair do cmd");
            HelpWriteLine("clear/limpar", "limpar a tela do cmd");
            HelpWriteLine("cmd", "inicializa uma linha de comando");

            PrintHelpCommandLine("cmd init pack", "nome do pack", "inicia um novo pacote");
            PrintHelpCommandLine_and("cmd init entry", "caminho do arquivo", "caminho relativo", "inicia uma nova entrada");

            PrintHelpCommandLine_or("cmd focus pack", "nome do pack", "indice", "foca no pacote para ser manipulado");

            HelpWriteLine("cmd show pack", "mostra o pacote que está focado");
            HelpWriteLine("cmd show packs", "mostra os pacotes que estão registrados");
            HelpWriteLine("cmd show entries", "mostra as entradas que estão alocadas no pacote que está focado");

            PrintHelpCommandLine_or("cmd delete pack", "nome do pack", "indice", "deletar o pacote");
            PrintHelpCommandLine_or("cmd delete entry", "caminho relativo", "indice", "deleta a entrada do pacote que está focado");

            PrintHelpCommandLine_and("cmd rename pack", "nome do pack", "novo nome do pack", "renomeá um pacote");
            PrintHelpCommandLine_and("cmd rename entry", "caminho do arquivo", "caminho relativo", "renomeá uma entrada do pacote que está focado");

            PrintHelpCommandLine("cmd in pack", "caminho do arquivo pack", "deserializa um arquivo de pacote");
            PrintHelpCommandLine_and("cmd in entry changeContent", "caminho do arquivo", "caminho relativo",
                "modifica o conteudo da entrada de um pacote que está focado");

            PrintHelpCommandLine_and("cmd out pack", "nome do pack", "caminho do arquivo pack", "serializa um pacote num arquivo de pacote");
            PrintHelpCommandLine_and("cmd out entry", "caminho relativo", "caminho do diretório de saída", "serializa a entrada num diretório especificado");
            PrintHelpCommandLine("cmd out info", "caminho do arquivo de saída", "gera uma saida com informações em .xml");
        }

        private static void root_help_en_US() {
            HelpWriteLine("help/ajuda", "show a list of commands");
            HelpWriteLine("exit/sair", "exit cmd");
            HelpWriteLine("clear/limpar", "clear cmd screen");
            HelpWriteLine("cmd", "initialize a command line");

            PrintHelpCommandLine("cmd init pack", "pack name", "start a new package");
            PrintHelpCommandLine_and("cmd init entry", "file path", "relative path", "start a new entry");

            PrintHelpCommandLine_or("cmd focus pack", "pack name", "index", "focus on the package to be manipulated");

            HelpWriteLine("cmd show pack", "shows the package that is focused");
            HelpWriteLine("cmd show packs", "shows the packages that are registered");
            HelpWriteLine("cmd show entries", "shows the entries that are allocated in the focused package");

            PrintHelpCommandLine_or("cmd delete pack", "pack name", "index", "delete the package");
            PrintHelpCommandLine_or("cmd delete entry", "relative path", "index", "delete the package entry that is focused");

            PrintHelpCommandLine_and("cmd rename pack", "pack name", "new pack name", "rename a package");
            PrintHelpCommandLine_and("cmd rename entry", "relative path", "new relative path", "rename an entry of the package that is focused");

            PrintHelpCommandLine("cmd in pack", "pack file path", "deserialize a package file");
            PrintHelpCommandLine_and("cmd in entry changeContent", "file path", "relative path",
                "modifies the contents of the input of a package that is focused");

            PrintHelpCommandLine_and("cmd out pack", "nome do pack", "caminho do arquivo pack", "serializes a package into a package file");
            PrintHelpCommandLine_and("cmd out entry", "relative path", "output directory path", "serialize the entry in the specified directory");
            PrintHelpCommandLine("cmd out info", "output file path", "generates an output with information in .xml");
        }

        private static void HelpWriteLine(string CommandType, string msm) {
            cmd_Debug.Msm(
                new cmd_Debug_InPut(ConsoleColor.DarkCyan, "#@ "),
                new cmd_Debug_InPut(ConsoleColor.DarkGreen, CommandType),
                new cmd_Debug_InPut(" => "),
                new cmd_Debug_InPut(ConsoleColor.DarkGray, msm),
                new cmd_Debug_InPut('\n')
                );
            Console.ResetColor();
        }

        private static void PrintHelpCommandLine_or(string CommandType, string cliv1, string cliv2, string msm) {
            cmd_Debug.Msm(
                new cmd_Debug_InPut(ConsoleColor.DarkCyan, "#@ "),
                new cmd_Debug_InPut(ConsoleColor.DarkGreen, CommandType),
                new cmd_Debug_InPut(ConsoleColor.DarkRed, " : "),
                new cmd_Debug_InPut(ConsoleColor.DarkGray, "{"),
                new cmd_Debug_InPut(cliv1),
                new cmd_Debug_InPut(ConsoleColor.DarkGray, "}"),
                new cmd_Debug_InPut(ConsoleColor.DarkRed, "/"),
                new cmd_Debug_InPut(ConsoleColor.DarkGray, "{"),
                new cmd_Debug_InPut(cliv2),
                new cmd_Debug_InPut(ConsoleColor.DarkGray, "}"),
                new cmd_Debug_InPut(" => "),
                new cmd_Debug_InPut(ConsoleColor.DarkGray, msm),
                new cmd_Debug_InPut('\n')
                );
            Console.ResetColor();
        }

        private static void PrintHelpCommandLine_and(string CommandType, string cliv1, string cliv2, string msm) {
            cmd_Debug.Msm(
                new cmd_Debug_InPut(ConsoleColor.DarkCyan, "#@ "),
                new cmd_Debug_InPut(ConsoleColor.DarkGreen, CommandType),
                new cmd_Debug_InPut(ConsoleColor.DarkRed, " : "),
                new cmd_Debug_InPut(ConsoleColor.DarkGray, "{"),
                new cmd_Debug_InPut(cliv1),
                new cmd_Debug_InPut(ConsoleColor.DarkGray, "}"),
                new cmd_Debug_InPut(ConsoleColor.DarkRed, " : "),
                new cmd_Debug_InPut(ConsoleColor.DarkGray, "{"),
                new cmd_Debug_InPut(cliv2),
                new cmd_Debug_InPut(ConsoleColor.DarkGray, "}"),
                new cmd_Debug_InPut(" => "),
                new cmd_Debug_InPut(ConsoleColor.DarkGray, msm),
                new cmd_Debug_InPut('\n')
                );
            Console.ResetColor();
        }

        private static void PrintHelpCommandLine(string CommandType, string cliv, string msm) {
            cmd_Debug.Msm(
                new cmd_Debug_InPut(ConsoleColor.DarkCyan, "#@ "),
                new cmd_Debug_InPut(ConsoleColor.DarkGreen, CommandType),
                new cmd_Debug_InPut(ConsoleColor.DarkRed, " : "),
                new cmd_Debug_InPut(ConsoleColor.DarkGray, "{"),
                new cmd_Debug_InPut(cliv),
                new cmd_Debug_InPut(ConsoleColor.DarkGray, "}"),
                new cmd_Debug_InPut(" => "),
                new cmd_Debug_InPut(ConsoleColor.DarkGray, msm),
                new cmd_Debug_InPut('\n')
                );
            Console.ResetColor();
        }
    }
}

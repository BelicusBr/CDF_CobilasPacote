using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Cobilas.IO.CobilasPackage.CLI {

    class Program {
        private static bool exit = false;
        private static List<CobilasPackage> packs = new List<CobilasPackage>();
        private static CobilasPackage focused;
        private static int focusedIndex;

        static void Main(string[] args) {

            Console.Title = "CLI cobila packages";
            Console.Write("Date {0} ", DateTime.Today.ToShortDateString());
            Console.Write("Time {0}\n", DateTime.Now.ToLongTimeString());
            Console.WriteLine("help");

            if (args.Length != 0)
                foreach (var item in args)
                    GetCommand(item);

            while (!exit && args.Length == 0)
                GetCommand(Console.ReadLine());
        }

        private static void CmdHelp() {
            switch (CultureInfo.CurrentCulture.Name) {
                case "pt-BR":
                    HelpWriteLine("cmd", "iniciação de comando");
                    HelpWriteLine("help", "exibir uma lista de comandos");
                    HelpWriteLine("exit/sair", "comando de saída do cmd");
                    HelpWriteLine("clear/limpar", "comando para limpar o cmd");
                    HelpWriteLine("cmd pack init {nome do pack}", "inicia um novo pack");
                    HelpWriteLine("cmd pack focused {nome do pack}/{indice}", "focar no pack para ser manípulado");
                    HelpWriteLine("cmd pack showPacks", "mostrar todos os packs registrados");
                    HelpWriteLine("cmd pack showFocusedItem", "mostrar o pack que está focado");
                    HelpWriteLine("cmd pack rename \"{nome do pack}\" to \"{novo nome do pack}\"", "renomeá um pack");
                    HelpWriteLine("cmd pack delete {nome do pack}/{indice}", "remova um pack registrado");
                    HelpWriteLine("cmd pack in {caminho do arquivo pack}", "deserializa um arquivo pack");
                    HelpWriteLine("cmd pack out \"{nome do pack}\" to \"{caminho do arquivo pack}\"", "serializa pack para um arquivo pack");
                    HelpWriteLine("cmd file init \"{caminho do arquivo}\" to \"{caminho relativo da entrada}\"", "inicia uma nova entrada no pack");
                    HelpWriteLine("cmd file showFiles", "mostrar todas as entradas no pack que esta focado");
                    HelpWriteLine("cmd file delete {caminho relativo da entrada}/{indice}", "remover uma entrada do pack");
                    HelpWriteLine("cmd file rename \"{caminho relativo da entrada}\" to \"{novo caminho relativo da entrada}\"", "renomeá uma entrada");
                    break;
                case "en-US":
                    HelpWriteLine("cmd", "command initiation");
                    HelpWriteLine("help", "display a list of commands");
                    HelpWriteLine("exit/sair", "cmd output command");
                    HelpWriteLine("clear/limpar", "command to clean cmd");
                    HelpWriteLine("cmd pack init {nome do pack}", "start a new pack");
                    HelpWriteLine("cmd pack focused {nome do pack}/{indice}", "focus on the pack to be manipulated");
                    HelpWriteLine("cmd pack showPacks", "show all registered packs");
                    HelpWriteLine("cmd pack showFocusedItem", "show the pack that is focused");
                    HelpWriteLine("cmd pack rename \"{nome do pack}\" to \"{novo nome do pack}\"", "rename a pack");
                    HelpWriteLine("cmd pack delete {nome do pack}/{indice}", "remove a registered pack");
                    HelpWriteLine("cmd pack in {caminho do arquivo pack}", "deserialize a pack file");
                    HelpWriteLine("cmd pack out \"{nome do pack}\" to \"{caminho do arquivo pack}\"", "serializes pack to a pack file");
                    HelpWriteLine("cmd file init \"{caminho do arquivo}\" to \"{caminho relativo da entrada}\"", "starts a new entry in the pack");
                    HelpWriteLine("cmd file showFiles", "show all entries in the pack that is focused");
                    HelpWriteLine("cmd file delete {caminho relativo da entrada}/{indice}", "remove an entry from the pack");
                    HelpWriteLine("cmd file rename \"{caminho relativo da entrada}\" to \"{novo caminho relativo da entrada}\"", "rename an entry");
                    break;
                default:
                    break;
            }
        }

        private static void HelpWriteLine(string CommandType, string msm) {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(CommandType);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" => ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(msm);
            Console.ResetColor();
            Console.Write('\n');
        }

        private static int IndexOfPack(string name) {
            for (int I = 0; I < packs.Count; I++)
                if (packs[I].Name == name)
                    return I;
            return -1;
        }

        private static bool ConteinsPack(string name) {
            foreach (var item in packs)
                if (item.Name == name)
                    return true;
            return false;
        }

        private static void CutBetweenTo(string arg, out string arg1, out string arg2) {
            int doubleQuote = arg.IndexOf('"', 1);
            int index = arg.IndexOf("to", doubleQuote);
            arg1 = arg.Remove(index).Trim('"', ' ');
            arg2 = arg.Remove(0, index);
            arg2 = arg2.Remove(0, arg2.IndexOf(' ') + 1).Trim('"', ' ');
        }

        private static void GetCommandType(string arg, out string CommandType, out string newArg) {
            int index = arg.IndexOf(' ');
            if (index == -1) {
                CommandType = arg;
                newArg = "";
            } else {
                CommandType = arg.Remove(index);
                newArg = arg.Remove(0, index + 1);
            }
        }

        private static void GetCommand(string arg) {
            string CommandType;
            string newArg;
            GetCommandType(arg, out CommandType, out newArg);
            switch (CommandType) {
                case "exit":
                case "sair":
                    exit = true;
                    break;
                case "clear":
                case "limpar":
                    Console.Clear();
                    break;
                case "help":
                    CmdHelp();
                    break;
                case "cmd":
                    CmdCommand(newArg);
                    break;
                default:
                    Console.WriteLine($"argument {CommandType} invalid");
                    break;
            }
        }

        private static void CmdCommand(string arg) {
            string CommandType;
            string newArg;
            GetCommandType(arg, out CommandType, out newArg);
            switch (CommandType) {
                case "pack":
                    CmdPackCommand(newArg);
                    break;
                case "file":
                    CmdFileCommand(newArg);
                    break;
                default:
                    Console.WriteLine($"argument {CommandType} invalid");
                    break;
            }
        }

        private static void CmdFileCommand(string arg) {
            string CommandType;
            string newArg;
            int res;
            GetCommandType(arg, out CommandType, out newArg);
            switch (CommandType) {
                case "init":
                    string path;
                    string relativePath;
                    CutBetweenTo(newArg, out path, out relativePath);
                    if (!focused.Contains(relativePath))
                        focused.Add(relativePath, File.ReadAllBytes(path));
                    else Console.WriteLine($"{relativePath} exists!");
                    break;
                case "showFiles":
                    for (int I = 0; I < focused.Count; I++)
                        Console.WriteLine("file N[{0}] name:{1}",I, focused[I].RelativePath);
                    break;
                case "delete":
                    if (int.TryParse(newArg, out res)) {
                        if (res > -1 && res < packs.Count)
                            packs.RemoveAt(res);
                        else
                            Console.WriteLine($"item at index {res} does not exist!");
                    } else {
                        if (ConteinsPack(newArg))
                            packs.RemoveAt(IndexOfPack(newArg));
                        else
                            Console.WriteLine($"item {newArg} does not exist!");
                    }
                    break;
                case "rename":
                    string oldName;
                    string newName;
                    CutBetweenTo(newArg, out oldName, out newName);
                    focused[oldName].Rename(newName);
                    break;
                default:
                    Console.WriteLine($"argument {CommandType} invalid");
                    break;
            }
        }

        private static CobilasPackage DeserializeCobilasPackage(string path) {
            BinaryFormatter formatter = new BinaryFormatter();
            CobilasPackage res = null;
            using (FileStream stream = File.OpenRead(path))
                res = (CobilasPackage)formatter.Deserialize(stream);
            return res;
        }

        private static void SerializeCobilasPackage(CobilasPackage package, string path) {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = File.Create(path))
                formatter.Serialize(stream, package);
        }

        private static void CmdPackCommand(string arg) {
            string CommandType;
            string newArg;
            int res;
            GetCommandType(arg, out CommandType, out newArg);
            switch (CommandType) {
                case "init":
                    if (!ConteinsPack(newArg))
                        packs.Add(new CobilasPackage(newArg));
                    else Console.WriteLine($"Pack {newArg} exist!");
                    break;
                case "in":
                    packs.Add(DeserializeCobilasPackage(newArg));
                    break;
                case "out":
                    string packName;
                    string pathFilePack;
                    CutBetweenTo(newArg, out packName, out pathFilePack);
                    SerializeCobilasPackage(packs[IndexOfPack(packName)] , pathFilePack);
                    break;
                case "focused":
                    if (int.TryParse(newArg, out res)) {
                        if (res > -1 && res < packs.Count) {
                            focused = packs[res];
                            focusedIndex = res;
                        } else
                            Console.WriteLine($"item at index {res} does not exist!");
                    } else {
                        if (ConteinsPack(newArg)) {
                            focusedIndex = IndexOfPack(newArg);
                            focused = packs[focusedIndex];
                        } else
                            Console.WriteLine($"item {newArg} does not exist!");
                    }
                    break;
                case "showPacks":
                    for (int I = 0; I < packs.Count; I++)
                        Console.WriteLine("pack N[{0}] name:{1}", I, packs[I].Name);
                    break;
                case "showFocusedItem":
                    Console.WriteLine($"Item: {focused.Name}");
                    Console.WriteLine($"Index item: {focusedIndex}");
                    break;
                case "rename":
                    string oldName;
                    string newName;
                    CutBetweenTo(newArg, out oldName, out newName);
                    packs[IndexOfPack(oldName)].Rename(newName);
                    break;
                case "delete":
                    if (int.TryParse(newArg, out res)) {
                        if (res > -1 && res < packs.Count)
                            packs.RemoveAt(res);
                        else
                            Console.WriteLine($"item at index {res} does not exist!");
                    } else {
                        if (ConteinsPack(newArg))
                            packs.RemoveAt(IndexOfPack(newArg));
                        else
                            Console.WriteLine($"item {newArg} does not exist!");
                    }
                    break;
                default:
                    Console.WriteLine($"{CommandType} invalid command type");
                    break;
            }
        }
    }
}

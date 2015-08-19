﻿/*==============================*\
 *  TeamSpeak³ Management Bot   *
 *   KCST - Thomas P. Kerman    *
 * http://kerbalspaceprogram.de *
\*==============================*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TeamSpeakE.Utility;

namespace TeamSpeakE
{
    namespace Commands
    {
        // Kommandozeilenkommandos, die der Bot nutzen kann
        public class CommandHandler
        {
            [Command("exit", "Stoppt den TS³-Bot.")]
            public static void Exit(string[] args)
            {
                // Sch(!)ieße die Anwendung ;D
                Environment.Exit(0);
            }

            [Command("help", "Zeigt eine Beschreibung der verfügbaren Befehle an.")]
            public static void Help(string[] args)
            {
                // Wenn ein spezielles Kommando dargestellt werden soll..
                if (args.Length > 0)
                {
                    // Stelle alle Command-Attribute dar
                    foreach (Command c in GetCommands().Where(c => args.Contains(c.name)))
                    {
                        Logging.Log(c.name + ": " + c.description);
                    }
                }
                else
                {
                    // Stelle alle Command-Attribute dar
                    foreach (Command c in GetCommands())
                    {
                        Logging.Log(c.name + ": " + c.description);
                    }
                }
            }

            // Findet alle Command-Attribute
            public static Command[] GetCommands()
            {
                // Finde alle Methoden
                MethodInfo[] infos = typeof(CommandHandler).GetMethods();
                List<Command> commands = new List<Command>();

                // Finde dir Command-Attribute
                foreach (MethodInfo info in infos)
                {
                    foreach (Command c in info.GetCustomAttributes(typeof(Command), true))
                    {
                        c.method = info;
                        commands.Add(c);
                    }
                }

                // Return
                return commands.ToArray();
            }

            // Prüft die Kommandozeile auf Kommandos
            public static void CheckCommands()
            {
                // Lese den aktuellen Befehl aus der Kommandozeile
                string command = Console.ReadLine();

                // Prüfe ob es ein Kommando ist
                if (command.StartsWith("/"))
                {
                    // Formatierung
                    List<string> commands = command.Remove(0, 1).Split(' ').ToList();

                    // Versuche das Kommando zu finden
                    Logging.Log(commands.First());
                    Command cmd = GetCommands().FirstOrDefault(c => c.name == commands.First());

                    // Wenn der Befehl falsch ist, abbrechen
                    if (cmd == null)
                    {
                        Logging.LogError("Kommando nicht gefunden: " + commands.First());
                        return;
                    }
                    else
                    {
                        // Führe die gespeicherte Methode aus
                        MethodInfo method = cmd.method;
                        commands.RemoveAt(0);
                        method.Invoke(null, new[] { commands.ToArray() });
                    }
                }
                else
                {
                    Logging.LogError("Kommando ungültig: " + command);
                    return;
                }
            }
        }   

        // Attribut um ein Kommando zu definieren
        public class Command : Attribute
        {
            public string name;
            public string description;
            public MethodInfo method;

            public Command (string name, string description)
            {
                this.name = name;
                this.description = description;
            }
        }
    }
}
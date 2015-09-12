/*==============================*\
 *  TeamSpeak³ Management Bot   *
 *   KCST - Thomas P. Kerman    *
 * http://kerbalspaceprogram.de *
\*==============================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TeamSpeakBot.ConnectionWorker;
using TeamSpeakBot.Utility;

namespace TeamSpeakBot
{
    namespace Commands
    {
        // Kommandozeilenkommandos, die der Bot nutzen kann
        public class CommandHandler
        {
            [Command("exit", "Stoppt den TS³-Bot.")]
            public static void Exit(string[] args)
            {
                // Stoppe die Verbindungen
                TS3Worker.fetch.CloseConnection();

                // Sch(!)ieße die Anwendung ;D
                Environment.Exit(0);
            }

            [Command("disconnect", "Trennt die Verbindung zum Server")]
            public static void Disconnect(string[] args)
            {
                // Prüfe ob eine Verbindung vorhanden ist
                if (TS3Worker.fetch.connected)
                {
                    TS3Worker.fetch.CloseConnection();
                }
                else
                {
                    Logging.LogWarning("Keine aktive Serververbindung gefunden!");
                }
            }

            [Command("connect", "Stellt eine Verbindung zu einem Server her", args = new[] { "host", "queryPort", "serverPort", "name", "pw" })]
            public static void Connect(string[] args)
            {
                // Wenn eine spezielle Verbindung angegeben wird
                if (args.Length == 5)
                {
                    ConnectionSettings connection = new ConnectionSettings()
                    {
                        host = args[0],
                        queryPort = ushort.Parse(args[1]),
                        serverPort = ushort.Parse(args[2]),
                        username = args[3],
                        password = args[4]
                    };
                    TS3Worker.CreateWorker(connection).OpenConnection();
                }
                else if (args.Length == 1 && args[0] == "default")
                {
                    TS3Worker.CreateWorker(TeamSpeakBot.connection).OpenConnection();
                }
                else
                {
                    TS3Worker.fetch.OpenConnection();
                }
            }

            [Command("help", "Beschreibung verfügbarer Befehle.", args = new[] { "<command>" })]
            public static void Help(string[] args)
            {
                // Wenn ein spezielles Kommando dargestellt werden soll..
                if (args.Length > 0)
                {
                    // Stelle alle Command-Attribute dar
                    foreach (Command c in GetCommands().Where(c => args.Contains(c.name)))
                    {
                        string arguments = "";
                        foreach (string s in c.args)
                            arguments += "[" + s + "] ";
                        Logging.Log(c.name + ": " + c.description + (arguments != "" ? " | " + arguments : ""));
                    }
                }
                else
                {
                    // Stelle alle Command-Attribute dar
                    foreach (Command c in GetCommands())
                    {
                        string arguments = "";
                        foreach (string s in c.args)
                            arguments += "[" + s + "] ";
                        Logging.Log(c.name + ": " + c.description + (arguments != "" ? " | " + arguments : ""));
                    }
                }
            }

            // Findet alle Command-Attribute
            public static Command[] GetCommands()
            {
                // Finde alle Methoden
                MethodInfo[] infos = Assembly.GetAssembly(typeof(Command)).GetTypes().SelectMany(t => t.GetMethods()).ToArray();
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
                while (TeamSpeakBot.isRunning)
                {
                    // Lese den aktuellen Befehl aus der Kommandozeile
                    string command = Console.ReadLine();

                    // Logging
                    Logging.Log("Kommando erkannt: " + command);

                    // Prüfe ob es ein Kommando ist
                    if (command.StartsWith("/"))
                    {
                        // Formatierung
                        List<string> commands = command.Remove(0, 1).Split(' ').ToList();

                        // Versuche das Kommando zu finden
                        Command cmd = GetCommands().FirstOrDefault(c => c.name == commands.First());

                        // Wenn der Befehl falsch ist, abbrechen
                        if (cmd == null)
                        {
                            Logging.Log("Kommando nicht gefunden: " + commands.First());
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
                        Logging.Log("Kommando ungültig: " + command);
                        return;
                    }
                }
            }
        }   

        // Attribut um ein Kommando zu definieren
        public class Command : Attribute
        {
            public string name;
            public string description;
            public string[] args = new string[0];
            public MethodInfo method;

            public Command(string name, string description)
            {
                this.name = name;
                this.description = description;
            }
        }
    }
}

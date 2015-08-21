/*==============================*\
 *  TeamSpeak³ Management Bot   *
 *   KCST - Thomas P. Kerman    *
 * http://kerbalspaceprogram.de *
\*==============================*/

using System;
using TeamSpeakBot.Utility;
using TeamSpeakBot.Commands;
using TeamSpeakBot.ConnectionWorker;
using System.Threading;

namespace TeamSpeakBot
{
    // Einstiegspunkt für den TeamSpeak³ Bot
    public class TeamSpeakBot
    {
        // Die Konfiguration des Bots
        public static BotSettings settings;
        public static ConnectionSettings connection;
        public static TS3Worker worker;

        // Threads
        public static Thread cmdThread;
        public static Thread botThread;

        // Status
        public static bool isRunning { get; protected set; }

        public static void Main(string[] args)
        {
            // Try - Catch erlaubt uns ein besseres Fehlermanagement, also nutzen wir das.
            try
            {
                // Lade die Konfiguration
                settings = SettingsParser<BotSettings>.Load();
                connection = SettingsParser<ConnectionSettings>.Load();

                // Logging starten
                Logging.InitLogging();

                // Exit-Methode registrieren
                AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnConsoleExit);

                // Log
                Console.Title = "TS³ Management Bot - " + Utility.Version.GetVersion();
                Logging.LogSpecial("Hallo, ich bin ein TeamSpeak³-Bot.");

                // Verbindung zum Server herstellen
                worker = TS3Worker.CreateWorker(connection);
                worker.OpenConnection();

                // Status
                isRunning = true;

                // Kommandos
                cmdThread = new Thread(CommandHandler.CheckCommands);
                cmdThread.Start();

                // BotLogic
                botThread = new Thread(TS3Worker.CheckLogic);
                botThread.Start();

                // Den Prozess nicht hier beenden
                while (true) { continue; }
            }
            catch (Exception e)
            {
                // Logge die Exception
                Logging.LogException(e);
            }
        }

        private static void OnConsoleExit(object sender, EventArgs e)
        {
            // Stelle Farbe wieder her
            Console.ForegroundColor = Logging.color;

            // Schreibe Nachricht
            Logging.LogSpecial("Bot schaltet sich ab!");
            Logging.Close();

            // Status
            isRunning = false;
        }
    }
}

/*==============================*\
 *  TeamSpeak³ Management Bot   *
 *   KCST - Thomas P. Kerman    *
 * http://kerbalspaceprogram.de *
\*==============================*/

using System;
using TeamSpeakBot.Utility;
using TeamSpeakBot.Commands;
using TS3QueryLib;

namespace TeamSpeakBot
{
    // Einstiegspunkt für den TeamSpeak³ Bot
    public class TeamSpeakBot
    {
        public static void Main(string[] args)
        {
            // Try - Catch erlaubt uns ein besseres Fehlermanagement, also nutzen wir das.
            try 
            {
                // Logging starten
                Logging.InitLogging();

                // Exit-Methode registrieren
                Console.CancelKeyPress += new ConsoleCancelEventHandler(OnConsoleExit);

                // Kein Nutzen, nur ein Test
                Console.Title = "TS³ Management Bot - " + Utility.Version.version;
                Logging.LogSpecial("Hallo, ich bin ein TeamSpeak³-Bot.");
                Logging.Log("Starte TS³-Verbindung...");
                Logging.LogWarning("Kann TS³-Server nicht finden!");
                Logging.LogError("Breche ab...");

                // Den Prozess nicht hier beenden
                while (true)
                {
                    CommandHandler.CheckCommands();
                }
            }
            catch (Exception e)
            {
                // Logge die Exception
                Logging.LogException(e);
            }
        }

        private static void OnConsoleExit(object sender, ConsoleCancelEventArgs e)
        {
            // Stelle Farbe wieder her
            Console.ForegroundColor = Logging.color;

            // Schreibe Nachricht
            Logging.LogSpecial("Bot schaltet sich ab!");
        }
    }
}

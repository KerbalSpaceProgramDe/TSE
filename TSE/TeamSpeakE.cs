/*==============================*\
 *  TeamSpeak³ Management Bot   *
 *   KCST - Thomas P. Kerman    *
 * http://kerbalspaceprogram.de *
\*==============================*/

using System;
using TeamSpeakE.Utility;
using TS3QueryLib;

namespace TeamSpeakE
{
    // Einstiegspunkt für den TeamSpeak³ Bot
    public class TeamSpeakE
    {
        public static void Main(string[] args)
        {
            // Logging starten
            Logging.InitLogging();

            // Kein Nutzen, nur ein Test
            Console.Title = "TeamSpeak³-Bot | kerbal.de";
            Logging.LogSpecial("Hallo, ich bin ein TeamSpeak³-Bot.");
            Logging.Log("Starte TS³-Verbindung...");
            Logging.LogWarning("Kann TS³-Server nicht finden!");
            Logging.LogError("Breche ab...");

            // Solange kein exit Kommando erkannt wird, warten
            while (Console.ReadLine() != "/exit") // <== Durch ein besseres System ersetzen!
            {
                continue;
            }

            // Exit Kommando erkannt
            Logging.LogSpecial("Bot schaltet sich ab!");
        }
    }
}

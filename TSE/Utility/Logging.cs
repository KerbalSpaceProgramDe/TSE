/*==============================*\
 *  TeamSpeak³ Management Bot   *
 *   KCST - Thomas P. Kerman    *
 * http://kerbalspaceprogram.de *
\*==============================*/

using System;
using System.IO;

namespace TeamSpeakBot
{
    namespace Utility
    {
        // Logging Klasse, für einfache Statusmeldungen
        public class Logging
        {
            // Schriftfarbe in der Konsole
            public static ConsoleColor color;

            // Loggen der Uhrzeit
            protected static string now => "[LOG " + DateTime.Now.ToLongTimeString() + "]: ";

            // Logging-Stream
            protected static StreamWriter logger;

            // Initializiere Logging
            public static void InitLogging()
            {
                // Erstelle den Ordner
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Logs/");

                // Starte den Logger
                logger = new StreamWriter(Directory.GetCurrentDirectory() + "/bot_log.txt", false);

                // Speichere Schriftfarbe
                color = Console.ForegroundColor;

                // Setze Schriftfarbe
                Console.ForegroundColor = TeamSpeakBot.settings.colorNormal;

                // Logge Header
                Log("TS³ Management Bot - " + Version.version);
            }

            // Wenn der Bot beendet wird, Logger schließen
            public static void Close()
            {
                logger.Flush();
                logger.Close();
            }

            // Einfaches Logging-Event
            public static void Log(object o)
            {
                // Schreibe das Object in Kommandozeile und Logger-Datei
                Console.WriteLine(now + o);
                logger.WriteLine(now + o);
                logger.Flush();
            }

            // Logging-Warnung
            public static void LogWarning(object o)
            {
                // Schreibe das Object in Gelb in die Kommandozeile
                Console.ForegroundColor = TeamSpeakBot.settings.colorWarning;
                Log(o);
                Console.ForegroundColor = TeamSpeakBot.settings.colorNormal;
            }

            // Logging-Error
            public static void LogError(object o)
            {
                // Schreibe das Object in Rot in die Kommandozeile
                Console.ForegroundColor = TeamSpeakBot.settings.colorError;
                Log(o);
                Console.ForegroundColor = TeamSpeakBot.settings.colorNormal;
            }

            // Spezielle Events
            public static void LogSpecial(object o)
            {
                // Schreibe das Object in Grün in die Kommandozeile
                Console.ForegroundColor = TeamSpeakBot.settings.colorSpecial;
                Log(o);
                Console.ForegroundColor = TeamSpeakBot.settings.colorNormal;
            }

            // Logging für C#-Exceptions
            public static void LogException(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Log(e.Message);
                Log(e.StackTrace);
                Console.ForegroundColor = TeamSpeakBot.settings.colorNormal;
            }
        }
    }
}

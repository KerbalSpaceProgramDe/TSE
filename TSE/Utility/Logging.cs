/*==============================*\
 *  TeamSpeak³ Management Bot   *
 *   KCST - Thomas P. Kerman    *
 * http://kerbalspaceprogram.de *
\*==============================*/

using System;

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

            // Gestriges Datum
            protected static string yesterday
            {
                get
                {
                    DateTime y = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                    return y.Year + "-" + y.Month + "-" + y.Day;
                }
            }

            // Initializiere Logging
            public static void InitLogging()
            {
                // Speichere Schriftfarbe
                color = Console.ForegroundColor;

                // Setze Schriftfarbe
                Console.ForegroundColor = ConsoleColor.Gray;

                // Logge Header
                Log("TS³ Management Bot - " + Version.version);
            }

            // Einfaches Logging-Event
            public static void Log(object o)
            {
                // Schreibe das Object in Weiß in die Kommandozeile
                Console.WriteLine(now + o);
            }

            // Logging-Warnung
            public static void LogWarning(object o)
            {
                // Schreibe das Object in Gelb in die Kommandozeile
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(now + o);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            // Logging-Error
            public static void LogError(object o)
            {
                // Schreibe das Object in Rot in die Kommandozeile
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(now + o);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            // Spezielle Events
            public static void LogSpecial(object o)
            {
                // Schreibe das Object in Grün in die Kommandozeile
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(now + o);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            // Logging für C#-Exceptions
            public static void LogException(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}

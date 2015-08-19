/*==============================*\
 *  TeamSpeak³ Management Bot   *
 *   KCST - Thomas P. Kerman    *
 * http://kerbalspaceprogram.de *
\*==============================*/

using System;

namespace TeamSpeakE
{
    namespace Utility
    {
        // Logging Klasse, für einfache Statusmeldungen
        public class Logging
        {
            // Schriftfarbe in der Konsole
            protected static ConsoleColor color;

            // Loggen der Uhrzeit
            protected static string now => "[LOG " + DateTime.Now.ToLongTimeString() + "] ";

            // Initializiere Logging
            public static void InitLogging()
            {
                // Speichere Schriftfarbe
                color = Console.ForegroundColor;
            }

            // Einfaches Logging-Event
            public static void Log(object o)
            {
                // Schreibe das Object in Weiß in die Kommandozeile
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(now + o);
                Console.ForegroundColor = color;
            }

            // Logging-Warnung
            public static void LogWarning(object o)
            {
                // Schreibe das Object in Gelb in die Kommandozeile
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(now + o);
                Console.ForegroundColor = color;
            }

            // Logging-Error
            public static void LogError(object o)
            {
                // Schreibe das Object in Rot in die Kommandozeile
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(now + o);
                Console.ForegroundColor = color;
            }

            // Spezielle Events
            public static void LogSpecial(object o)
            {
                // Schreibe das Object in Grün in die Kommandozeile
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(now + o);
                Console.ForegroundColor = color;
            }
        }
    }
}

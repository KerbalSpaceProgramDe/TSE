/*==============================*\
 *  TeamSpeak³ Management Bot   *
 *   KCST - Thomas P. Kerman    *
 * http://kerbalspaceprogram.de *
\*==============================*/

using Newtonsoft.Json;
using System;
using System.IO;

namespace TeamSpeakBot
{
    namespace Utility
    {
        // Settings-Parser
        public class SettingsParser<T> where T : new()
        {
            // Laden der Einstellungen
            public static T Load()
            {
                // Finde den Typ der Settings-Klasse
                Type type = typeof(T);

                // Erstelle Instanz der Settings Klasse
                T settings = new T();

                // Ermittle den Pfad der Settings-Datei
                string path = Directory.GetCurrentDirectory() + "/Configs/" + type.Name + ".json";

                // Prüfe, ob Settings-Datei existiert
                if (File.Exists(path))
                {
                    // Lade die Settings-Datei aus dem JSON-Dokument
                    settings = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));

                    // Gib die Settings-Datei zurück.
                    return settings;
                }
                else // Erstelle eine neue Datei
                {
                    // Speichere die Klasse
                    Save(settings);

                    // Gib die neue Klasse zurück
                    return settings;
                }
            }

            // Speichern der Einstellungen
            public static void Save(T settings)
            {
                // Finde den Typ der Settings-Klasse
                Type type = typeof(T);

                // Ermittle den Pfad der Settings-Datei
                string path = Directory.GetCurrentDirectory() + "/Configs/" + type.Name + ".json";

                // Erstelle den Ordner
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                // Serializiere das Object in ein JSON-Dokument und schreibe es in den Ordner
                File.WriteAllText(path, JsonConvert.SerializeObject(settings, Formatting.Indented));
            }
        }

        // Settings-Klasse
        [JsonObject(Description = "Die generellen Einstellungen des TeamSpeak³-Bots.")]
        public class BotSettings
        {
            [JsonProperty]
            public ConsoleColor colorNormal = ConsoleColor.Gray;
            [JsonProperty]
            public ConsoleColor colorSpecial = ConsoleColor.DarkGreen;
            [JsonProperty]
            public ConsoleColor colorWarning = ConsoleColor.DarkYellow;
            [JsonProperty]
            public ConsoleColor colorError = ConsoleColor.DarkRed;
            [JsonProperty]
            public string name = "TeamSpeak Bot";
        }

        // Verbindingskonfiguration
        [JsonObject(Description = "Die Verbindung des TS³-Bots zum Server.")]
        public class ConnectionSettings
        {
            [JsonProperty]
            public string host = "127.0.0.1";
            [JsonProperty]
            public ushort queryPort = 10011;
            [JsonProperty]
            public ushort serverPort = 9987;
            [JsonProperty]
            public string username = "serveradmin";
            [JsonProperty]
            public string password = "";
        }
    }
}

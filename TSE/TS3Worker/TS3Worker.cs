/*==============================*\
 *  TeamSpeak³ Management Bot   *
 *   KCST - Thomas P. Kerman    *
 * http://kerbalspaceprogram.de *
\*==============================*/

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Linq;
using TeamSpeakBot.Utility;
using TS3QueryLib.Core;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Common.Responses;
using TS3QueryLib.Core.Communication;
using TS3QueryLib.Core.Server;

namespace TeamSpeakBot
{
    namespace ConnectionWorker
    {
        // Klasse die die Verbindung zu einem TS³ Server managed
        public class TS3Worker
        {
            // Instanz
            public static TS3Worker fetch { get; protected set; }

            // Die Adresse des TeamSpeak Servers
            public string host { get; protected set; }
            public ushort queryPort { get; protected set; }
            public ushort serverPort { get; protected set; }
            public string username { get; protected set; }
            public string password { get; protected set; }

            // Die Verbindung zum Server
            public AsyncTcpDispatcher tcpSocket { get; protected set; }
            public QueryRunner query { get; protected set; }

            // Status
            public bool connected { get; protected set; }

            // Bot Logic
            public List<IBotLogic> botLogic { get; protected set; }

            // Erstelle eine neue Instanz den Workers
            public static TS3Worker CreateWorker(ConnectionSettings connection)
            {
                fetch = new TS3Worker()
                {
                    host = connection.host,
                    queryPort = connection.queryPort,
                    serverPort = connection.serverPort,
                    password = connection.password,
                    username = connection.username,
                    botLogic = new List<IBotLogic>()
                    
                };
                return fetch;
            }

            // Öffne die Verbindung zum TeamSpeak Server
            public void OpenConnection()
            {
                // Logging
                Logging.Log("Baue Verbindung zu " + host + ":" + queryPort + " auf...");

                // Verbindung
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                tcpSocket = new AsyncTcpDispatcher(host, queryPort);
                tcpSocket.ReadyForSendingCommands += OnReadyToSendCommands;
                tcpSocket.BanDetected += OnBanDetected;
                tcpSocket.NotificationReceived += OnNotificationReceived;
                tcpSocket.ServerClosedConnection += OnServerClosedConnection;
                tcpSocket.Connect();
            }

            // Schließt die Verbindung zum Server
            public void CloseConnection()
            {
                // Verbindung
                query.Logout();
                tcpSocket.Dispose();
                tcpSocket.Disconnect();

                // Logging
                Logging.Log("Serververbindung getrennt.");

                // Status
                connected = false;
            }

            // Bereit zum senden von Kommandos
            private void OnReadyToSendCommands(object sender, EventArgs e)
            {
                // Logging
                Logging.Log("Verbindung erfolgreich. Starte Authentifizierung...");

                // Login
                query = new QueryRunner(tcpSocket);
                query.Login(username, password);
                query.SelectVirtualServerByPort(serverPort);

                // Mehr Logging
                Logging.Log("Authentifizierung erfolgreich. Starte Bot-Logik...");

                // Bot Logik
                Type[] types = Assembly.GetAssembly(GetType()).GetTypes().Where(t => t.GetInterface("IBotLogic") != null).ToArray();
                foreach (Type bot in types)
                {
                    Logging.Log("Starte " + bot.Name + "...");
                    try
                    {
                        IBotLogic logic = Activator.CreateInstance(bot) as IBotLogic;
                        fetch.botLogic.Add(logic);
                        Logging.LogSpecial(bot.Name + " gestartet.");
                    }
                    catch (Exception e2)
                    {
                        Logging.LogWarning("Kann " + bot.Name + " nicht starten!");
                        Logging.LogException(e2);
                    }
                }

                // Status
                connected = true;
            }

            // Ban erkannt
            private void OnBanDetected(object sender, EventArgs<SimpleResponse> e)
            {
                // Logging
                Logging.LogWarning("Query-Account wurde gesperrt!");
                Logging.LogWarning("Grund: " + e.Value.StatusText);

                // Verbindung
                CloseConnection();
            }

            // Nachricht erhalten
            private void OnNotificationReceived(object sender, EventArgs<string> e)
            {
                // Logging
                Logging.LogSpecial("Nachricht empfangen: " + e.Value);
            }

            // Server schließt Verbindung
            private void OnServerClosedConnection(object sender, EventArgs e)
            {
                // Status
                connected = false;
            }

            // Bot-Logic Check
            public static void CheckLogic()
            {
                while (TeamSpeakBot.isRunning)
                {
                    // Das zickt rum, also betreiben wir die "Thomas Feuerwehr" Methode und unterdrücken den Fehler :P
                    // Ehrlich: Der Fehler ist nicht schlimm, man kann aber auch nichts gegen machen ^^
                    try
                    {
                        foreach (IBotLogic bot in fetch.botLogic)
                            bot.CheckLogic();
                    }
                    catch { }
                }
            }
        }
    }
}

﻿/*==============================*\
 *  TeamSpeak³ Management Bot   *
 *   KCST - Thomas P. Kerman    *
 * http://kerbalspaceprogram.de *
\*==============================*/

using System;
using System.Threading;
using TeamSpeakBot.Utility;
using TS3QueryLib.Core;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Common.Responses;
using TS3QueryLib.Core.Communication;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Responses;

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

            // Erstelle eine neue Instanz den Workers
            public static TS3Worker CreateWorker(ConnectionSettings connection)
            {
                fetch = new TS3Worker()
                {
                    host = connection.host,
                    queryPort = connection.queryPort,
                    serverPort = connection.serverPort,
                    password = connection.password,
                    username = connection.username
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
                Logging.Log("Authentifizierung erfolgreich. Starte Bot-Logik.");

                // Status
                connected = true;
            }
        }
    }
}

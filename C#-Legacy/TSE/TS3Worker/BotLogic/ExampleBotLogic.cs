/*==============================*\
 *  TeamSpeak³ Management Bot   *
 *   KCST - Thomas P. Kerman    *
 * http://kerbalspaceprogram.de *
\*==============================*/

using System;
using System.Linq;
using TS3QueryLib.Core.Common.Responses;
using TS3QueryLib.Core.Server.Entities;

namespace TeamSpeakBot
{
    namespace ConnectionWorker
    {
        // BotLogic Beispiel
        public class ExampleBotLogic : IBotLogic
        {
            // Anzahl von Usern auf dem TS
            private uint[] users = new uint[0];

            // Logic-Check
            void IBotLogic.CheckLogic()
            {
                // Finde alle Clienten
                ListResponse<ClientListEntry> clients = TS3Worker.fetch.query.GetClientList();

                // Prüfe ob sie beim letzten Check schon auf dem Server waren
                foreach (ClientListEntry client in clients)
                {
                    // Wenn nein, begrüße den User
                    if (!users.Contains(client.ClientId))
                    {
                        TS3Worker.fetch.query.PokeClient(client.ClientId, "Wilkommen auf dem TeamSpeak Server " + client.Nickname + "!");
                    }
                }

                // Generiere Array neu
                users = clients.Select(u => u.ClientId).ToArray();
            }

            // Konstuktor
            public ExampleBotLogic() { }
        }
    }
}

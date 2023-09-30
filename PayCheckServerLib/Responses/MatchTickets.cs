using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.WSController;

namespace PayCheckServerLib.Responses
{
    public class MatchTickets
    {
        public class TicketReqJson
        {
            public Dictionary<string, object> attributes { get; set; }
            public Dictionary<string, int> latencies { get; set; }
            public string matchPool { get; set; }
            public string sessionId { get; set; }
        }

        public class TicketRspJson
        {
            public string matchTicketID { get; set; }
            public int queueTime { get; set; }
        }
        public class WSS_OnMatchmakingStarted
        {
            public string TicketID { get; set; }
            public string PartyID { get; set; }
            public string Namespace { get; set; }
            public string CreatedAt { get; set; }
            public string MatchPool { get; set; }
        }

        [HTTP("POST", "/match2/v1/namespaces/{namespace}/match-tickets")]
        public static bool Tickets(HttpRequest request, PC3Server.PC3Session session)
        {
            var auth = session.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            var ticket = JsonConvert.DeserializeObject<TicketReqJson>(request.Body);
            var ticketId = UserIdHelper.CreateNewID();
            TicketRspJson ticketRsp = new()
            {
                matchTicketID = ticketId,
                queueTime = 2
            }; 
            ResponseCreator response = new();
            response.SetBody(JsonConvert.SerializeObject(ticketRsp));
            session.SendResponse(response.GetResponse());
            Debugger.PrintDebug("OnMatchmakingStarted YEET");
            //Store Both and send into Lobby WSS

            WSS_OnMatchmakingStarted onMatchmakingStarted = new()
            { 
                TicketID = ticketId,
                PartyID = ticket.sessionId,
                CreatedAt = DateTime.UtcNow.ToString("o"),
                MatchPool = ticket.matchPool,
                Namespace = session.Headers["namespace"]
            };
            Dictionary<string, string> resp = new()
            {
                { "type", "messageSessionNotif" },
                { "id", UserIdHelper.CreateNewID() },
                { "from", "system" },
                { "to", token.UserId },
                { "topic", "OnMatchmakingStarted" },
                { "payload", LobbyControl.Base64Encode(JsonConvert.SerializeObject(onMatchmakingStarted)) },
                { "sentAt", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") }
            };
            LobbyControl.SendToLobby(resp, session.GetWSLobby(token.UserId));
            GSController.Make(ticket, session, session.Headers["namespace"]);
            GSController.Tickets.Add(token.UserId, ticketId);
            /*
             * Need to check what happens at this stage.
             * Do we need to send everyone its token or something?
            List<string> ids = new();
            rsp.Members.ForEach(m => ids.Add(m.Id));

            foreach (var id in session.WSSServer().WSUserIds)
            {
                if (ids.Contains(id))
                {
                    LobbyControl.SendToLobby(resp, session.GetWSLobby(id));
                }
            }*/


            return true;
        }
    }
}

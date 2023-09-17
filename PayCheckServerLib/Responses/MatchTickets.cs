using NetCoreServer;
using Newtonsoft.Json;

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

        [HTTP("POST", "/match2/v1/namespaces/pd3beta/match-tickets")]
        public static bool Tickets(HttpRequest request, PC3Server.PC3Session session)
        {
            var ticket = JsonConvert.DeserializeObject<TicketReqJson>(request.Body);
            var ticketId = UserIdHelper.CreateNewID();
            TicketRspJson ticketRsp = new()
            {
                matchTicketID = ticketId,
                queueTime = 2
            };
            //Store Both and send into Lobby WSS
            ResponseCreator response = new();
            response.SetBody(JsonConvert.SerializeObject(ticketRsp));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}

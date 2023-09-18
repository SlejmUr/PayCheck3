using NetCoreServer;
using Newtonsoft.Json;
using static PayCheckServerLib.Responses.MatchTickets;

namespace PayCheckServerLib.Responses
{
    public class Parties
    {
        [HTTP("PATCH", "/session/v1/public/namespaces/pd3/parties/{partyid}")]
        public static bool PATCH_Parties(HttpRequest request, PC3Server.PC3Session session)
        {
            /*
            var body = JsonConvert.DeserializeObject<object>(request.Body);
            ResponseCreator response = new();
            response.SetBody(JsonConvert.SerializeObject(""));
            session.SendResponse(response.GetResponse());
            */
            return false;
        }


        [HTTP("DELETE", "/session/v1/public/namespaces/pd3/parties/{partyid}/users/me/leave")]
        public static bool LeaveParties(HttpRequest request, PC3Server.PC3Session session)
        {
            /*
            var body = JsonConvert.DeserializeObject<object>(request.Body);
            ResponseCreator response = new();
            response.SetBody(JsonConvert.SerializeObject(""));
            session.SendResponse(response.GetResponse());
            */
            return false;
        }


        [HTTP("POST", "/session/v1/public/namespaces/pd3/parties/{partyid}/users/me/join")]
        public static bool JoinParties(HttpRequest request, PC3Server.PC3Session session)
        {
            /*
            var body = JsonConvert.DeserializeObject<object>(request.Body);
            ResponseCreator response = new();
            response.SetBody(JsonConvert.SerializeObject(""));
            session.SendResponse(response.GetResponse());
            */
            return false;
        }

        [HTTP("POST", "/session/v1/public/namespaces/pd3/parties/{partyid}/users/me/reject")]
        public static bool RejectParties(HttpRequest request, PC3Server.PC3Session session)
        {
            /*
            var body = JsonConvert.DeserializeObject<object>(request.Body);
            ResponseCreator response = new();
            response.SetBody(JsonConvert.SerializeObject(""));
            session.SendResponse(response.GetResponse());
            */
            return false;
        }

        [HTTP("POST", "/session/v1/public/namespaces/pd3/parties/users/me/join/code")]
        public static bool JoinPartyByCode(HttpRequest request, PC3Server.PC3Session session)
        {
            /*
            var body = JsonConvert.DeserializeObject<object>(request.Body);
            ResponseCreator response = new();
            response.SetBody(JsonConvert.SerializeObject(""));
            session.SendResponse(response.GetResponse());
            */
            return false;
        }

        [HTTP("POST", "/session/v1/public/namespaces/pd3/parties/{partyid}/invite")]
        public static bool InviteOtherPlayer(HttpRequest request, PC3Server.PC3Session session)
        {
            /*
            var body = JsonConvert.DeserializeObject<object>(request.Body);
            ResponseCreator response = new();
            response.SetBody(JsonConvert.SerializeObject(""));
            session.SendResponse(response.GetResponse());
            */
            return false;
        }
    }
}

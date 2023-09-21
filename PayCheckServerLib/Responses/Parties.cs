using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons.PartyStuff;
using PayCheckServerLib.WSController;

namespace PayCheckServerLib.Responses
{
    public class Parties
    {
        [HTTP("POST", "/session/v1/public/namespaces/pd3/party")]
        public static bool Party(HttpRequest request, PC3Server.PC3Session session)
        {
            var body = JsonConvert.DeserializeObject<PartyPostReq>(request.Body);
            var rsp = PartyController.CreateParty(body);
            ResponseCreator response = new();
            response.SetBody(JsonConvert.SerializeObject(rsp));
            session.SendResponse(response.GetResponse());

            //send notif to user to party created
            var wss_sess = session.GetWSLobby(session.WSUserId);
            Dictionary<string, string> resp = new()
            {
                { "type", "messageSessionNotif" },
                { "topic", "OnPartyCreated" },
                { "sentAt", DateTime.UtcNow.ToString("o") },
            };
            OnPartyCreated pld = new()
            { 
                Code = rsp.Code,
                CreatedBy = rsp.CreatedBy,
                PartyId = rsp.Id,
                TextChat = body.TextChat
            };
            resp.Add("payload", LobbyControl.Base64Encode(JsonConvert.SerializeObject(pld)));
            LobbyControl.SendToLobby(resp,wss_sess);
            return true;
        }
        [HTTP("PATCH", "/session/v1/public/namespaces/pd3/parties/{partyid}")]
        public static bool PATCH_Parties(HttpRequest request, PC3Server.PC3Session session)
        {
            var body = JsonConvert.DeserializeObject<PartyPatch>(request.Body);
            PartyPost.Response rsp = PartyController.UpdateParty(session.HttpParam["partyid"],body);
            ResponseCreator response = new();
            response.SetBody(JsonConvert.SerializeObject(rsp));
            session.SendResponse(response.GetResponse());

            Dictionary<string, string> resp = new()
            {
                { "type", "messageSessionNotif" },
                { "topic", "OnPartyUpdated" },
                { "sentAt", DateTime.UtcNow.ToString("o") },
            };
            OnPartyUpdated pld = new()
            {
                Code = rsp.Code,
                CreatedBy = rsp.CreatedBy,
                Attributes = rsp.Attributes,
                Configuration = new()
                { 
                    ClientVersion = rsp.Configuration.ClientVersion,
                    Joinability = rsp.Configuration.Joinability,
                    InactiveTimeout = rsp.Configuration.InactiveTimeout,
                    InviteTimeout = rsp.Configuration.InviteTimeout,
                    MaxPlayers = rsp.Configuration.MaxPlayers,
                    MinPlayers = rsp.Configuration.MinPlayers,
                    Name = rsp.Configuration.Name,
                    Type = rsp.Configuration.Type,
                    RequestedRegions = rsp.Configuration.RequestedRegions
                },
                ConfigurationName = rsp.Configuration.Name,
                CreatedAt = rsp.CreatedAt,
                Id = rsp.Id,
                IsFull = rsp.IsFull,
                LeaderId = rsp.LeaderId,
                Members = rsp.Members,
                Namespace = "pd3",
                UpdatedAt = rsp.UpdatedAt,
                Version = rsp.Version
            };
            resp.Add("payload", LobbyControl.Base64Encode(JsonConvert.SerializeObject(pld)));

            List<string> ids = new();
            rsp.Members.ForEach(m => ids.Add(m.Id));

            foreach (var id in session.WSSServer().WSUserIds)
            { 
                if (ids.Contains(id))
                {
                    LobbyControl.SendToLobby(resp, session.GetWSLobby(id));
                }
            }

            return true;
        }


        [HTTP("DELETE", "/session/v1/public/namespaces/pd3/parties/{partyid}/users/me/leave")]
        public static bool LeaveParties(HttpRequest request, PC3Server.PC3Session session)
        {
            //This response sadly KILLING THE GAME (Even without emu)
            //  SBZ PLEASE FIX!
            ResponseCreator response = new(204);
            session.SendResponse(response.GetResponse());
            return true;
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

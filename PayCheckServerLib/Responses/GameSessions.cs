using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.GS;
using PayCheckServerLib.WSController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib.Responses
{
    public class GameSessions
    {
        [HTTP("GET", "/session/v1/public/namespaces/pd3/gamesessions/{sessionid}")]
        public static bool GETGameSessions(HttpRequest _, PC3Server.PC3Session session)
        {
            var auth = session.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            var gs = GSController.GetGameSession(session.HttpParam["sessionid"]);
            response.SetBody(JsonConvert.SerializeObject(gs));
            session.SendResponse(response.GetResponse());

            if (GSController.MatchFoundSent.Contains(token.UserId))
                return true;

            //SEND OnMatchFound on WSS
            OnMatchFound onMatchFound = new()
            { 
                CreatedAt = gs.CreatedAt,
                ID = gs.Id,
                MatchPool = gs.MatchPool,
                Namespace = gs.Namespace,
                Teams = new(),
                Tickets = new()
            };
            foreach (var team in gs.Teams)
            {
                foreach (var uid in team.UserIDs)
                {
                    onMatchFound.Tickets.Add(new()
                    { 
                        TicketID = GSController.Tickets[uid]
                    }); 
                }
                onMatchFound.Teams.Add(new()
                {
                    UserIDs = team.UserIDs,
                });
            }

            Dictionary<string, string> kv = new()
            {
                { "type", "messageSessionNotif" },
                { "id", UserIdHelper.CreateNewID() },
                { "from", "system" },
                { "to", token.UserId },
                { "topic", "OnMatchFound" },
                { "payload", LobbyControl.Base64Encode(JsonConvert.SerializeObject(onMatchFound)) },
                { "sentAt", DateTime.UtcNow.ToString("o") },
            };
            LobbyControl.SendToLobby(kv, session.GetWSLobby(token.UserId));
            GSController.MatchFoundSent.Add(token.UserId);
            return true;
        }

        [HTTP("PATCH", "/session/v1/public/namespaces/pd3/gamesessions/{sessionid}")]
        public static bool PATCHGameSessions(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            //response.SetBody(JsonConvert.SerializeObject(gamesessions));
            session.SendResponse(response.GetResponse());

            //OnDSStatusChanged

            return true;
        }

        [HTTP("POST", "/session/v1/public/namespaces/pd3/gamesessions/{sessionid}/join")]
        public static bool JoinToGameSessions(HttpRequest _, PC3Server.PC3Session session)
        {
            var auth = session.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            var gs = GSController.JoinSession(session.HttpParam["sessionid"], token.UserId);
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            response.SetBody(JsonConvert.SerializeObject(gs));
            session.SendResponse(response.GetResponse());
            //Send OnSessionMembersChanged, OnSessionJoined

            OnSessionJoined onSessionJoined = new()
            { 
                SessionID = gs.Id,
                TextChat = false,
                Members = gs.Members            
            }; 
            Dictionary<string, string> kv = new()
            {
                { "type", "messageSessionNotif" },
                { "topic", "OnSessionJoined" },
                { "payload", LobbyControl.Base64Encode(JsonConvert.SerializeObject(onSessionJoined)) },
                { "sentAt", DateTime.UtcNow.ToString("o") },
            };
            LobbyControl.SendToLobby(kv, session.GetWSLobby(token.UserId));

            
            OnSessionMembersChanged onSessionMembersChanged = new()
            {
                JoinerID = token.UserId,
                SessionID = gs.Id,
                LeaderID = token.UserId,
                TextChat = false,
                Members = new(),
                Teams = gs.Teams,
                Session = new()
                {
                    DSInformation = gs.DSInformation,
                    Attributes = gs.Attributes,
                    BackfillTicketID = gs.BackfillTicketID,
                    Code = gs.Code,
                    Configuration = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText("Files/Lobby_pveheist_DS.json")),
                    ConfigurationName = gs.MatchPool,
                    CreatedAt = gs.CreatedAt,
                    CreatedBy = gs.CreatedBy,
                    GameMode = gs.MatchPool,
                    ID = gs.Id,
                    IsFull = gs.IsFull,
                    LeaderID = gs.LeaderID,
                    MatchPool = gs.MatchPool,
                    Members = new(),
                    Namespace = gs.Namespace,
                    Teams = gs.Teams,
                    UpdatedAt = gs.UpdatedAt,
                    Version = gs.Version
                }
            };
            foreach (var member in gs.Members)
            {
                onSessionMembersChanged.Members.Add(new()
                { 
                    ID = member.Id,
                    Status = member.Status,
                    StatusV2 = member.StatusV2,
                    PlatformID = member.PlatformId,
                    PlatformUserID = member.PlatformUserId,
                    UpdatedAt = member.UpdatedAt
                });
                onSessionMembersChanged.Session.Members.Add(new()
                {
                    ID = member.Id,
                    Status = member.Status,
                    StatusV2 = member.StatusV2,
                    PlatformID = member.PlatformId,
                    PlatformUserID = member.PlatformUserId,
                    UpdatedAt = member.UpdatedAt
                });
            }
            kv = new()
            {
                { "type", "messageSessionNotif" },
                { "topic", "OnSessionMembersChanged" },
                { "payload", LobbyControl.Base64Encode(JsonConvert.SerializeObject(onSessionMembersChanged)) },
                { "sentAt", DateTime.UtcNow.ToString("o") },
            };

            //OnMemeberChanged to full team?
            LobbyControl.SendToLobby(kv, session.GetWSLobby(token.UserId));
            return true;
        }

        [HTTP("DELETE", "/session/v1/public/namespaces/pd3/gamesessions/{sessionid}/leave")]
        public static bool LeaveGameSessions(HttpRequest _, PC3Server.PC3Session session)
        {
            return false;
        }
    }
}

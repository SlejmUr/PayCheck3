using ModdableWebServer;
using ModdableWebServer.Helper;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons.GS;
using PayCheckServerLib.Jsons.PartyStuff;
using PayCheckServerLib.Responses;
using PayCheckServerLib.WSController;
using static PayCheckServerLib.Jsons.GS.OnMatchFound;

namespace PayCheckServerLib.Helpers
{
    //AKA GameSessionController
    public class GSController
    {
        public static List<string> MatchFoundSent = new();
        public static List<string> DSInfoSentList = new();
        public static Dictionary<string, DSInformationServer> DSInfo = new();
        public static Dictionary<string, GameSession> Sessions = new();
        public static Dictionary<string, string> Tickets = new();
        //  Here we making a Full created Match 
        public static void Make(MatchTickets.TicketReqJson ticketReq, ServerStruct serverStruct, string NameSpace)
        {
            var party = PartyController.PartySaves.Where(x => x.Value.Id == ticketReq.sessionId).FirstOrDefault().Value;
            if (party == null)
            {
                Debugger.PrintError("NO Code???? WHAT THE FUCK");
            }

            var id = UserIdHelper.CreateNewID();
            var gs = new GameSession()
            {
                DSInformation = new()
                {
                    RequestedAt = "0001-01-01T01:01:01.001Z00:00",
                    Server = null,
                    Status = "NEED_TO_REQUEST",
                    StatusV2 = "NEED_TO_REQUEST"
                },
                Attributes = ticketReq.attributes,
                BackfillTicketID = UserIdHelper.CreateNewID(),
                Code = UserIdHelper.CreateCode(),
                Configuration = JsonConvert.DeserializeObject<PartyPost.Configuration>(File.ReadAllText($"Files/{ticketReq.matchPool}Configuration.json")),
                CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                CreatedBy = "client-db" + UserIdHelper.CreateNewID(),
                Id = id,
                IsActive = true,
                IsFull = party.Members.Count > 4,
                TicketIDs = null,
                LeaderID = "",
                MatchPool = ticketReq.matchPool,
                Members = new(),
                Namespace = NameSpace,
                Teams = new(),
                UpdatedAt = DateTime.UtcNow.ToString("o"),
                Version = 1
            };
            Debugger.PrintDebug("gs!!");
            var team = new Jsons.GS.Team()
            {
                Parties = new(),
                UserIDs = new()
            };
            var party_gs = new Jsons.GS.Party()
            {
                PartyID = party.Id,
                UserIDs = new()
            };
            Debugger.PrintDebug("gsParty");
            foreach (var member in party.Members)
            {
                Debugger.PrintDebug(member.ToString());
                gs.Members.Add(new()
                {
                    Id = member.Id,
                    Status = "INVITED",
                    StatusV2 = "INVITED",
                    PlatformId = member.PlatformId,
                    PlatformUserId = member.PlatformUserId,
                    UpdatedAt = DateTime.UtcNow.ToString("o")
                });
                team.UserIDs.Add(member.Id);
                party_gs.UserIDs.Add(member.Id);
            }
            Debugger.PrintDebug("foreach done");
            team.Parties.Add(party_gs);
            gs.Teams.Add(team);
            Sessions.Add(NameSpace + "_" + id, gs);
            Debugger.PrintDebug("GS_Session Made!");
            OnSessionInvited onSessionInvited = new()
            {
                SenderID = gs.CreatedBy,
                SessionID = id
            };
            Dictionary<string, string> kv = new()
            {
                { "type", "messageSessionNotif" },
                { "topic", "OnSessionInvited" },
                { "payload", LobbyControl.Base64Encode(JsonConvert.SerializeObject(onSessionInvited)) },
                { "sentAt", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") },
            };
            Debugger.PrintDebug("OnSessionInvited");
            //  Maybe can be much better this but atleast works
            foreach (var key in LobbyControl.LobbyUsers.Keys)
            {
                var splitted = key.Split("_");
                string vs_ui = splitted[1];
                foreach (var uid in team.UserIDs)
                {
                    if (uid == vs_ui)
                    {
                        Debugger.PrintDebug("GS_Session OnSessionInvited sent to " + vs_ui);
                        LobbyControl.SendToLobby(kv, LobbyControl.GetLobbyUser(vs_ui, NameSpace));
                    }
                }
            }
        }

        public static GameSession GetGameSession(string id, string NameSpace)
        {
            if (Sessions.TryGetValue(NameSpace + "_" + id, out var session))
            {
                return session;
            }
            Debugger.PrintWarn("Session NOT FOUND");
            throw new Exception("Session NOT FOUND");
        }

        public static GameSession JoinSession(string id, string UserId, string NameSpace)
        {
            if (!Sessions.TryGetValue(NameSpace + "_" + id, out var session))
            {
                Debugger.PrintWarn("Session NOT FOUND");
                throw new Exception("Session NOT FOUND");
            }

            session.DSInformation.Status = "REQUESTED";
            session.DSInformation.StatusV2 = "REQUESTED";
            foreach (var item in session.Members)
            {
                if (item.Id == UserId)
                {
                    item.Status = "JOINED";
                    item.StatusV2 = "JOINED";
                }
            }
            session.Version++;
            session.UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            if (string.IsNullOrEmpty(session.LeaderID))
            {
                session.LeaderID = UserId;
            }
            Sessions[NameSpace + "_" + id] = session;
            return session;
        }

        public static GameSession Patch(PatchGameSessions patchGameSessions, string id, ServerStruct serverStruct, string NameSpace)
        {
            if (!Sessions.TryGetValue(NameSpace + "_" + id, out var session))
            {
                Debugger.PrintWarn("Session NOT FOUND");
                throw new Exception("Session NOT FOUND");
            }
            //send stuff to middleman
            var random = new Random();
            int rand = random.Next(0, MiddleManControl.MiddleMans.Count);
            var MiddleMan = MiddleManControl.MiddleMans.ElementAt(rand);

            //todo get client info
            string Req = "DSInfoReq-END-" + "eu-central-1," + "624677," + session.Id + "," + NameSpace;
            Debugger.PrintDebug("Sending to middleman");
            var x = MiddleMan.Value;
            WebSocketSender.SendWebSocketText(x, Req);
            //recieve back here?
            //  PLEASE if you know how to wait better than this, make a PR!
            while (!DSInfoSentList.Contains(NameSpace + "_" + id))
            {
                Thread.Sleep(100);
            }
            Debugger.PrintDebug("Recieved from MM in Patch");
            session.DSInformation.Server = DSInfo[id];
            session.DSInformation.Status = "AVAILABLE";
            session.DSInformation.StatusV2 = "AVAILABLE";


            OnDSStatusChanged.Basic onDSStatusChanged = new()
            {
                SessionID = id,
                GameServer = DSInfo[id],
                Session = new()
                {
                    ID = id,
                    IsFull = session.IsFull,
                    DSInformation = new()
                    {
                        Server = session.DSInformation.Server,
                        Status = "AVAILABLE",
                        StatusV2 = "AVAILABLE"
                    },
                    BackfillTicketID = session.BackfillTicketID,
                    Attributes = session.Attributes,
                    Code = session.Code,
                    Configuration = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText("Files/Lobby_pveheist_DS.json")),
                    ConfigurationName = session.MatchPool,
                    CreatedAt = session.CreatedAt,
                    CreatedBy = session.CreatedBy,
                    GameMode = session.MatchPool,
                    LeaderID = session.LeaderID,
                    MatchPool = session.MatchPool,
                    Members = new(),
                    Namespace = session.Namespace,
                    Teams = session.Teams,
                    UpdatedAt = DateTime.UtcNow.ToString("o"),
                    Version = session.Version++
                }
            };
            foreach (var member in session.Members)
            {
                onDSStatusChanged.Session.Members.Add(new()
                {
                    ID = member.Id,
                    Status = member.Status,
                    StatusV2 = member.StatusV2,
                    PlatformID = member.PlatformId,
                    PlatformUserID = member.PlatformUserId,
                    UpdatedAt = member.UpdatedAt
                });
            }
            Dictionary<string, string> kv = new()
            {
                { "type","messageSessionNotif" },
                { "topic", "OnDSStatusChanged" },
                { "payload", LobbyControl.Base64Encode(JsonConvert.SerializeObject(onDSStatusChanged)) },
                { "sentAt", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") },
            };

            //  Maybe can be much better this but atleast works

            foreach (var key in LobbyControl.LobbyUsers.Keys)
            {
                var splitted = key.Split("_");
                string vs_ui = splitted[1];
                foreach (var uid in session.Members)
                {
                    if (uid.Id == vs_ui)
                    {
                        Debugger.PrintDebug("GS_Session OnSessionInvited sent to " + vs_ui);
                        LobbyControl.SendToLobby(kv, LobbyControl.GetLobbyUser(vs_ui, NameSpace));
                    }
                }
            }

            session.Attributes = patchGameSessions.Attributes;
            session.Version = patchGameSessions.version++;
            Sessions[NameSpace + "_" + id] = session;
            return session;
        }
    }
}

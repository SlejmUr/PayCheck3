using Newtonsoft.Json;
using PayCheckServerLib.Jsons.GS;
using PayCheckServerLib.Responses;
using PayCheckServerLib.Jsons.PartyStuff;
using PayCheckServerLib.WSController;


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
        public static void Make(MatchTickets.TicketReqJson ticketReq, PC3Server.PC3Session session)
        {
            var party = PartyController.PartySaves.Where(x => x.Value.Id == ticketReq.sessionId).FirstOrDefault().Value;
            if (party == null)
            {
                Debugger.PrintError("NO Code???? WHAT THE FUCK");
                throw new Exception("Code is not exist in saved parties????");
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
                Configuration = JsonConvert.DeserializeObject<PartyPost.Configuration>(File.ReadAllText($"{ticketReq.matchPool}Configuration.json")),
                CreatedAt = DateTime.UtcNow.ToString("o"),
                CreatedBy = "client-db" + UserIdHelper.CreateNewID(),
                Id = id,
                IsActive = true,
                IsFull = party.Members.Count > 4,
                TicketIDs = null,
                LeaderID = "",
                MatchPool = ticketReq.matchPool,
                Members = new(),
                Namespace = "pd3",
                Teams = new(),
                UpdatedAt = DateTime.UtcNow.ToString("o"),
                Version = 1
            };
            var team = new Team();
            var party_gs = new Jsons.GS.Party()
            {
                PartyID = party.Id
            };
            foreach (var member in party.Members)
            {
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
            team.Parties.Add(party_gs);
            gs.Teams.Add(team);
            Sessions.Add(id, gs);
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
                { "sentAt", DateTime.UtcNow.ToString("o") },
            };

            //  Maybe can be much better this but atleast works
            foreach (var vs_ui in session.WSSServer().WSUserIds)
            {
                foreach (var uid in team.UserIDs)
                {
                    if (uid == vs_ui)
                    {
                        LobbyControl.SendToLobby(kv, session.GetWSLobby(vs_ui));
                    }
                }
            }
        }

        public static GameSession GetGameSession(string id)
        {
            if (Sessions.TryGetValue(id, out var session))
            {
                return session;
            }
            Debugger.PrintWarn("Session NOT FOUND");
            throw new Exception("Session NOT FOUND");
        }

        public static GameSession JoinSession(string id, string UserId)
        {
            if (!Sessions.TryGetValue(id, out var session))
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
            session.UpdatedAt = DateTime.UtcNow.ToString("o");
            if (string.IsNullOrEmpty(session.LeaderID))
            {
                session.LeaderID = UserId;
            }
            Sessions[id] =  session;
            return session;
        }

        public static GameSession Patch(PatchGameSessions patchGameSessions, string id, PC3Server.PC3Session pcSession)
        {
            if (!Sessions.TryGetValue(id, out var session))
            {
                Debugger.PrintWarn("Session NOT FOUND");
                throw new Exception("Session NOT FOUND");
            }
            //send stuff to middleman
            var random = new Random();
            var mlist = pcSession.MiddleMans;
            int index = random.Next(mlist.Count);
            var middleMan = mlist[index];
            string Req = "DSInfoReq-END-" + "eu-central-1,"+ "624677," + session.Id;
            middleMan.Send(Req);
            //recieve back here?
            //  PLEASE if you know how to wait better than this, make a PR!
            while (!DSInfoSentList.Contains(id))
            {
                Thread.Sleep(100);
            }
            
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
                    Version = session.Version+1
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
                { "sentAt", DateTime.UtcNow.ToString("o") },
            };

            var ws = pcSession.GetWSLobby(pcSession.WSUserId);
            LobbyControl.SendToLobby(kv, ws);

            session.Attributes = patchGameSessions.Attributes;
            session.Version = patchGameSessions.version+1;
            Sessions[id] = session;
            return session;
        }
    }
}

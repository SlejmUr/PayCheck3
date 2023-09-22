using Newtonsoft.Json;
using PayCheckServerLib.Jsons.GS;
using PayCheckServerLib.Responses;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.PartyStuff;

namespace PayCheckServerLib.Helpers
{
    //AKA GameSessionController
    public class GSController
    {
        public static Dictionary<string, GameSession> Sessions = new();


        //  Please Kill me
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
                CreatedBy = "client-db"+ UserIdHelper.CreateNewID(),
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
        }
    }
}

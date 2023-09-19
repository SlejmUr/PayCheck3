using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;
using System.Linq;

namespace PayCheckServerLib.Responses
{
    internal class Challenge
    {
        [HTTP("GET", "/challenge/v1/public/namespaces/pd3/users/{userId}/eligibility")]
        public static bool ChallengeEligibility(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator creator = new();
            creator.SetBody("{\n    \"isComply\": true\n}");
            session.SendResponse(creator.GetResponse());
            return true;
        }

        [HTTP("GET", "/challenge/v1/public/namespaces/pd3/users/me/records?limit=2147483647&offset=0")]
        public static bool ChallengeRecordsAll(HttpRequest _, PC3Server.PC3Session session)
        {
            var auth = session.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            ResponseCreator creator = new();
            var challenges = JsonConvert.DeserializeObject<DataPaging<ChallengesData>>(File.ReadAllText("Files/ChallengeRecords.json")) ?? throw new Exception("ChallengeRecords is null!");
            foreach (var item in challenges.Data)
            {
                item.UserId = token.UserId;
            }

            creator.SetBody(JsonConvert.SerializeObject(challenges));
            session.SendResponse(creator.GetResponse());
            return true;
        }

        [HTTP("GET", "/challenge/v1/public/namespaces/pd3/users/me/records?limit={limit}&offset={offset}")]
        public static bool ChallengeRecordsSplit(HttpRequest _, PC3Server.PC3Session session)
        {
            var auth = session.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            ResponseCreator creator = new();
            var challenges = JsonConvert.DeserializeObject<DataPaging<ChallengesData>>(File.ReadAllText("Files/ChallengeRecords.json")) ?? throw new Exception("ChallengeRecords is null!");
            challenges.Data.Skip(int.Parse(session.HttpParam["offset"])).Take(int.Parse(session.HttpParam["limit"]));
            foreach (var item in challenges.Data)
            {
                item.UserId = token.UserId;
            }

            creator.SetBody(JsonConvert.SerializeObject(challenges));
            session.SendResponse(creator.GetResponse());
            return true;
        }
    }
}

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

        [HTTP("GET", "/challenge/v1/public/namespaces/pd3/users/me/records?limit={limit}&offset={offset}")]
        public static bool ChallengeRecordsSplit(HttpRequest _, PC3Server.PC3Session session)
        {
            var limit_offset = session.HttpParam["limit&offset=offset"].Replace("records?limit=","").Replace("offset=", ""); //records?limit=100&offset=0
            var offset = int.Parse(limit_offset.Split("&")[1]);
            var limit = int.Parse(limit_offset.Split("&")[0]);
            var auth = session.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            ResponseCreator creator = new();
            var challenges = JsonConvert.DeserializeObject<DataPaging<ChallengesData>>(File.ReadAllText("Files/ChallengeRecords.json")) ?? throw new Exception("ChallengeRecords is null!");
            challenges.Data.Skip(offset).Take(limit);
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

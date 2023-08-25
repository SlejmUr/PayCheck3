using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses
{
    internal class Challenge
    {
        [HTTP("GET", "/challenge/v1/public/namespaces/pd3beta/users/{userId}/eligibility")]
        public static bool ChallengeEligibility(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator creator = new ResponseCreator();
            creator.SetBody("{\n    \"isComply\": true\n}");
            session.SendResponse(creator.GetResponse());
            return true;
        }

        [HTTP("GET", "/challenge/v1/public/namespaces/pd3beta/users/me/records?limit=2147483647&offset=0")]
        public static bool ChallengeRecords(HttpRequest request, PC3Server.PC3Session session)
        {
            var auth = session.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            ResponseCreator creator = new ResponseCreator();
            var challanges = JsonConvert.DeserializeObject<Challenges>(File.ReadAllText("Files/ChallangeRecords.json"));

            foreach (var item in challanges.Data)
            {
                item.UserId = token.UserId;
            }

            creator.SetBody(JsonConvert.SerializeObject(challanges));
            session.SendResponse(creator.GetResponse());
            return true;
        }
    }
}

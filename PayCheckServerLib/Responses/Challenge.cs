using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [HTTP("GET", "/challenge/v1/public/namespaces/pd3beta/users/me/records")]
        public static bool ChallengeRecords(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator creator = new ResponseCreator();
            var challanges = JsonConvert.DeserializeObject<Challenges>(File.ReadAllText("Files/ChallangeRecords.json"));

            foreach (var item in challanges.Data)
            {
                item.UserId = "29475976933497845197035744456968";
            }

            creator.SetBody(JsonConvert.SerializeObject(challanges));
            session.SendResponse(creator.GetResponse());
            return true;
        }
    }
}

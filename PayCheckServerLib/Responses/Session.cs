using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses
{
    public class Session
    {
        [HTTP("GET", "/session/v1/public/namespaces/pd3beta/users/me/attributes")]
        public static bool GETSessionAttributes(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            AttribSuccess success = new()
            {
                CrossplayEnabled = true,
                CurrentPlatform = "STEAM",
                Namespace = "pd3beta",
                Platforms = new()
                {
                    new()
                    { 
                        Name = "STEAM",
                        UserId = "76561199227922074"
                    }
                },
                UserId = "29475976933497845197035744456968"
            };
            response.SetBody(JsonConvert.SerializeObject(success));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("POST", "/session/v1/public/namespaces/pd3beta/users/me/attributes")]
        public static bool POSTSessionAttributes(HttpRequest request, PC3Server.PC3Session session)
        {
            var req = JsonConvert.DeserializeObject<AttribRequest>(request.Body);
            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            AttribSuccess success = new()
            { 
                CrossplayEnabled = req.CrossplayEnabled,
                CurrentPlatform = req.CurrentPlatform,
                Namespace = "pd3beta",
                Platforms = req.Platforms,
                UserId = "29475976933497845197035744456968"
            };
            response.SetBody(JsonConvert.SerializeObject(success));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/session/v1/public/namespaces/pd3beta/users/me/parties")]
        public static bool SessionsParties(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            Challenges challenges = new()
            { 
                Paging = new()
                { 
                    First = "",
                    Last = "",
                    Previous = "",
                    Next = ""
                },
                Data = new()
            };
            response.SetBody(JsonConvert.SerializeObject(challenges));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/session/v1/public/namespaces/pd3beta/users/me/gamesessions")]
        public static bool Sessionsgamesessions(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            Challenges challenges = new()
            {
                Paging = new()
                {
                    First = "",
                    Last = "",
                    Previous = "",
                    Next = ""
                },
                Data = new()
            };
            response.SetBody(JsonConvert.SerializeObject(challenges));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}

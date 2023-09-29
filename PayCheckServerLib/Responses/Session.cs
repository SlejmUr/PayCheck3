using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons.Basic;

namespace PayCheckServerLib.Responses
{
    public class Session
    {
        [HTTP("GET", "/session/v1/public/namespaces/pd3/users/me/attributes")]
        public static bool GETSessionAttributes(HttpRequest _, PC3Server.PC3Session session)
        {
            var auth = session.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            ResponseCreator response = new();
            AttribSuccess success = new()
            {
                CrossplayEnabled = true,
                CurrentPlatform = token.PlatformType.ToString().ToUpper(),
                Namespace = "pd3",
                Platforms = new()
                {
                    new()
                    {
                        Name = token.PlatformType.ToString().ToUpper(),
                        UserId = token.PlatformId
                    }
                },
                UserId = token.UserId
            };
            response.SetBody(JsonConvert.SerializeObject(success));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("POST", "/session/v1/public/namespaces/pd3/users/me/attributes")]
        public static bool POSTSessionAttributes(HttpRequest request, PC3Server.PC3Session session)
        {
            var auth = session.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            var req = JsonConvert.DeserializeObject<AttribRequest>(request.Body) ?? throw new Exception("POSTSessionAttributes -> req is null!");
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            AttribSuccess success = new()
            {
                CrossplayEnabled = req.CrossplayEnabled,
                CurrentPlatform = req.CurrentPlatform,
                Namespace = "pd3",
                Platforms = req.Platforms,
                UserId = token.UserId
            };
            response.SetBody(JsonConvert.SerializeObject(success));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/session/v1/public/namespaces/pd3/users/me/parties")]
        public static bool SessionsParties(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            DataPaging<object> gamesessions = new()
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
            response.SetBody(JsonConvert.SerializeObject(gamesessions));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/session/v1/public/namespaces/pd3/users/me/gamesessions")]
        public static bool Sessionsgamesessions(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            DataPaging<object> gamesessions = new()
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
            response.SetBody(JsonConvert.SerializeObject(gamesessions));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}

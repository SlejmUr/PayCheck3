using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons.Basic;

namespace PayCheckServerLib.Responses
{
    public class Session
    {

        [HTTP("GET", "/session/v1/public/namespaces/pd3/recent-player?limit=200")]
        public static bool GetRecentPlayersLimit(HttpRequest _, ServerStruct serverStruct)
        {
          ResponseCreator response = new ResponseCreator();
          response.SetHeader("Content-Type", "application/json");
          response.SetBody("{\"data\":[]}");
          serverStruct.Response = response.GetResponse();
          serverStruct.SendResponse();
          return true;
        }

        [HTTP("GET", "/session/v1/public/namespaces/{namespace}/users/me/attributes")]
        public static bool GETSessionAttributes(HttpRequest _, ServerStruct serverStruct)
        {
            var auth = serverStruct.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            ResponseCreator response = new();
            AttribSuccess success = new()
            {
                CrossplayEnabled = true,
                CurrentPlatform = token.PlatformType.ToString().ToUpper(),
                Namespace = token.Namespace,
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
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("POST", "/session/v1/public/namespaces/{namespace}/users/me/attributes")]
        public static bool POSTSessionAttributes(HttpRequest request, ServerStruct serverStruct)
        {
            var auth = serverStruct.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            var req = JsonConvert.DeserializeObject<AttribRequest>(request.Body) ?? throw new Exception("POSTSessionAttributes -> req is null!");
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            AttribSuccess success = new()
            {
                CrossplayEnabled = req.CrossplayEnabled,
                CurrentPlatform = req.CurrentPlatform,
                Namespace = token.Namespace,
                Platforms = req.Platforms,
                UserId = token.UserId
            };
            response.SetBody(JsonConvert.SerializeObject(success));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("GET", "/session/v1/public/namespaces/{namespace}/users/me/parties")]
        public static bool SessionsParties(HttpRequest _, ServerStruct serverStruct)
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
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("GET", "/session/v1/public/namespaces/{namespace}/users/me/gamesessions")]
        public static bool SessionsUsersMeGamesessions(HttpRequest _, ServerStruct serverStruct)
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
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }


        [HTTP("GET", "/session/v1/public/namespaces/{namespace}/gamesessions")]
        public static bool Sessionsgamesessions(HttpRequest _, ServerStruct serverStruct)
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
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }
    }
}

using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses
{
    public class BasicResponse
    {
        [HTTP("GET", "/qosm/public/qos")]
        public static bool QOSM_Public_QOS(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            var rsp = new JsonServers()
            {
                Servers = new()
            };
            foreach (var server in ConfigHelper.ServerConfig.DS_Servers)
            {
                rsp.Servers.Add(new()
                { 
                    Alias = server.Alias,
                    Status = server.Status,
                    Ip = server.Ip,
                    LastUpdate = "2023-08-06T10:00:00.000000000Z",
                    Port = server.Port,
                    Region = server.Region
                });
            }

            response.SetBody(JsonConvert.SerializeObject(rsp, Formatting.Indented));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/basic/v1/public/misc/time")]
        public static bool Time(HttpRequest request, PC3Server.PC3Session session)
        {
            if (session.Headers.ContainsKey("cookie"))
            {
                var tokens = TokenHelper.ReadFromHeader(session.Headers);
                Debugger.PrintDebug($"{tokens.AccessToken.UserId}({tokens.AccessToken.Name}) Is still in the server!");
            }

            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            var rsp = new Time()
            {
                CurrentTime = DateTime.UtcNow.ToString("o")
            };
            response.SetBody(JsonConvert.SerializeObject(rsp));
            session.SendResponse(response.GetResponse());
            return true;
        }



        [HTTP("GET", "/lobby/v1/messages")]
        public static bool LobbyMessages(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            response.SetBody(File.ReadAllBytes("Files/messages.json"));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/iam/v3/location/country")]
        public static bool Country(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            response.SetBody(File.ReadAllBytes("Files/Country.json"));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}

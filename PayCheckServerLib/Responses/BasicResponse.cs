using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses
{
    internal class BasicResponse
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
                {
                    new()
                    {
                        Alias = "eu-central-1",
                        Region = "eu-central-1",
                        Status = "ACTIVE",
                        Ip = "127.0.0.1",
                        LastUpdate = "2023-08-06T10:00:00.000000000Z",
                        Port = 6969
                    },
                    new()
                    {
                        Alias = "eu-north-1",
                        Region = "eu-north-1",
                        Status = "ACTIVE",
                        Ip = "127.0.0.1",
                        LastUpdate = "2023-08-06T10:00:00.000000000Z",
                        Port = 6969
                    },
                    new()
                    {
                        Alias = "eu-west-1",
                        Region = "eu-west-1",
                        Status = "ACTIVE",
                        Ip = "127.0.0.1",
                        LastUpdate = "2023-08-06T10:00:00.000000000Z",
                        Port = 6969
                    }
                }
            };
            response.SetBody(JsonConvert.SerializeObject(rsp, Formatting.Indented));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/basic/v1/public/misc/time")]
        public static bool Time(HttpRequest request, PC3Server.PC3Session session)
        {
            var cookie = session.Headers["cookie"];
            var x = cookie.Split(";");
            Dictionary<string, string> Cookies = new();
            foreach (var item in x)
            {
                var kv = item.Split("=");
                Cookies.Add(kv[0].ToLower(), kv[1]);
            }

            var access_token = Cookies["access_token"];
            var token = TokenHelper.ReadToken(access_token);
            Debugger.PrintDebug($"{token.UserId}({token.Name}) Is still in the server!");

            //todo send friendstatus online in WSS


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
    }
}

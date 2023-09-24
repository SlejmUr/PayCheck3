﻿using NetCoreServer;
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
                {
                    new()
                    {
                        Alias = "eu-central-1",
                        Region = "eu-central-1",
                        Status = "ACTIVE",
                        Ip = ConfigHelper.ServerConfig.Hosting.IP,
                        LastUpdate = "2023-08-06T10:00:00.000000000Z",
                        Port = ConfigHelper.ServerConfig.Hosting.UDP_PORT
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
            var tokens = TokenHelper.ReadFromHeader(session.Headers);
            Debugger.PrintDebug($"{tokens.AccessToken.UserId}({tokens.AccessToken.Name}) Is still in the server!");
            
            //todo send friendstatus online in WSS -NOT
            //save the state if still active if not active then X sec then send offline into wss.


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

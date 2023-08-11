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
    public class Telemetry
    {
        [HTTP("POST", "/game-telemetry/v1/protected/events")]
        public static bool ProtectedEvents(HttpRequest request, PC3Server.PC3Session session)
        {
            session.SendResponse(session.Response.MakeOkResponse());
            return true;
        }
    }
}

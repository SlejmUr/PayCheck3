using NetCoreServer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib.Responses
{
    public class DLCSync
    {
        public class PutDLC
        { 
            public string steamId { get; set; }
            public string appId { get; set; }
        }


        [HTTP("PUT", "/platform/public/namespaces/pd3/users/{UserId}/dlc/steam/sync")]
        public static bool PUT_DLC_SteamSync(HttpRequest request, PC3Server.PC3Session session)
        {
            //var body = JsonConvert.DeserializeObject<PutDLC>(request.Body);
            ResponseCreator response = new ResponseCreator(204);
            response.SetHeader("Content-Type", "application/json");
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}

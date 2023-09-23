using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayCheckServerLib;
using PayCheckServerLib.Jsons.GS;
using PayCheckServerLib.Helpers;

namespace DS_Server.Helpers
{
    public class DS_Helper
    {
        //  GameSessionId => DSSesver
        public static Dictionary<string, DSInformationServer> DS_Servers = new();

        public static DSInformationServer MakeServer(string version, string region,string GameSessionId)
        {
            //CURRENTLY EVERYTHING HARDCODED!
            DSInformationServer server = new DSInformationServer()
            { 
                SessionId = GameSessionId,
                Deployment = "pveheistv2",
                GameVersion = version, //edit here if you want to support new/old version
                Source = "",
                AlternateIps = new List<string>()
                { 
                    ""
                },
                Status = "BUSY",
                AmsProtocol = null,
                CustomAttribute = "",
                ImageVersion = "127.0.0.1/test-server:" + version,  //TESTING!
                Ip = "127.0.0.1",
                IsOverrideGameVersion = false,
                LastUpdate = DateTime.UtcNow.ToString("o"),
                Namespace = "pd3",
                PodName = "test-server-"+region+ "-pveheistv2-"+version+"-"+Guid.NewGuid(),
                Port = 6969,
                Ports = new()
                { 
                    Beacon = 6970
                },
                Protocol = "udp",
                Provider = "aws",
                Region = region
            };
            DS_Servers.Add(GameSessionId, server);
            return server;
        }
    }
}

using NetCoreServer;
using System.Security.Authentication;

namespace DS_Server
{
    public class MiddleManStarter
    {
        static MiddleManClient? Client;
        static List<DS_UDPServer> DS_UDPServers = new List<DS_UDPServer>();
        public static void Start()
        {
            var context = new SslContext(SslProtocols.Tls12, PayCheckServerLib.PC3Server.GetCert());
            Client = new MiddleManClient(context, PayCheckServerLib.Helpers.ConfigHelper.ServerConfig.Hosting.IP, 443);
            Client.Connect();
            DS_UDPServers.Add(new("127.0.0.1", 6969));
            DS_UDPServers.Add(new("127.0.0.1", 6970));
        }

        public static void Stop()
        {
            Client?.Disconnect();
            foreach (var item in DS_UDPServers)
            {
                item.Stop();
            }
        }
    }
}

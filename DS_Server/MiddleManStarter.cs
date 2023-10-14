using NetCoreServer;
using System.Security.Authentication;
using ModdableWebServer.Helper;

namespace DS_Server
{
    public class MiddleManStarter
    {
        static MiddleManClient? Client;
        static List<DS_UDPServer> DS_UDPServers = new List<DS_UDPServer>();
        public static void Start()
        {
            var context = new SslContext(SslProtocols.Tls12, CertHelper.GetCert("cert.pfx","cert"));
            Client = new MiddleManClient(context, PayCheckServerLib.Helpers.ConfigHelper.ServerConfig.Hosting.IP, 443);
            Client.Connect();
            DS_UDPServers.Add(new("127.0.0.1", 6969));  // THIS is a Qos Server
            DS_UDPServers.Add(new("127.0.0.1", 6970));  // This is a Beacon Server
            foreach (var item in DS_UDPServers)
            {
                item.Start();
            }
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

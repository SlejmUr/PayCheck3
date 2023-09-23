using NetCoreServer;
using System.Security.Authentication;

namespace DS_Server
{
    public class MiddleManStarter
    {
        static MiddleManClient? Client;
        public static void Start()
        {
            var context = new SslContext(SslProtocols.Tls12, PayCheckServerLib.PC3Server.GetCert());
            Client = new MiddleManClient(context, PayCheckServerLib.Helpers.ConfigHelper.ServerConfig.Hosting.IP, 443);
            Client.Connect();
        }

        public static void Stop()
        {
            Client?.Disconnect();
        }
    }
}

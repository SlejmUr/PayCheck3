using Newtonsoft.Json;
using PayCheckServerLib;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PayCheck3ServerApp
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            /*
            var token = TokenHelper.GenerateNewToken();
            Console.WriteLine(token.ToPrint());oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo
            Console.WriteLine(token.ToBase64());
            token.PlatformType = TokenHelper.TokenPlatform.Device;
            token.PlatformId = UserIdHelper.CreateNewID();
            TokenHelper.StoreToken(token);
            var tok = TokenHelper.ReadToken(token.UserId);
            Console.WriteLine(tok.ToPrint());
            */
            ServerManager.Start();
            Console.ReadLine();
            ServerManager.Stop();
            /*
            PC3Server.Start("127.0.0.1",443);
            var gSTATICServer = new GSTATICServer.GSServer("127.0.0.1",80);
            PD3UDPServer pd3udp = new PD3UDPServer("127.0.0.1",6969);
            gSTATICServer.Start();
            pd3udp.Start();
            Console.ReadLine();
            PC3Server.Stop();
            gSTATICServer.Stop();
            pd3udp.Stop();*/

        }
        
    }
}
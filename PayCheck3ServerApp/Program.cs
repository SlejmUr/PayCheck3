using Newtonsoft.Json;
using PayCheckServerLib;
using System.Text;

namespace PayCheck3ServerApp
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            PC3Server.Start("127.0.0.1",443);
            var gSTATICServer = new GSTATICServer.GSServer("127.0.0.1",80);
            PD3UDPServer pd3udp = new PD3UDPServer("127.0.0.1",6969);
            gSTATICServer.Start();
            pd3udp.Start();
            Console.ReadLine();
            PC3Server.Stop();
            gSTATICServer.Stop();
            pd3udp.Stop();
        }
    }
}
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
            gSTATICServer.Start();
            Console.ReadLine();
            PC3Server.Stop();
            gSTATICServer.Stop();

        }
    }
}
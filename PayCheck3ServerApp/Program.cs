using Newtonsoft.Json;
using PayCheckServerLib;

namespace PayCheck3ServerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server(s)!");
            ServerManager.UpdateFinished += ServerManager_UpdateFinished;
            ServerManager.Pre();
        }

        private static void ServerManager_UpdateFinished(object? sender, bool e)
        {
            ServerManager.Start();
            Console.WriteLine("Enter q to quit.");
            string stop = "";
            while (stop != "q")
            {
                stop = Console.ReadLine();
            };
            ServerManager.Stop();
        }
    }
}
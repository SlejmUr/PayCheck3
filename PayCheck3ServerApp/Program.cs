using Newtonsoft.Json;
using PayCheckServerLib;
using PayCheckServerLib.Helpers;
using System.Text;

namespace PayCheck3ServerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server(s)!");
            ServerManager.UpdateFinished += ServerManager_UpdateFinished;

            if (File.Exists("beta_build"))
            { 
                Updater.DownloadBetaFiles();
                //force false to prevent update to newest versions
                ConfigHelper.ServerConfig.EnableAutoUpdate = false;
            }
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
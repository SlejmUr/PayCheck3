using PayCheckServerLib;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.ModdableWebServerExtensions;

namespace PayCheck3ServerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server(s)!");
            ServerManager.UpdateFinished += ServerManager_UpdateFinished;

            ArgumentHandler.MainArg(args);
            if (ArgumentHandler.AskHelp)
                ArgumentHandler.PrintHelp();

            if (ArgumentHandler.UseBetaFiles)
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
                stop = Console.ReadLine()!;
				stop = stop.ToLower().Trim();

				if(stop.StartsWith("grantitem"))
				{
					// Feature to grant items to a user on the server for debugging purposes
					// "grantitem <userid> <namespace> <itemsku> <quantity>"

					if(stop.Split(' ').Length == 5)
					{
						try
						{
							var namespace_ = stop.Split(" ")[1];
							var userId = stop.Split(" ")[2];
							var itemsku = stop.Split(" ")[3];
							int quantity = -1;
							var quantityParseSuccess = int.TryParse(stop.Split(" ")[4], out quantity);
							if (quantityParseSuccess)
							{
								PayCheckServerLib.Helpers.UserEntitlementHelper.AddEntitlementToUserViaSKU(userId, namespace_, itemsku, quantity, out List<EntitlementsData> addedEntitlements);


								foreach(var entitlement in addedEntitlements)
								{
									Console.WriteLine($"Successfully added entitlement: {entitlement.Name} to user {userId}");
								}
							}
						}
						catch
						{
							Console.WriteLine($"Invalid syntax for grantitem (\"grantitem <namespace> <userid> <itemsku> <quantity>\")");
						}
					} else
					{
						Console.WriteLine($"Invalid syntax for grantitem (\"grantitem <namespace> <userid> <itemsku> <quantity>\")");
					}
				}
            };
            ServerManager.Stop();
        }
    }
}
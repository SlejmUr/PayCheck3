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
							var userId = stop.Split(" ")[1];
							var namespace_ = stop.Split(" ")[2];
							var itemsku = stop.Split(" ")[3];
							int quantity = -1;
							var quantityParseSuccess = int.TryParse(stop.Split(" ")[4], out quantity);
							if (quantityParseSuccess)
							{
								EntitlementsData? addedEntitlement;
								PayCheckServerLib.Helpers.UserEntitlementHelper.AddEntitlementToUserViaSKU(userId, namespace_, itemsku, quantity, out addedEntitlement);

								if (addedEntitlement == null)
								{
									Console.WriteLine($"Unknown error occurred while adding sku {itemsku} to user, is the user id correct?");
								}
								else
								{
									Console.WriteLine($"Successfully added entitlement with id: {addedEntitlement.Id} and sku: {itemsku} to user {userId}");
								}
							}
						}
						catch
						{
							Console.WriteLine($"Invalid syntax for grantitem (\"grantitem <userid> <namespace> <itemsku> <quantity>\")");
						}
					} else
					{
						Console.WriteLine($"Invalid syntax for grantitem (\"grantitem <userid> <namespace> <itemsku> <quantity>\")");
					}
				}
            };
            ServerManager.Stop();
        }
    }
}
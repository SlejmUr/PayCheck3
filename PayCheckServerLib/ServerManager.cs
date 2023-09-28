using PayCheckServerLib.Helpers;

namespace PayCheckServerLib
{
    public class ServerManager
    {
        static GSTATICServer.GSServer STATICServer;
        /// <summary>
        /// Use this to check if update finished (Either Cancelled or Success)
        /// </summary>
        public static event EventHandler<bool> UpdateFinished;
        public static void Pre()
        {
            Debugger.logger.Info("Lib Info: " + BranchHelper.GetBranch() + " - " + BranchHelper.GetBuildDate() + " " + BranchHelper.GetCommitId());
            if (ConfigHelper.ServerConfig.InDevFeatures.GiveMeMoney > 0)
            {
                Debugger.PrintDebug("GiveMeMoney Cheat Activated");
            }
            if (ConfigHelper.ServerConfig.EnableAutoUpdate)
                Updater.CheckForJsonUpdates();
            UpdateFinished?.Invoke(null, true);

        }

        public static void Start()
        {
            if (ConfigHelper.ServerConfig.Hosting.WSS)
                PC3Server.Start(ConfigHelper.ServerConfig.Hosting.IP, 443);
            if (ConfigHelper.ServerConfig.Hosting.Gstatic)
            {
                STATICServer = new GSTATICServer.GSServer(ConfigHelper.ServerConfig.Hosting.IP, 80);
                STATICServer.Start();
            }
        }

        public static void Stop()
        {
            if (ConfigHelper.ServerConfig.Hosting.WSS)
                PC3Server.Stop();
            if (ConfigHelper.ServerConfig.Hosting.Gstatic)
                STATICServer.Stop();
        }
    }
}

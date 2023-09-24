﻿using PayCheckServerLib.Helpers;

namespace PayCheckServerLib
{
    public class ServerManager
    {
        static GSTATICServer.GSServer STATICServer;
        static PD3UDPServer UDPServer;
        /// <summary>
        /// Use this to check if update finished (Either Cancelled or Success)
        /// </summary>
        public static event EventHandler<bool> UpdateFinished;
        public static void Pre()
        {
            Debugger.logger.Info("Lib Info: " + BranchHelper.GetBranch() + " - " + BranchHelper.GetBuildDate() + " " + BranchHelper.GetCommitId());
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
            /*
            if (ConfigHelper.ServerConfig.Hosting.Udp)
            {
                UDPServer = new PD3UDPServer(ConfigHelper.ServerConfig.Hosting.IP, ConfigHelper.ServerConfig.Hosting.UDP_PORT);
                UDPServer.Start();
            }
            */
        }

        public static void Stop()
        {
            if (ConfigHelper.ServerConfig.Hosting.WSS)
                PC3Server.Stop();
            if (ConfigHelper.ServerConfig.Hosting.Gstatic)
                STATICServer.Stop();
            /*
            if (ConfigHelper.ServerConfig.Hosting.Udp)
                UDPServer.Stop();
            */
        }
    }
}

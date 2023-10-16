using ModdableWebServer.Helper;
using ModdableWebServer.Servers;
using NetCoreServer;
using PayCheckServerLib.Helpers;
using System.Diagnostics;
using System.Reflection;

namespace PayCheckServerLib
{
    public class ServerManager
    {
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
            if (ArgumentHandler.Debug)
                DebugPrinter.PrintToConsole = true;
            else
                DebugPrinter.PrintToConsole = false;

            if (ArgumentHandler.NoUpdate)
            {
                UpdateFinished?.Invoke(null, true);
                return;
            }

            if (ConfigHelper.ServerConfig.EnableAutoUpdate || ArgumentHandler.ForceUpdate)
                Updater.CheckForJsonUpdates();
            UpdateFinished?.Invoke(null, true);

        }
        static WSS_Server? server = null;
        static HTTP_Server? gserver = null;
        public static void Start()
        {
            if (ConfigHelper.ServerConfig.Hosting.WSS)
            {
                var context = CertHelper.GetContext( System.Security.Authentication.SslProtocols.Tls12 , "cert.pfx", "cert");
                server = new(context, ConfigHelper.ServerConfig.Hosting.IP, 443);
                server.ReceivedFailed += ReceivedFailed;
                server.Started += Server_Started;
                server.Stopped += Server_Stopped;
                server.HTTP_AttributeToMethods.Merge(Assembly.GetAssembly(typeof(ConfigHelper)));
                server.WS_AttributeToMethods = AttributeMethodHelper.UrlWSLoader(Assembly.GetAssembly(typeof(ConfigHelper)));
                server.WSError += WSError;
                server.Start();
            }
            if (ConfigHelper.ServerConfig.Hosting.Gstatic)
            {
                gserver = new(ConfigHelper.ServerConfig.Hosting.IP, 80);
                gserver.AttributeToMethods.Merge(Assembly.GetAssembly(typeof(ConfigHelper)));
                gserver.Start();
            }
        }

        public static void Stop()
        {
            if (ConfigHelper.ServerConfig.Hosting.WSS)
                server?.Stop();
            if (ConfigHelper.ServerConfig.Hosting.Gstatic)
                gserver?.Stop();
            server = null;
            gserver = null;
        }

        #region Actions
        private static void Server_Stopped(object? sender, EventArgs e)
        {
            Console.WriteLine("HTTPS Server stopped!");
        }

        private static void Server_Started(object? sender, (string address, int port) e)
        {
            Console.WriteLine("HTTPS Server started!");
        }

        private static void ReceivedFailed(object? sender, HttpRequest request)
        {
            File.AppendAllText("REQUESTED.txt", request.Url + "\n" + request.Method + "\n" + request.Body + "\n");
            Debugger.logger.Debug(request.Url + "\n" + request);
            Console.WriteLine("something isnt good");
        }
        private static void WSError(object? sender, string error)
        {
            Console.WriteLine("WSError! " + error);
            Debugger.PrintDebug($"Server reported error: {error}");
            StackTrace st = new StackTrace(true);
            for (int i = 0; i < st.FrameCount; i++)
            {
                var sf = st.GetFrame(i);
                if (sf == null)
                    continue;
                Debugger.PrintDebug("");
                Debugger.PrintDebug($"Method: " + sf.GetMethod());
                Debugger.PrintDebug($"File: " + sf.GetFileName());
                Debugger.PrintDebug($"Line Number: " + sf.GetFileLineNumber());
                Debugger.PrintDebug("");
            }
        }
        #endregion
    }
}

using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using ModdableWebServer.Servers;
using NetCoreServer;
using Newtonsoft.Json.Linq;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using System.Diagnostics;
using System.Reflection;

namespace PayCheckServerLib
{
	public class ServerManager
	{
		public class PayCheck3UrlNotHandledException : Exception {
			public string Method { get; private set; }
			public string Url { get; private set; }
			public PayCheck3UrlNotHandledException(string method, string url)
			{
				Method = method;
				Url = url;
			}
		}
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
			if (ArgumentHandler.DebugAll)
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
		static PayCheckServerLib.ModdableWebServerExtensions.PayCheck3WSSServer? server = null;
		static HTTP_Server? gserver = null;

		static Thread? FileSaveThread = null;
		public static void Start()
		{
			if (ConfigHelper.ServerConfig.Hosting.WSS)
			{
				if (!File.Exists("cert.pfx"))
				{
					Debugger.PrintError("Fatal Error: cert.pfx is not present, PayCheck3 is unable to serve HTTPS traffic, please generate a SSL certificate or place one in the install folder");
					Debugger.PrintInfo("As no certificate file is present, PayCheck3 will not start.");
				} else
				{
					var context = CertHelper.GetContextNoValidate(System.Security.Authentication.SslProtocols.Tls12, "cert.pfx", ConfigHelper.ServerConfig.Hosting.CertificatePassword);
					server = new(context, ConfigHelper.ServerConfig.Hosting.IP, ConfigHelper.ServerConfig.Hosting.Port);
					server.ReceivedFailed += ReceivedFailed;
					server.Started += Server_Started;
					server.Stopped += Server_Stopped;

					//server.HTTP_AttributeToMethods.Merge(Assembly.GetAssembly(typeof(ConfigHelper)));
					//server.WS_AttributeToMethods = AttributeMethodHelper.UrlWSLoader(Assembly.GetAssembly(typeof(ConfigHelper)));

					server.WSError += WSError;
					server.Start();
				}
			}
			if (ConfigHelper.ServerConfig.Hosting.Gstatic)
			{
				gserver = new(ConfigHelper.ServerConfig.Hosting.IP, 80);
				gserver.AttributeToMethods.Merge(Assembly.GetAssembly(typeof(ConfigHelper)));
				gserver.Start();
			}

			FileReadWriteHelper.ShouldRunCachedFilesThread = true;
			FileSaveThread = new Thread(() => { FileReadWriteHelper.SaveCachedFilesThread(); });
			FileSaveThread.Start();
			Debugger.PrintInfo("Started file saving thread");

			Debugger.PrintInfo($"PayCheck3 branch: {BranchHelper.GetBranch()} commit: {BranchHelper.GetCommitId()} commit date: {BranchHelper.GetBuildDate()}");
		}

		public static void Stop()
		{
			if (ConfigHelper.ServerConfig.Hosting.WSS)
				server?.Stop();
			if (ConfigHelper.ServerConfig.Hosting.Gstatic)
				gserver?.Stop();
			if(FileSaveThread != null)
			{
				FileReadWriteHelper.ShouldRunCachedFilesThread = false;
				FileSaveThread.Join();
			}
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
			Debugger.PrintError(String.Format("Url not handled! {0} : {1}", request.Method, request.Url));
#if DEBUG
			throw new PayCheck3UrlNotHandledException(request.Method, request.Url);
#endif
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

		[HTTP("GET", "/favicon.ico")]
		public static bool GetFavicon(HttpRequest _, ServerStruct serverStruct)
		{
			var response = new ResponseCreator(204);
			serverStruct.Response = response.GetResponse();
			PayCheckServerLib.ModdableWebServerExtensions.SendResponseOverride.SendResponse(serverStruct);
			//serverStruct.SendResponse();
			return true;
		}
	}
}

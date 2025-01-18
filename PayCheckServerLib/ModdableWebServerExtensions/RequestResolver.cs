using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using System.Reflection;


namespace PayCheckServerLib.ModdableWebServerExtensions
{
	public static class SendResponseOverride
	{
		public static void SendResponse(this ServerStruct serverStruct)
		{
			if (serverStruct.Response != null)
			{
				if(serverStruct.Response.Body.Length == 0)
				{
					Debugger.PrintError($"Zero sized response body!");
				}
			}
			switch (serverStruct.Enum)
			{
				case ServerEnum.HTTP:
					serverStruct.HTTP_Session?.SendResponse(serverStruct.Response);
					DebugPrinter.Debug("[SendResponse] HTTP Response Sent!");
					break;
				case ServerEnum.HTTPS:
					serverStruct.HTTPS_Session?.SendResponse(serverStruct.Response);
					DebugPrinter.Debug("[SendResponse] HTTPS Response Sent!");
					break;
				case ServerEnum.WS:
					serverStruct.WS_Session?.SendResponse(serverStruct.Response);
					DebugPrinter.Debug("[SendResponse] WS Response Sent!");
					break;
				case ServerEnum.WSS:
					serverStruct.WSS_Session?.SendResponse(serverStruct.Response);
					DebugPrinter.Debug("[SendResponse] WSS Response Sent!");
					break;
			}
		}
	}
	public class RequestResolver
	{

		// Identical to https://github.com/SlejmUr/ModdableWebServer/blob/master/ModdableWebServer/Helper/RequestSender.cs#L9, but modified to check for user permissions and authorizations if required
		public static bool SendRequestHTTP(ServerStruct server, HttpRequest request, Dictionary<HTTPAttribute, MethodInfo> AttributeToMethods)
		{
			Dictionary<string, string> Parameters = new();
			string url = request.Url;
			url = Uri.UnescapeDataString(url);
			//DebugPrinter.Debug($"[SendRequestHTTP] Requesting with URL: {url}");

			bool Sent = false;
			foreach (var item in AttributeToMethods) {
				if ((ModdableWebServer.Helper.UrlHelper.Match(url, item.Key.url, out Parameters) || item.Key.url == url) && request.Method.ToUpper() == item.Key.method.ToUpper()) {

					//DebugPrinter.Debug($"[SendRequestHTTP] URL Matched! {url}");

					//var authRequiredAttribute = item.Value.GetCustomAttribute<AuthenticationRequiredAttribute>();
					//if (authRequiredAttribute != null)
					//{
					//	Debugger.PrintInfo($"{authRequiredAttribute.PermissionRequired} : {authRequiredAttribute.AccessRequired}");
					//}

					server.Headers = request.GetHeaders();
					server.Parameters = Parameters;

					Sent = (bool)item.Value.Invoke(server, new object[] { request, server })!;

					Debugger.PrintInfo($"{item.Key.method} : {url}");

					//DebugPrinter.Debug("[SendRequestHTTP] Invoked!");
					break;
				}

			}
			return Sent;
		}
	}
}

using ModdableWebServer;
using ModdableWebServer.Helper;
using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// This set of extensions serves to overwrite ModdableWebServer.Servers.WSS_Server.Session.OnReceivedRequest to call a custom defined request resolver instead of ModdableWebServer.Helper.RequestSender.SendRequestHTTP
// https://github.com/SlejmUr/ModdableWebServer/blob/fb17c720f5fc9efe615b28f74d50c03657534b1b/ModdableWebServer/Servers/WSS_Server.cs#L139
namespace PayCheckServerLib.ModdableWebServerExtensions
{
	public class PayCheck3WSSServer : ModdableWebServer.Servers.WSS_Server
	{
		public class PayCheck3Session : ModdableWebServer.Servers.WSS_Server.Session
		{
			public PayCheck3Session(WssServer server) : base(server)
			{
			}

			protected override void OnReceivedRequest(HttpRequest request)
			{
				ServerStruct serverStruct = new ServerStruct()
				{
					WSS_Session = this,
					Response = this.Response,
					Enum = ServerEnum.WSS
				};

				if (request.GetHeaders().ContainsValue("websocket"))
				{
					base.OnReceivedRequest(request);
					return;
				}

				bool IsSent = RequestResolver.SendRequestHTTP(serverStruct, request, WSS_Server.HTTP_AttributeToMethods);
				bool IsSentHeader = ModdableWebServer.Helper.RequestSender.SendRequestHTTPHeader(serverStruct, request, WSS_Server.HeaderAttributeToMethods);

				if (!IsSent && !IsSentHeader)
					WSS_Server.ReceivedFailed?.Invoke(this, request);

				if (WSS_Server.DoReturn404IfFail && (!IsSent && !IsSentHeader))
					SendResponse(Response.MakeErrorResponse(404));
			}


			internal WebSocketStruct ws_Struct;
			public override bool OnWsConnecting(HttpRequest request, HttpResponse response)
			{
				ws_Struct = new()
				{
					IsConnecting = true,
					IsConnected = false,
					IsClosed = false,
					Request = new()
					{
						Body = request.Body,
						Url = request.Url,
						Headers = request.GetHeaders()
					},
					WSRequest = null,
					Enum = WSEnum.WSS,
					WS_Session = null,
					WSS_Session = this
				};
				ws_Struct.SendRequestWS(WSS_Server.WS_AttributeToMethods);
				return true;//base.OnWsConnecting(request, response);
			}
		}
		public PayCheck3WSSServer(SslContext context, DnsEndPoint endPoint) : base(context, endPoint)
		{
		}

		//public PayCheck3WSSServer(SslContext context, IPEndPoint endPoint) : base(context, endPoint)
		public PayCheck3WSSServer(SslContext context, IPEndPoint endPoint) : base(context, endPoint)
		{
		}

		public PayCheck3WSSServer(SslContext context, string address, int port) : base(context, address, port)
		{
			HTTP_AttributeToMethods = AttributeMethodHelper.UrlHTTPLoader(Assembly.GetAssembly(typeof(ServerManager)));
			WS_AttributeToMethods = AttributeMethodHelper.UrlWSLoader(Assembly.GetAssembly(typeof(ServerManager)));
		}

		public PayCheck3WSSServer(SslContext context, IPAddress address, int port) : base(context, address, port)
		{
		}

		protected override SslSession CreateSession()
		//protected override TcpSession CreateSession()
		{
			return new PayCheck3Session(this);
		}
	}
}

using NetCoreServer;
using System.Net.Sockets;
using System.Text;

namespace DS_Server
{
    public class MiddleManClient : WssClient
    {
        public MiddleManClient(SslContext context, string address, int port) : base(context, address, port) { }

        public override void OnWsConnecting(HttpRequest request)
        {
            request.SetBegin("GET", "/middleman");
            request.SetHeader("Host", PayCheckServerLib.Helpers.ConfigHelper.ServerConfig.Hosting.IP);
            request.SetHeader("Origin", "https://" + PayCheckServerLib.Helpers.ConfigHelper.ServerConfig.Hosting.IP);
            request.SetHeader("Upgrade", "websocket");
            request.SetHeader("Connection", "Upgrade");
            request.SetHeader("Sec-WebSocket-Key", Convert.ToBase64String(WsNonce));
            request.SetHeader("Sec-WebSocket-Protocol", "chat, superchat");
            request.SetHeader("Sec-WebSocket-Version", "13");
            request.SetHeader("MiddleMan", Id.ToString());
            request.SetBody();
        }
        public override void OnWsReceived(byte[] buffer, long offset, long size)
        {
            Console.WriteLine($"Incoming: {Encoding.UTF8.GetString(buffer, (int)offset, (int)size)}");
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat WebSocket client caught an error with code {error}");
        }

    }
}
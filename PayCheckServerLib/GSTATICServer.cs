using NetCoreServer;
using System.Net.Sockets;

namespace PayCheckServerLib
{
    public class GSTATICServer
    {
        public class GSServer : HttpServer
        {
            public GSServer(string address, int port) : base(address, port) { }

            protected override TcpSession CreateSession() { return new GSSession(this); }

            protected override void OnError(SocketError error)
            {
                Console.WriteLine($"HTTP session caught an error: {error}");
            }
            public class GSSession : HttpSession
            {
                public GSSession(HttpServer server) : base(server) { }

                protected override void OnReceivedRequest(HttpRequest request)
                {
                    // Show HTTP request content
                    Console.WriteLine(request.Url);
                    ResponseCreator response = new ResponseCreator(204);
                    SendResponse(response.GetResponse());
                }

                protected override void OnReceivedRequestError(HttpRequest request, string error)
                {
                    Console.WriteLine($"Request error: {error}");
                }

                protected override void OnError(SocketError error)
                {
                    Console.WriteLine($"HTTP session caught an error: {error}");
                }
            }
        }
    }
}

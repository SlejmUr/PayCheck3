using NetCoreServer;
using PayCheckServerLib.WSController;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PayCheckServerLib
{
    public class PC3Server
    {
        public static X509Certificate GetCert()
        {

            X509Certificate2 cert = new(File.ReadAllBytes($"cert.pfx"));
            return cert;
        }

        static PC3HTTPServer? server = null;
        static Dictionary<(string url, string method), MethodInfo> HttpServerThingy = new();
        public static void Start(string IP, int Port)
        {
            HttpServerThingy.Clear();
            var context = new SslContext(SslProtocols.Tls12, GetCert());
            server = new PC3HTTPServer(context, IP, Port);
            Console.WriteLine("[HTTPS] Server Started on https://" + IP + ":" + Port);
            server.Start();
            var methods = Assembly.GetExecutingAssembly().GetTypes().SelectMany(x => x.GetMethods()).ToArray();
            var basemethods = methods.Where(x => x.GetCustomAttribute<HTTPAttribute>() != null && x.ReturnType == typeof(bool)).ToArray();
            foreach (var method in basemethods)
            {
                if (method == null)
                    continue;
                var httpAttr = method.GetCustomAttribute<HTTPAttribute>();
                Debugger.PrintDebug(method.Name + $" ({httpAttr.url}) ({httpAttr.method}) is added as an URL", "HTTPServer");
                HttpServerThingy.Add((httpAttr.url, httpAttr.method), method);
            }
        }

        public static void Stop()
        {
            server?.Stop();
            server?.Dispose();
            server = null;
            Console.WriteLine("[HTTPS] Server Stopped");

        }



        public class PC3Session : WssSession
        {
            HttpRequest? _request;
            public HttpRequest? LastRequest()
            {
                return _request;
            }
            public WSEnum WhatTheHell = WSEnum.IDK;
            public enum WSEnum
            {
                IDK = -1,
                Lobby,
                Chat
            }

            public PC3Session(WssServer server) : base(server) { }
            public Dictionary<string, string> Headers = new();
            public Dictionary<string, string> HttpParam = new();

            public override void OnWsConnected(HttpRequest request)
            {
                if (request.Url == "/lobby/")
                {
                    WhatTheHell = WSEnum.Lobby;
                    var x = "type: connectNotif\r\nloginType: NewRegister\r\nreconnectFromCode: 5000\r\nlobbySessionID: ee62822a8428424d9a408f6385484ae5";
                    SendBinaryAsync(Encoding.UTF8.GetBytes(x));
                }
                if (request.Url == "/chat/")
                {
                    WhatTheHell = WSEnum.Chat;
                    var x = "CaSr{\"jsonrpc\":\"2.0\",\"method\":\"eventConnected\",\"params\":{\"sessionId\":\"9f51a15b940b4c538cc48281950de549\"}}CaEd";
                    SendBinaryAsync(Encoding.UTF8.GetBytes(x));
                }
            }

            public override void OnWsError(string error)
            {
                Debugger.PrintError($"Request error: {error}");
            }


            public override void OnWsReceived(byte[] buffer, long offset, long size)
            {
                var buf = buffer[..(int)size];
                Debugger.PrintInfo("OnWsReceived:" + WhatTheHell.ToString());
                switch (WhatTheHell)
                {
                    case WSEnum.Lobby:
                        LobbyControl.Control(buf, this);
                        return;
                    case WSEnum.Chat:
                        ChatControl.Control(buf, this);
                        return;
                    case WSEnum.IDK:
                    default:
                        return;

                }
            }

            protected override void OnReceivedRequest(HttpRequest request)
            {
                Headers.Clear();
                for (int i = 0; i < request.Headers; i++)
                {
                    var headerpart = request.Header(i);
                    Headers.Add(headerpart.Item1.ToLower(), headerpart.Item2);
                }
                string url = request.Url;
                url = Uri.UnescapeDataString(url);
                // Show HTTP request content
                if (Headers.ContainsKey("upgrade"))
                {
                    base.OnReceivedRequest(request);
                    return;
                }

                Debugger.PrintDebug(url);

                _request = request;
                bool Sent = false;
                foreach (var item in HttpServerThingy)
                {
                    if ((UrlHelper.Match(url, item.Key.url, out HttpParam) || item.Key.url == url) && request.Method == item.Key.method)
                    {
                        Debugger.PrintInfo("Url Called function: " + item.Value.Name);
                        item.Value.Invoke(this, new object[] { request, this });
                        Sent = true;
                        return;
                    }

                }

                if (!Sent)
                {
                    File.AppendAllText("REQUESTED.txt", url + "\n" + request.Method + "\n" + request.Body + "\n");
                    Debugger.logger.Debug(url + "\n" + request);
                }
                Console.WriteLine("something isnt good");
                //SendResponse(Response.MakeOkResponse());
            }

            protected override void OnReceivedRequestError(HttpRequest request, string error)
            {
                Debugger.PrintError($"Request error: {error}");
            }

            protected override void OnError(SocketError error)
            {
                Debugger.PrintDebug($"HTTP session caught an error: {error}");
            }
        }

        public class PC3HTTPServer : WssServer
        {
            public ConcurrentDictionary<Guid, PC3Session> Sessions = new();

            public PC3HTTPServer(SslContext context, string address, int port) : base(context, address, port)
            {
            }
            PC3Session? session;

            public PC3Session? GetSession()
            {
                return session;
            }

            protected override SslSession CreateSession()
            {
                session = new PC3Session(this);
                return session;
            }

            protected override void OnError(SocketError error) => Debugger.PrintDebug($"HTTP session caught an error: {error}");
        }
    }
}
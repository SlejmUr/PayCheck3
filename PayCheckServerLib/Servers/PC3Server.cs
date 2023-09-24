﻿using NetCoreServer;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.WSController;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PayCheckServerLib
{
    public class PC3Server
    {
        public static X509Certificate GetCert()
        {
            //read the cert.pfx
            X509Certificate2 cert = new(File.ReadAllBytes($"cert.pfx"), "cert");
            return cert;
        }

        static PC3WSSServer? server = null;
        static Dictionary<(string url, string method), MethodInfo> HttpServerThingy = new();
        public static void Start(string IP, int Port)
        {
            HttpServerThingy.Clear();
            var context = new SslContext(SslProtocols.Tls12, GetCert());
            server = new PC3WSSServer(context, IP, Port);
            Console.WriteLine("[HTTPS] Server Started on https://" + IP + ":" + Port);
            server.Start();
            var methods = Assembly.GetExecutingAssembly().GetTypes().SelectMany(x => x.GetMethods()).ToArray();
            var basemethods = methods.Where(x => x.GetCustomAttribute<HTTPAttribute>() != null && x.ReturnType == typeof(bool)).ToArray();
            foreach (var method in basemethods)
            {
                if (method == null)
                    continue;
                var httpAttr = method.GetCustomAttribute<HTTPAttribute>();
                if (httpAttr == null)
                    continue;
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
            public WSEnum WS_ID = WSEnum.IDK;
            public enum WSEnum
            {
                IDK = -1,
                Lobby,
                Chat,
                MiddleMan
            }

            
            public List<PC3Session> MiddleMans = new();

            public PC3Session? GetWSLobby(string UserId)
            {
                if (UserId == null)
                    Debugger.PrintError("UserId is null! (Parameter)");

                if (WSSServer().WSS_Stuff.TryGetValue(UserId + "_lobby", out var value))
                {
                    return value;
                }
                Debugger.PrintError($"User({UserId}_lobby) not found in WSS_Stuff!");
                return null;
            }

            public PC3Session GetWSChat(string UserId) => WSSServer().WSS_Stuff[UserId + "_chat"];

            public PC3WSSServer WSSServer() => (PC3WSSServer)Server;

            public string WSUserId = "";

            public PC3Session(WssServer server) : base(server) { }
            public Dictionary<string, string> Headers = new();
            public Dictionary<string, string> HttpParam = new();

            public override void OnWsConnected(HttpRequest request)
            {
                Headers.Clear();
                for (int i = 0; i < request.Headers; i++)
                {
                    var headerpart = request.Header(i);
                    Headers.Add(headerpart.Item1.ToLower(), headerpart.Item2);
                }
                string id = "";
                //There is a bug where this LobbySession empty and it contains the bearer token :)
                if (Headers.ContainsKey("x-ab-lobbysessionid"))
                {
                    id = Headers["x-ab-lobbysessionid"].Replace("Authorization: Bearer ", "");
                }
                else if (Headers.ContainsKey("middleman"))
                {
                    WSUserId = "MiddleManId-" + Headers["middleman"];
                    WS_ID = WSEnum.MiddleMan;
                    MiddleMans.Add(this);
                    Debugger.PrintInfo(WSUserId + " joined to " + WS_ID);
                    return;
                }
                else
                {
                    id = Headers["authorization"].Replace("Bearer ", "");
                }
                var token =  TokenHelper.ReadToken(id);
                if (request.Url == "/lobby/")
                {
                    WS_ID = WSEnum.Lobby;
                    WSUserId = token.UserId;
                    var serv = (PC3WSSServer)this.Server;
                    serv.WSUserIds.Add(WSUserId);
                    if (WSSServer().WSS_Stuff.ContainsKey(token.UserId + "_" + WS_ID.ToString().ToLower()))
                    {
                        Debugger.PrintWarn("The fuck? This User now wants to to join to WS again! " + WS_ID);
                    }
                    WSSServer().WSS_Stuff.Add(token.UserId + "_" + WS_ID.ToString().ToLower(), this);
                    var x = "type: connectNotif\r\nloginType: NewRegister\r\nreconnectFromCode: 5000\r\nlobbySessionID: ee62822a8428424d9a408f6385484ae5";
                    SendBinaryAsync(Encoding.UTF8.GetBytes(x));
                }
                if (request.Url == "/chat/")
                {
                    WS_ID = WSEnum.Chat;
                    WSUserId = token.UserId;
                    if (WSSServer().WSS_Stuff.ContainsKey(token.UserId + "_" + WS_ID.ToString().ToLower()))
                    {
                        Debugger.PrintWarn("The fuck? This User now wants to to join to WS again! " + WS_ID);
                    }
                    WSSServer().WSS_Stuff.Add(token.UserId + "_" + WS_ID.ToString().ToLower(), this);
                    var x = "CaSr{\"jsonrpc\":\"2.0\",\"method\":\"eventConnected\",\"params\":{\"sessionId\":\"9f51a15b940b4c538cc48281950de549\"}}CaEd";
                    SendBinaryAsync(Encoding.UTF8.GetBytes(x));
                }
                Debugger.PrintInfo(WSUserId + " joined to " + WS_ID);
                foreach (var wss_stuff in WSSServer().WSS_Stuff)
                {
                    Debugger.PrintInfo(wss_stuff.Key);
                }
            }

            public override void OnWsDisconnecting()
            {
                Console.WriteLine(WSUserId + " " + WS_ID + " quit");
                if (WS_ID == WSEnum.Lobby)
                {
                    var user = UserController.GetUser(WSUserId);
                    if (user == null)
                    {
                        Debugger.PrintWarn($"User not found! ({WSUserId}) WSS Cannot continue");
                    }
                    user.Status.activity = "nil";
                    user.Status.availability = "offline";
                    user.Status.platform = "nil";
                    user.Status.lastSeenAt = DateTime.UtcNow.ToString("O");
                    UserController.SaveUser(user);
                    Dictionary<string, string> rsp = new()
                    {
                        { "type", "userStatusNotif" },
                        { "userID", WSUserId },
                        { "availability", "offline" },
                        { "activity", "nil" },
                        { "platform", "nil" },
                        { "lastSeenAt", user.Status.lastSeenAt },
                    }; 
                    foreach (var id in WSSServer().WSUserIds)
                    {
                        if (id == WSUserId)
                            continue;
                        LobbyControl.SendToLobby(rsp, GetWSLobby(id));
                    }
                }
                WSSServer().WSS_Stuff.Remove(WS_ID + "_" + WS_ID.ToString().ToLower());
                var serv = (PC3WSSServer)Server;
                serv.WSUserIds.Remove(WSUserId);
                WSUserId = "";
            }

            public override void OnWsError(string error)
            {
                Debugger.PrintError($"Request error: {error}");
            }


            public override void OnWsReceived(byte[] buffer, long offset, long size)
            {
                var buf = buffer[..(int)size];
                var buf2 = buffer.Take((int)size).ToArray();
                if (size == 0)
                {
                    Debugger.PrintInfo("Nothing was sent to WSS");
                    return;
                }
                Debugger.PrintInfo(WSUserId + " on " + WS_ID + " WSS Recieved!");
                Debugger.logger.Debug(offset + " " + size);
                Debugger.logger.Debug(BitConverter.ToString(buffer));
                switch (WS_ID)
                {
                    case WSEnum.Lobby:
                        LobbyControl.Control(buffer, offset, size, this);
                        return;
                    case WSEnum.Chat:
                        ChatControl.Control(buffer, offset, size, this);
                        return;
                    case WSEnum.MiddleMan:
                        MiddleManControl.Control(buffer, offset, size, this);
                        return;
                    case WSEnum.IDK:
                    default:
                        Debugger.PrintInfo("We received WS Stuff but we dont know which!");
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
                        Debugger.logger.Debug(url + "\n" + request);
                        Debugger.PrintInfo("Url Called function: " + item.Value.Name);
                        bool ret = (bool)item.Value.Invoke(this, new object[] { request, this })!;
                        Sent = ret;
                        break;
                    }

                }

                if (!Sent)
                {
                    File.AppendAllText("REQUESTED.txt", url + "\n" + request.Method + "\n" + request.Body + "\n");
                    Debugger.logger.Debug(url + "\n" + request);
                    Console.WriteLine("something isnt good");
                }
                else
                { 
                    //Everything went fine!
                }
                //SendResponse(Response.MakeOkResponse());
            }

            protected override void OnReceivedRequestError(HttpRequest request, string error)
            {
                Debugger.PrintError($"Request error: {error}");
            }

            protected override void OnError(SocketError error)
            {
                Debugger.PrintDebug($"HTTPS session caught an error: {error}");
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

        }

        public class PC3WSSServer : WssServer
        {
            public List<string> WSUserIds;
            public Dictionary<string, PC3Session> WSS_Stuff = new();
            public PC3WSSServer(SslContext context, string address, int port) : base(context, address, port)
            {
                WSUserIds = new();
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


            protected override void OnError(SocketError error) 
            {
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
        }
    }
}
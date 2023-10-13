using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using System.Text;

namespace PayCheckServerLib.WSController
{
    public class LobbyControl
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        [WS("/lobby/")]
        public static void Lobby(WebSocketStruct socketStruct)
        {
            string auth_token;
            if (socketStruct.Request.Headers.ContainsKey("x-ab-lobbysessionid") && socketStruct.Request.Headers["x-ab-lobbysessionid"].Contains("Bearer"))
            {
                auth_token = socketStruct.Request.Headers["x-ab-lobbysessionid"].Replace("Authorization: Bearer ", "");
            }
            else
            {
                auth_token = socketStruct.Request.Headers["authorization"].Replace("Bearer ", "");
            }
            var token = TokenHelper.ReadToken(auth_token);
            var key = $"{token.Namespace}_{token.UserId}";
            if (socketStruct.IsConnected)
            {
                if (!LobbyUsers.ContainsKey(key))
                {
                    var x = "type: connectNotif\r\nloginType: NewRegister\r\nreconnectFromCode: 5000\r\nlobbySessionID: ee62822a8428424d9a408f6385484ae5";
                    socketStruct.SendWebSocketByteArray(Encoding.UTF8.GetBytes(x));
                    LobbyUsers.Add(key, socketStruct);
                }
            }
            else
            {
                var user = UserController.GetUser(token.UserId);
                if (user != null)
                {
                    user.Status.activity = "nil";
                    user.Status.availability = "offline";
                    user.Status.platform = "nil";
                    user.Status.lastSeenAt = DateTime.UtcNow.ToString("O");
                    UserController.SaveUser(user);
                    Dictionary<string, string> rsp = new()
                    {
                        { "type", "userStatusNotif" },
                        { "userID", token.UserId },
                        { "availability", "offline" },
                        { "activity", "nil" },
                        { "platform", "nil" },
                        { "lastSeenAt", user.Status.lastSeenAt },
                    };
                    foreach (var id in LobbyUsers.Keys)
                    {
                        //split
                        var splitted = id.Split("_");

                        if (splitted[1] == token.UserId)
                            continue;
                        if (splitted[0] != token.Namespace)
                            continue;

                        SendToLobby(rsp, GetLobbyUser(id, token.Namespace));
                    }
                }
                LobbyUsers.Remove(key);
            }

            if (socketStruct.WSRequest != null)
            {
                Control(socketStruct.WSRequest.Value.buffer, socketStruct.WSRequest.Value.offset, socketStruct.WSRequest.Value.size, socketStruct, token);
            }

        }

        public static Dictionary<string, WebSocketStruct> LobbyUsers = new();

        public static WebSocketStruct? GetLobbyUser(string UserId, string NameSpace)
        {
            var key = $"{NameSpace}_{UserId}";
            if (LobbyUsers.TryGetValue(key, out var webSocketStruct))
            {
                return webSocketStruct;
            }
            return null;
        }
        public static void Control(byte[] buffer, long offset, long size, WebSocketStruct socketStruct, TokenHelper.Token token)
        {
            if (size == 0)
                return;
            buffer = buffer.Take((int)size).ToArray();
            if (!Directory.Exists("Lobby")) { Directory.CreateDirectory("Lobby"); }
            var time = DateTime.Now.ToString("s").Replace(":", "-");
            File.WriteAllBytes("Lobby/" + time + ".bytes", buffer);
            var str = Encoding.UTF8.GetString(buffer);
            Dictionary<string, string> kv = new();
            var strArray = str.Split("\n");
            Debugger.PrintWebsocket(strArray.Count().ToString());
            foreach (var item in strArray)
            {
                if (string.IsNullOrEmpty(item) || item == "\n")
                    continue;
                var kv2 = item.Split(": ");
                Console.WriteLine(item);
                kv.Add(kv2[0], kv2[1]);
            }
            Debugger.PrintWebsocket("KVs done!");
            SwitchingType(kv, socketStruct, token);
            //Now doing some magic here for stuff.
        }

        static void SwitchingType(Dictionary<string, string> kv, WebSocketStruct socketStruct, TokenHelper.Token token)
        {
            try
            {
                User? user;
                Dictionary<string, string> rsp = new();
                Debugger.PrintDebug("[" + kv["type"] + "]");
                switch (kv["type"])
                {
                    case "setUserStatusRequest":
                        rsp.Add("type", "setUserStatusResponse");
                        rsp.Add("id", kv["id"]);
                        rsp.Add("code", "0");
                        user = UserController.GetUser(token.UserId);
                        if (user == null)
                        {
                            Debugger.PrintWarn($"User not found! ({token.UserId}) WSS Cannot continue");
                            new Exception("UserId is null");
                            break;
                        }
                        user.Status.activity = kv["activity"];
                        user.Status.availability = kv["availability"];
                        user.Status.platform = kv["platform"];
                        user.Status.lastSeenAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                        UserController.SaveUser(user);
                        SendToLobby(rsp, socketStruct);
                        rsp.Remove("type");
                        rsp.Remove("code");
                        rsp.Add("type", "userStatusNotif");
                        rsp.Add("userID", token.UserId);
                        rsp.Add("activity", kv["activity"]);
                        rsp.Add("availability", kv["availability"]);
                        rsp.Add("platform", kv["platform"]);
                        rsp.Add("lastSeenAt", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                        foreach (var id in LobbyUsers.Keys)
                        {
                            //split
                            var splitted = id.Split("_");

                            if (splitted[1] == token.UserId)
                                continue;
                            if (splitted[0] != token.Namespace)
                                continue;

                            SendToLobby(rsp, GetLobbyUser(id, token.Namespace));
                        }
                        break;
                    case "joinDefaultChannelRequest":
                        rsp.Add("type", "joinDefaultChannelResponse");
                        rsp.Add("id", kv["id"]);
                        rsp.Add("channelSlug", "default-channel");
                        rsp.Add("code", "0");
                        SendToLobby(rsp, socketStruct);
                        break;
                    case "listIncomingFriendsRequest":
                        rsp.Add("type", "listIncomingFriendsResponse");
                        rsp.Add("id", kv["id"]);
                        rsp.Add("code", "0");
                        rsp.Add("friendsId", "[]");
                        SendToLobby(rsp, socketStruct);
                        break;
                    case "listOutgoingFriendsRequest":
                        rsp.Add("type", "listOutgoingFriendsResponse");
                        rsp.Add("id", kv["id"]);
                        rsp.Add("code", "0");
                        rsp.Add("friendsId", "[]");
                        SendToLobby(rsp, socketStruct);
                        break;
                    case "friendsStatusRequest":
                        rsp.Add("type", "friendsStatusResponse");
                        rsp.Add("id", kv["id"]);
                        rsp.Add("code", "0");
                        user = UserController.GetUser(token.UserId);
                        if (user == null)
                        {
                            Debugger.PrintWarn($"User not found! ({token.UserId}) WSS Cannot continue");
                            throw new Exception("UserId is null");
                        }
                        List<string> friendsId = new();
                        List<string> availability = new();
                        List<string> activity = new();
                        List<string> platform = new();
                        List<string> lastSeenAt = new();
                        foreach (var fr in user.Friends)
                        {
                            var fud = fr.UserId;
                            friendsId.Add(fud);

                            var fuser = UserController.GetUser(fud);
                            if (fuser == null)
                            {
                                Debugger.PrintWarn($"User not found! ({token.UserId}) WSS Cannot continue");
                                throw new Exception("UserId is null");
                            }
                            availability.Add(fuser.Status.availability);
                            activity.Add(fuser.Status.activity);
                            platform.Add(fuser.Status.platform);
                            lastSeenAt.Add(fuser.Status.lastSeenAt);
                        }

                        rsp.Add("friendsId", ListToStr(friendsId));
                        rsp.Add("availability", ListToStr(availability));
                        rsp.Add("activity", ListToStr(activity));
                        rsp.Add("platform", ListToStr(platform));
                        rsp.Add("lastSeenAt", ListToStr(lastSeenAt));
                        SendToLobby(rsp, socketStruct);
                        break;
                    default:
                        Debugger.PrintWebsocket("Not sending back anything: " + kv["type"]);
                        break;
                }
            }
            catch (Exception ex)
            {
                Debugger.PrintError(ex.ToString());
            }


        }

        public static string ListToStr<T>(List<T> list) where T : class
        {
            var str = JsonConvert.SerializeObject(list);
            str = str.Replace("\"]", ",]");
            str = str.Replace("\"", "");
            return str;
        }


        public static void SendToLobby(Dictionary<string, string> kv, WebSocketStruct? socketStruct)
        {
            Debugger.PrintDebug("SendToLobby Called!");
            var str = "";
            foreach (var item in kv)
            {
                str += item.Key + ": " + item.Value + "\n";
            }

            str = str.Remove(str.Length - 1);
            Debugger.PrintDebug(str);
            socketStruct?.SendWebSocketByteArray(Encoding.UTF8.GetBytes(str));
        }
    }
}

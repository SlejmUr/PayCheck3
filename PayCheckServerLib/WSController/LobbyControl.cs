using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using System.Text;
using static PayCheckServerLib.PC3Server;

namespace PayCheckServerLib.WSController
{
    public class LobbyControl
    {
        public static void Control(byte[] buffer, long offset, long size, PC3Session session)
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
                Console.WriteLine(kv2[0]);
                kv.Add(kv2[0], kv2[1]);
            }
            Debugger.PrintWebsocket("KVs done!");
            SwitchingType(kv, session);
            //Now doing some magic here for stuff.
        }

        static void SwitchingType(Dictionary<string, string> kv, PC3Session session)
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
                        user = UserController.GetUser(session.WSUserId);
                        if (user == null)
                        {
                            Debugger.PrintWarn($"User not found! ({session.WSUserId}) WSS Cannot continue");
                            new Exception("UserId is null");
                            break;
                        }
                        user.Status.activity = kv["activity"];
                        user.Status.availability = kv["availability"];
                        user.Status.platform = kv["platform"];
                        user.Status.lastSeenAt = DateTime.UtcNow.ToString("O");
                        UserController.SaveUser(user);
                        SendToLobby(rsp, session);
                        rsp.Remove("type");
                        rsp.Remove("code");
                        rsp.Add("type", "userStatusNotif");
                        rsp.Add("userID", session.WSUserId);
                        rsp.Add("activity", kv["activity"]);
                        rsp.Add("availability", kv["availability"]);
                        rsp.Add("platform", kv["platform"]);
                        rsp.Add("lastSeenAt", DateTime.UtcNow.ToString("O"));
                        foreach (var id in session.WSSServer().WSUserIds)
                        { 
                            if (id == session.WSUserId)
                                continue;
                            SendToLobby(rsp, session.GetWSLobby(id));
                        }
                        break;
                    case "joinDefaultChannelRequest":
                        rsp.Add("type", "joinDefaultChannelResponse");
                        rsp.Add("id", kv["id"]);
                        rsp.Add("channelSlug", "default-channel");
                        rsp.Add("code", "0");
                        SendToLobby(rsp, session);
                        break;
                    case "listIncomingFriendsRequest":
                        rsp.Add("type", "listIncomingFriendsResponse");
                        rsp.Add("id", kv["id"]);
                        rsp.Add("code", "0");
                        rsp.Add("friendsId", "[]");
                        SendToLobby(rsp, session);
                        break;
                    case "listOutgoingFriendsRequest":
                        rsp.Add("type", "listOutgoingFriendsResponse");
                        rsp.Add("id", kv["id"]);
                        rsp.Add("code", "0");
                        rsp.Add("friendsId", "[]");
                        SendToLobby(rsp, session);
                        break;
                    case "friendsStatusRequest":
                        rsp.Add("type", "friendsStatusResponse");
                        rsp.Add("id", kv["id"]);
                        rsp.Add("code", "0");
                        user = UserController.GetUser(session.WSUserId);
                        if (user == null)
                        {
                            Debugger.PrintWarn($"User not found! ({session.WSUserId}) WSS Cannot continue");
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
                                Debugger.PrintWarn($"User not found! ({session.WSUserId}) WSS Cannot continue");
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
                        SendToLobby(rsp, session);
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
            str = str.Replace("\"]",",]");
            str = str.Replace("\"","");
            return str;
        }


        public static void SendToLobby(Dictionary<string, string> kv, PC3Session session)
        {
            var str = "";
            foreach (var item in kv)
            {
                str += item.Key + ": " + item.Value + "\n";
            }
            str = str.Remove(str.Length - 1);
            Debugger.PrintWebsocket(str);
            session.SendBinaryAsync(str);
        }
    }
}

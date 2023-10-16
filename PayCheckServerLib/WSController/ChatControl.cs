using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons.WSS;
using System.Text;

namespace PayCheckServerLib.WSController
{
    public class ChatControl
    {
        [WS("/chat/")]
        public static void Chat(WebSocketStruct socketStruct)
        {
            Debugger.PrintDebug("Chat");
            var auth_token = socketStruct.Request.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth_token);
            var key = $"{token.Namespace}_{token.UserId}";
            if (socketStruct.IsConnected)
            {
                if (!ChatUsers.ContainsKey(key))
                {
                    var x = "CaSr{\"jsonrpc\":\"2.0\",\"method\":\"eventConnected\",\"params\":{\"sessionId\":\"9f51a15b940b4c538cc48281950de549\"}}CaEd";
                    socketStruct.SendWebSocketByteArray(Encoding.UTF8.GetBytes(x));
                    ChatUsers.Add(key, socketStruct);
                }
            }
            else if (!socketStruct.IsConnecting)
            {
                ChatUsers.Remove(key);
            }

            if (socketStruct.WSRequest != null)
            {
                Control(socketStruct.WSRequest.Value.buffer, socketStruct.WSRequest.Value.offset, socketStruct.WSRequest.Value.size, socketStruct);
            }
        
        }

        public static Dictionary<string, WebSocketStruct> ChatUsers = new();

        public static WebSocketStruct? GetChatUser(string UserId, string NameSpace)
        {
            var key = $"{NameSpace}_{UserId}";
            if (ChatUsers.TryGetValue(key, out var webSocketStruct))
            {
                return webSocketStruct;
            }
            return null;
        }

        public static void Control(byte[] buffer, long offset, long size, WebSocketStruct socketStruct)
        {
            if (size == 0)
                return;
            buffer = buffer.Skip((int)offset).Take((int)size).ToArray();
            if (!Directory.Exists("Chat")) { Directory.CreateDirectory("Chat"); }
            File.WriteAllBytes("Chat/" + DateTime.Now.ToString("s").Replace(":", "-") + ".bytes", buffer);
            var str = Encoding.UTF8.GetString(buffer);
            var chatbase = JsonConvert.DeserializeObject<Chats.ChatBase>(str) ?? throw new Exception("chatbase is null!");
            switch (chatbase.Method)
            {
                case "actionQueryTopic":
                    {
                        Chats.actionQueryTopicRSP rsp = new()
                        {
                            Id = chatbase.Id,
                            Jsonrpc = chatbase.Jsonrpc,
                            Method = chatbase.Method,
                            Params = new()
                            {
                                Processed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()
                            }
                        };
                        var resp = "CaSr" + JsonConvert.SerializeObject(rsp) + "CaEd";
                        Console.WriteLine("Sending back: " + resp);
                        socketStruct.SendWebSocketByteArray(Encoding.UTF8.GetBytes(resp));
                    }
                    return;
                case "actionQueryTopicById":
                    {
                        //PLEASE HELP ME IF THIS WORKS OR NOT.
                        var idk = JsonConvert.DeserializeObject<Chats.actionQueryTopicById>(str);
                        var party = PartyController.PartySaves.Where(x => x.Value.Id == idk.Params.TopicId.Replace("p.", "")).FirstOrDefault().Value;
                        if (party == null)
                        {
                            Debugger.PrintError("NO Code???? WHAT THE FUCK");
                            throw new Exception("Code is not exist in saved parties????");
                        }
                        Chats.actionQueryTopicByIdRSP rsp = new()
                        {
                            Id = idk.Id,
                            Jsonrpc = "2.0",
                            Method = "actionQueryTopicById",
                            Result = new()
                            {
                                Processed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
                                Data = new()
                                {
                                    Name = "Party-" + party.Id,
                                    TopicId = idk.Params.TopicId,
                                    Type = "GROUP",
                                    UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
                                    Members = new()
                                    {
                                    }
                                }
                            }
                        };
                        foreach (var item in party.Members)
                        {
                            rsp.Result.Data.Members.Add(item.Id);
                        }
                        var resp = "CaSr" + JsonConvert.SerializeObject(rsp) + "CaEd";
                        Console.WriteLine("Sending back: " + resp);
                        socketStruct.SendWebSocketByteArray(Encoding.UTF8.GetBytes(resp));
                    }
                    return;
                default:
                    Debugger.PrintWebsocket("ChatControl: " + chatbase.Method);
                    return;
            }
        }

        public static void SendToChat(string Json, WebSocketStruct? socketStruct)
        {
            var resp = "CaSr" + Json + "CaEd";
            socketStruct?.SendWebSocketByteArray(Encoding.UTF8.GetBytes(resp));
        }
    }
}

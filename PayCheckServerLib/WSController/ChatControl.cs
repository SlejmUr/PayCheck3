using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons.WSS;
using System.Text;
using static PayCheckServerLib.PC3Server;

namespace PayCheckServerLib.WSController
{
    public class ChatControl
    {
        public static void Control(byte[] buffer, long offset, long size, PC3Session session)
        {
            if (size == 0)
                return;
            buffer = buffer.Take((int)size).ToArray();
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
                        session.SendTextAsync(resp);
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
                        session.SendTextAsync(resp);
                    }
                    return;
                default:
                    Debugger.PrintWebsocket("ChatControl: " + chatbase.Method);
                    return;
            }
        }

        public static void SendToChat(string Json, PC3Session session)
        {
            var resp = "CaSr" + Json + "CaEd";
            session.SendTextAsync(resp);
        }
    }
}

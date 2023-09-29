using Newtonsoft.Json;
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
                default:
                    Debugger.PrintWebsocket("ChatControl: " + chatbase.Method);
                    return;
            }
        }
    }
}

using Newtonsoft.Json;
using PayCheckServerLib.Jsons;
using System.Text;
using static PayCheckServerLib.PC3Server;

namespace PayCheckServerLib.WSController
{
    public class ChatControl
    {
        public static void Control(byte[] buffer, PC3Session session)
        {
            Console.WriteLine(BitConverter.ToString(buffer));
            if (buffer.Length != 0)
            {
                var str = Encoding.UTF8.GetString(buffer);
                var chatbase = JsonConvert.DeserializeObject<Chats.ChatBase>(str);
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
                                    Processed = "1691204630"
                                }
                            };
                            var resp = "CaSr" + JsonConvert.SerializeObject(rsp) + "CaEd";
                            Console.WriteLine("Sending back: " + resp);
                            session.SendTextAsync(resp);
                        }
                        return;
                    default:
                        Console.WriteLine("ChatControl: " + chatbase.Method);
                        return;
                
                }


            }
        }
    }
}

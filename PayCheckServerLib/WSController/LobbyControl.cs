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
                var kv2 = item.Split(": ");
                Console.WriteLine(kv2[0]);
                kv.Add(kv2[0], kv2[1]);
            }
            Debugger.PrintWebsocket("KVs done!");

            //Now doing some magic here for stuff.
        }

        public static void SendToLobby(Dictionary<string, string> kv, PC3Session session)
        {
            var str = "";
            foreach (var item in kv)
            {
                str = item.Key + ": " + item.Value + "\n";
            }
            str = str[..-2];
            Debugger.PrintWebsocket(str);
            session.SendBinaryAsync(str);
        }
    }
}

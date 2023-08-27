using System.Text;
using static PayCheckServerLib.PC3Server;

namespace PayCheckServerLib.WSController
{
    public class LobbyControl
    {
        public static void Control(byte[] buffer, PC3Session session)
        {
            if (buffer.Length != 0)
            {
                if (!Directory.Exists("Lobby")) { Directory.CreateDirectory("Lobby"); }
                File.WriteAllBytes("Lobby/" + DateTime.Now.ToString("s").Replace(":", "-") + ".bytes", buffer);
            }

            Debugger.logger.Debug("LobbyControl.BYTES:\n" + BitConverter.ToString(buffer));
            try
            {
                var str = Encoding.UTF8.GetString(buffer);
                Debugger.logger.Debug("LobbyControl.TEXT:\n" + str);
            }
            catch
            {
            }
        }
    }
}

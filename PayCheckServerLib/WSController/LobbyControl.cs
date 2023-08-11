using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PayCheckServerLib.PC3Server;

namespace PayCheckServerLib.WSController
{
    public class LobbyControl
    {
        public static void Control(byte[] buffer, PC3Session session)
        {
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

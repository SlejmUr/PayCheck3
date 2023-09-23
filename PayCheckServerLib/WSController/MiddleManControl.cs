using Newtonsoft.Json;
using PayCheckServerLib.Jsons;
using static PayCheckServerLib.PC3Server;

namespace PayCheckServerLib.WSController
{
    public class MiddleManControl
    {
        public static void Control(byte[] buffer, long offset, long size, PC3Session session)
        {
            if (size == 0)
                return;
            buffer = buffer.Take((int)size).ToArray();
            
        }
    }
}

using PayCheckServerLib.Helpers;

namespace PayCheckServerLib
{
    public class SaveHandler
    {
        static SaveHandler()
        {
            if (!Directory.Exists("Save")) { Directory.CreateDirectory("Save"); }
        }

        public static void SaveUser(string UserId, byte[] data)
        {
            File.WriteAllBytes($"Save/{UserId}.{ConfigHelper.ServerConfig.Saves.Extension}", data);
        }
        public static void SaveUser_Request(string UserId, string data)
        {
            File.WriteAllText($"Save/{UserId}_{DateTime.Now.ToString("s").Replace(":", "-")}.{ConfigHelper.ServerConfig.Saves.Extension}", data);
        }

        public static bool IsUserExist(string UserId)
        {
            return File.Exists($"Save/{UserId}.{ConfigHelper.ServerConfig.Saves.Extension}");
        }

        public static byte[] ReadUserByte(string UserId)
        {
            return File.ReadAllBytes($"Save/{UserId}.{ConfigHelper.ServerConfig.Saves.Extension}");
        }
        public static string ReadUserSTR(string UserId)
        {
            return File.ReadAllText($"Save/{UserId}.{ConfigHelper.ServerConfig.Saves.Extension}");
        }
    }
}

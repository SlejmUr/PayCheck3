using PayCheckServerLib.Helpers;

namespace PayCheckServerLib
{
    public class SaveHandler
    {
        static SaveHandler()
        {
            if (!Directory.Exists("Save")) { Directory.CreateDirectory("Save"); }
        }

        public static void SaveUser(string UserId, string NameSpace, byte[] data)
        {
            File.WriteAllBytes($"Save/{NameSpace}_{UserId}.{ConfigHelper.ServerConfig.Saves.Extension}", data);
        }
        public static void SaveUser_Request(string UserId, string NameSpace, string data)
        {
            File.WriteAllText($"Save/{NameSpace}_{UserId}_{DateTime.Now.ToString("s").Replace(":", "-")}.{ConfigHelper.ServerConfig.Saves.Extension}", data);
        }

        public static bool IsUserExist(string UserId, string NameSpace)
        {
            return File.Exists($"Save/{NameSpace}_{UserId}.{ConfigHelper.ServerConfig.Saves.Extension}");
        }

        public static byte[] ReadUserByte(string UserId, string NameSpace)
        {
            return File.ReadAllBytes($"Save/{NameSpace}_{UserId}.{ConfigHelper.ServerConfig.Saves.Extension}");
        }
        public static string ReadUserSTR(string UserId, string NameSpace)
        {
            return File.ReadAllText($"Save/{NameSpace}_{UserId}.{ConfigHelper.ServerConfig.Saves.Extension}");
        }
    }
}

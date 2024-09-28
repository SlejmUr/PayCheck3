using PayCheckServerLib.Helpers;

namespace PayCheckServerLib
{
    public class SaveFileHandler
    {
        static SaveFileHandler()
        {
            if (!Directory.Exists("Save")) { Directory.CreateDirectory("Save"); }
        }

        public static void SaveUser(string UserId, string NameSpace, byte[] data, SaveType saveType)
        {
            try
            {
                File.WriteAllBytes($"Save/{NameSpace}_{UserId}_{saveType}.{ConfigHelper.ServerConfig.Saves.Extension}", data);
            } catch (IOException e) { }
        }
        public static void SaveUser(string UserId, string NameSpace, string data, SaveType saveType)
        {
            File.WriteAllText($"Save/{NameSpace}_{UserId}_{saveType}.{ConfigHelper.ServerConfig.Saves.Extension}", data);
        }
        public static void SaveUser_Request(string UserId, string NameSpace, string data, SaveType saveType)
        {
            File.WriteAllText($"Save/{NameSpace}_{UserId}_{saveType}_{DateTime.Now.ToString("s").Replace(":", "-")}.{ConfigHelper.ServerConfig.Saves.Extension}", data);
        }

        public static bool IsUserExist(string UserId, string NameSpace, SaveType saveType)
        {
            return File.Exists($"Save/{NameSpace}_{UserId}_{saveType}.{ConfigHelper.ServerConfig.Saves.Extension}");
        }

        public static byte[] ReadUserByte(string UserId, string NameSpace, SaveType saveType)
        {
            return File.ReadAllBytes($"Save/{NameSpace}_{UserId}_{saveType}.{ConfigHelper.ServerConfig.Saves.Extension}");
        }
        public static string ReadUserSTR(string UserId, string NameSpace, SaveType saveType)
        {
            var name = $"Save/{NameSpace}_{UserId}_{saveType}.{ConfigHelper.ServerConfig.Saves.Extension}";
            if (File.Exists(name))
                return File.ReadAllText($"Save/{NameSpace}_{UserId}_{saveType}.{ConfigHelper.ServerConfig.Saves.Extension}");
            else
                return string.Empty;
        }

        public enum SaveType
        {
            progressionsavegame,
            PlatformBackendSettingsData,
            statitems,
            currency,
            challenges
        };
    }
}

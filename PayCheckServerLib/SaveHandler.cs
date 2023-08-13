namespace PayCheckServerLib
{
    public class SaveHandler
    {
        public SaveHandler() 
        {
            if (!Directory.Exists("Save")) { Directory.CreateDirectory("Save"); }
        }

        public static void SaveUser(string UserId, byte[] data)
        {
            File.WriteAllBytes($"Save/{UserId}.save", data);
        }
        public static void SaveUser_Request(string UserId, string data)
        {
            File.WriteAllText($"Save/{UserId}_{DateTime.Now.ToString("s").Replace(":", "-")}.save", data);
        }

        public static bool IsUserExist(string UserId)
        {
            return File.Exists($"Save/{UserId}.save");
        }

        public static byte[] ReadUserByte(string UserId)
        {
            return File.ReadAllBytes($"Save/{UserId}.save");
        }
        public static string ReadUserSTR(string UserId)
        {
            return File.ReadAllText($"Save/{UserId}.save");
        }
    }
}

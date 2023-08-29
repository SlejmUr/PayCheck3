using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Helpers
{
    /// <summary>
    /// Controlling user behaviour, useful for friends, login, etc.
    /// </summary>
    public class UserController
    {
        public static void RegisterUser(string PlatformId, TokenHelper.TokenPlatform platform, string UserName)
        {
            var UserId = UserIdHelper.CreateNewID();
            User user = new User()
            { 
                Friends = new(),
                UserData = new() 
                {
                    AvatarUrl = "",
                    DisplayName = UserName,
                    UserId = UserId,
                    PlatformUserIds = new()
                    {
                        {  platform.ToString().ToLower(), PlatformId }
                    }
                }
            };
            SaveUser(user);
            Debugger.PrintInfo("User has been registered!");
        }

        public static bool CheckUser(string PlaformId, TokenHelper.TokenPlatform platform)
        {
            foreach (User item in GetUsers())
            {
                if (item.UserData.PlatformUserIds.TryGetValue(platform.ToString().ToLower(), out string pId))
                {
                    return (pId == PlaformId);
                }
            }

            return true;
        }

        public static void LoginUser(/* args */)
        { 
            //Todo Generate Token.
        
        
        }



        public static void SaveUser(User user)
        {
            if (!Directory.Exists("Users")) { Directory.CreateDirectory("Users"); }
            File.WriteAllText($"Users/{user.UserData.UserId}.json", JsonConvert.SerializeObject(user));
        }

        public static User? GetUser(string UserId)
        {
            if (File.Exists($"Users/{UserId}.json"))
            {
                return JsonConvert.DeserializeObject<User>(File.ReadAllText($"Users/{UserId}.json"));
            }
            return null;        
        }

        public static List<User> GetUsers()
        { 
            List<User> users = new List<User>();
            foreach (var item in Directory.GetFiles("Users"))
            {
                users.Add(JsonConvert.DeserializeObject<User>(File.ReadAllText(item)));
            }
            return users;
        }
    }
}

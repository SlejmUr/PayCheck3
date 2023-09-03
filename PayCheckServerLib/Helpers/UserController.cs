using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Helpers
{
    /// <summary>
    /// Controlling user behaviour, useful for friends, login, etc.
    /// </summary>
    public class UserController
    {
        public static User RegisterUser(string PlatformId, TokenHelper.TokenPlatform platform, string UserName)
        {
            User user = new()
            { 
                Friends = new(),
                UserData = new() 
                {
                    AvatarUrl = "",
                    DisplayName = UserName,
                    UserId = UserIdHelper.CreateNewID(),
                    PlatformUserIds = new()
                    {
                        {  platform.ToString().ToLower(), PlatformId }
                    }
                }
            };
            SaveUser(user);
            Debugger.PrintInfo("User has been registered!");
            return user;
        }

        public static bool CheckUser(string PlaformId, TokenHelper.TokenPlatform platform)
        {
            foreach (User item in GetUsers())
            {
                if (item.UserData.PlatformUserIds.TryGetValue(platform.ToString().ToLower(), out var pId))
                {
                    if (pId != null)
                        return (pId == PlaformId);
                }
            }

            return true;
        }

        public static User? GetUser(string PlaformId, TokenHelper.TokenPlatform platform)
        {
            foreach (User item in GetUsers())
            {
                if (item.UserData.PlatformUserIds.TryGetValue(platform.ToString().ToLower(), out var pId))
                {
                    if (pId != null & pId == PlaformId)
                        return item;
                }
            }

            return null;
        }

        public static (TokenHelper.Token AccessToken, TokenHelper.Token RefleshToken) LoginUser(string PlaformId, TokenHelper.TokenPlatform platform)
        {
            var user = GetUser(PlaformId, platform);
            if (user == null)
            {
                Debugger.PrintWarn("User Is not Registered! Continue generating DefaultUser...","USERCONTROLLER.WARN");
                user = RegisterUser(PlaformId, platform, "DefaultUser");
            }
            var token = TokenHelper.GenerateNewTokenFromUser(user, platform);
            TokenHelper.StoreToken(token); 
            var tokenRef = TokenHelper.GenerateNewTokenFromUser(user, platform, false);
            TokenHelper.StoreToken(tokenRef);
            return (token, tokenRef);
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
            if (!Directory.Exists("Users")) { Directory.CreateDirectory("Users"); }
            foreach (var item in Directory.GetFiles("Users"))
            {
                users.Add(JsonConvert.DeserializeObject<User>(File.ReadAllText(item)));
            }
            return users;
        }
    }
}

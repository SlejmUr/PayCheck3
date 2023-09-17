using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Helpers
{
    /// <summary>
    /// Controlling user behaviour, useful for friends, login, etc.
    /// </summary>
    public class UserController
    {
        /// <summary>
        /// Register a user based on Parameters
        /// </summary>
        /// <param name="PlatformId">SteamId or DeviceId</param>
        /// <param name="platform">Steam or Device</param>
        /// <param name="UserName">Unique username</param>
        /// <returns>The new User</returns>
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
                },
                Status = new()
                { 
                    IsOnWSs = false,
                    availability = "offline",
                    activity = "nil",
                    platform = "nil",
                    lastSeenAt = "2023-09-08T12:00:00Z"
                }
            };
            SaveUser(user);
            Debugger.PrintInfo("User has been registered!");
            return user;
        }

        /// <summary>
        /// Check if User Exist by Parameters
        /// </summary>
        /// <param name="PlaformId">SteamId or DeviceId</param>
        /// <param name="platform">Steam or Device</param>
        /// <returns>True or False</returns>
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

        /// <summary>
        /// Get User by Parameters
        /// </summary>
        /// <param name="PlaformId">SteamId or DeviceId</param>
        /// <param name="platform">Steam or Device</param>
        /// <returns>User object or null</returns>
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

        /// <summary>
        /// Login User with Parameters, If not exist it will create one!
        /// </summary>
        /// <param name="PlaformId">SteamId or DeviceId</param>
        /// <param name="platform">Steam or Device</param>
        /// <returns>AccessToken and RefleshToken</returns>
        public static (TokenHelper.Token AccessToken, TokenHelper.Token RefleshToken) LoginUser(string PlaformId, TokenHelper.TokenPlatform platform)
        {
            var user = GetUser(PlaformId, platform);
            if (user == null)
            {
                Debugger.PrintWarn("User Is not Registered! Continue generating DefaultUser...", "USERCONTROLLER.WARN");
                user = RegisterUser(PlaformId, platform, "DefaultUser");
            }
            var token = TokenHelper.GenerateNewTokenFromUser(user, platform);
            TokenHelper.StoreToken(token);
            var tokenRef = TokenHelper.GenerateNewTokenFromUser(user, platform, false);
            TokenHelper.StoreToken(tokenRef);
            return (token, tokenRef);
        }


        /// <summary>
        /// Save the user to disk
        /// </summary>
        /// <param name="user">User Object</param>
        public static void SaveUser(User user)
        {
            if (!Directory.Exists("Users")) { Directory.CreateDirectory("Users"); }
            File.WriteAllText($"Users/{user.UserData.UserId}.json", JsonConvert.SerializeObject(user));
        }

        /// <summary>
        /// Get user from disk
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns>User object if Exist or null</returns>
        public static User? GetUser(string UserId)
        {
            if (File.Exists($"Users/{UserId}.json"))
            {
                return JsonConvert.DeserializeObject<User>(File.ReadAllText($"Users/{UserId}.json"));
            }
            return null;
        }

        /// <summary>
        /// Get All Users from the disk
        /// </summary>
        /// <returns>List of Users</returns>
        public static List<User> GetUsers()
        {
            List<User> users = new();
            if (!Directory.Exists("Users")) { Directory.CreateDirectory("Users"); }
            foreach (var item in Directory.GetFiles("Users"))
            {
                users.Add(JsonConvert.DeserializeObject<User>(File.ReadAllText(item))!);
            }
            return users;
        }
    }
}

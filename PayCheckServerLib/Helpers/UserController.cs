using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayCheckServerLib.Jsons;
using static PayCheckServerLib.Jsons.User;

namespace PayCheckServerLib.Helpers
{
    /// <summary>
    /// Controlling user behaviour, useful for friends, login, etc.
    /// </summary>
    public class UserController
    {
		private static string GetDataJsonPathForUserId(string userId)
		{
			return String.Format("./UserData/{0}.json", userId);
		}
		private static string GetDefaultUserRoleId()
		{
			return "2251438839e948d783ec0e5281daf05b"; // Identical on every AccelByte instance for some reason.
		}
		public class PayCheck3UserData
		{

			public class PayCheck3UserRoleAssignmentData
			{
				[JsonProperty("RoleId")]
				public string RoleId { get; set; }
				[JsonProperty("Namespace")]
				public string Namespace { get; set; }
			}

			[JsonProperty("UserData")]
			public UserBulk.UserBulkData UserData { get; set; }
			[JsonProperty("Friends")]
			public List<FriendsPlatfrom.FriendsPlatfromData> Friends { get; set; }
			[JsonProperty("Namespace")]
			public string Namespace { get; set; }
			[JsonProperty("Roles")]
			public List<PayCheck3UserRoleAssignmentData> Roles { get; set; }
			[JsonProperty("Status")]
			public FStatus Status { get; set; }
		}



        /// <summary>
        /// Register a user based on Parameters
        /// </summary>
        /// <param name="PlatformId">SteamId or DeviceId</param>
        /// <param name="platform">Steam or Device</param>
        /// <param name="UserName">Unique username</param>
        /// <returns>The new User</returns>
        public static PayCheck3UserData RegisterUser(string PlatformId, TokenHelper.TokenPlatform platform, string UserName, string NameSpace)
        {
			PayCheck3UserData user = new()
			{
				Roles = [
					new() {
						Namespace = NameSpace,
						RoleId = GetDefaultUserRoleId()
					}
				],
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
                    availability = "offline",
                    activity = "nil",
                    platform = "nil",
                    lastSeenAt = "2023-09-08T12:00:00Z"
                },
				Namespace = NameSpace
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
        public static bool CheckUser(string PlaformId, TokenHelper.TokenPlatform platform, string NameSpace)
        {
            foreach (PayCheck3UserData item in GetUsers())
            {
                if (item.Namespace != NameSpace)
                    continue;

                if (item.UserData.PlatformUserIds.TryGetValue(platform.ToString().ToLower(), out var pId))
                {
                    if (pId != null)
                        return (pId == PlaformId);
                }
            }

            return false;
        }

        /// <summary>
        /// Get User by Parameters
        /// </summary>
        /// <param name="PlaformId">SteamId or DeviceId</param>
        /// <param name="platform">Steam or Device</param>
        /// <returns>User object or null</returns>
        public static PayCheck3UserData? GetUser(string PlaformId, TokenHelper.TokenPlatform platform, string NameSpace)
        {
            foreach (PayCheck3UserData item in GetUsers())
            {
                if (item.Namespace != NameSpace)
                    continue;

                if (item.UserData.PlatformUserIds.TryGetValue(platform.ToString().ToLower(), out var pId))
                {
                    if (pId != null & pId == PlaformId)
                        return item;
                }
            }

            return null;
        }

        public static PayCheck3UserData? GetUser(string UserId, string NameSpace)
        {
			if (!UserIdHelper.IsValidUserId(UserId))
				return null;

            foreach (PayCheck3UserData item in GetUsers())
            {
                if (item.Namespace != NameSpace)
                    continue;
                if (item.UserData.UserId == UserId)
                    return item;
            }

            return null;
        }




        /// <summary>
        /// Login User with Parameters, If not exist it will create one!
        /// </summary>
        /// <param name="PlaformId">SteamId or DeviceId</param>
        /// <param name="platform">Steam or Device</param>
        /// <returns>AccessToken and RefleshToken</returns>
        public static (TokenHelper.Token AccessToken, TokenHelper.Token RefleshToken) LoginUser(string PlaformId, TokenHelper.TokenPlatform platform, string NameSpace)
        {
            var user = GetUser(PlaformId, platform, NameSpace);
            if (user == null)
            {
                Debugger.PrintWarn("User Is not Registered! Continue generating DefaultUser...", "USERCONTROLLER.WARN");
                user = RegisterUser(PlaformId, platform, "DefaultUser", NameSpace);
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
        public static void SaveUser(PayCheck3UserData user)
        {
            if (!Directory.Exists("UserData")) { Directory.CreateDirectory("UserData"); }

			if (!UserIdHelper.IsValidUserId(user.UserData.UserId))
				return;

			if (!Directory.Exists(String.Format("./UserData/{0}", user.UserData.UserId)))
				Directory.CreateDirectory(String.Format("./UserData/{0}", user.UserData.UserId));

			FileReadWriteHelper.WriteAllText(GetDataJsonPathForUserId(user.UserData.UserId), JsonConvert.SerializeObject(user));
        }

        /// <summary>
        /// Get user from disk
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns>User object if Exist or null</returns>
        public static PayCheck3UserData? GetUser(string UserId)
        {
			if (!UserIdHelper.IsValidUserId(UserId))
				return null;

			string path = GetDataJsonPathForUserId(UserId);


			if (FileReadWriteHelper.Exists(path))
            {
                return JsonConvert.DeserializeObject<PayCheck3UserData>(FileReadWriteHelper.ReadAllText(path));
            }
            return null;
        }

        /// <summary>
        /// Get All Users from the disk
        /// </summary>
        /// <returns>List of Users</returns>
        public static List<PayCheck3UserData> GetUsers()
        {
            List<PayCheck3UserData> users = new();
            if (!Directory.Exists("UserData")) { Directory.CreateDirectory("UserData"); }
            foreach (var item in Directory.GetFiles("UserData"))
            {

				var userData = JsonConvert.DeserializeObject(FileReadWriteHelper.ReadAllText(item));

				if (userData != null) { // validation to ensure that any json file checked for user data is actual user data.
					if (userData.GetType() == typeof(JObject)) {
						var userDataObj = ((JObject)userData).ToObject<PayCheck3UserData>();

						if (userDataObj != null)
						{
							users.Add(userDataObj);
						}
					}
				}
            }
            return users;
        }
    }
}

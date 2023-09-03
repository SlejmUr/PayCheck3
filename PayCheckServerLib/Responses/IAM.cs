using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses
{
    public class IAM
    {
        [HTTP("POST", "/iam/v3/oauth/platforms/steam/token")]
        public static bool SteamToken(HttpRequest request, PC3Server.PC3Session session)
        {
            var splitted = request.Body.Split("&");
            Dictionary<string, string> bodyTokens = new();
            foreach (var item in splitted)
            {
                var it = item.Split("=");
                bodyTokens.Add(it[0], it[1]);
            }

            var platform_token = bodyTokens["platform_token"];
            var steamId = UserIdHelper.GetSteamIdFromAUTH(platform_token);
            Debugger.PrintInfo(steamId);

            var (access_token, refresh_token) = UserController.LoginUser(steamId, TokenHelper.TokenPlatform.Steam);

            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            response.SetCookie("refresh_token", refresh_token.ToBase64());
            response.SetCookie("access_token", access_token.ToBase64());
            LoginToken LoginToken = new()
            {
                AccessToken = access_token.ToBase64(),
                Scope = "account commerce social publishing analytics",
                Bans = new() { },
                DisplayName = access_token.Name,
                ExpiresIn = 360000,
                IsComply = true,
                Jflgs = 1,
                Namespace = "pd3beta",
                NamespaceRoles = new()
                {
                    new NamespaceRole()
                    {
                        Namespace = "*",
                        RoleId = "2251438839e948d783ec0e5281daf05"
                    }

                },
                Permissions = new() { },
                PlatformId = "steam",
                PlatformUserId = access_token.PlatformId,
                RefreshExpiresIn = 86400,
                RefreshToken = refresh_token.ToBase64(),
                Roles = new() { "2251438839e948d783ec0e5281daf05" },
                TokenType = "Bearer",
                UserId = access_token.UserId
            };
            response.SetBody(JsonConvert.SerializeObject(LoginToken));
            session.SendResponse(response.GetResponse());
            Debugger.PrintDebug("Sent Response!");
            return true;
        }

        [HTTP("POST", "/iam/v3/oauth/platforms/device/token")]
        public static bool DeviceToken(HttpRequest request, PC3Server.PC3Session session)
        {
            var deviceid = request.Body.Split('=')[1];
            Debugger.PrintDebug(deviceid);
            var (access_token, refresh_token) = UserController.LoginUser(deviceid, TokenHelper.TokenPlatform.Device);

            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            response.SetCookie("refresh_token", refresh_token.ToBase64());
            response.SetCookie("access_token", access_token.ToBase64());
            LoginToken token = new()
            {
                AccessToken = access_token.ToBase64(),
                Scope = "account commerce social publishing analytics",
                Bans = new() { },
                DisplayName = access_token.Name,
                ExpiresIn = 360000,
                IsComply = true,
                Jflgs = 4,
                Namespace = "pd3beta",
                NamespaceRoles = new()
                {
                    new NamespaceRole()
                    {
                        Namespace = "*",
                        RoleId = "2251438839e948d783ec0e5281daf05"
                    }
                },
                Permissions = new() { },
                PlatformId = "device",
                PlatformUserId = deviceid,
                RefreshExpiresIn = 86400,
                RefreshToken = refresh_token.ToBase64(),
                Roles = new() { "2251438839e948d783ec0e5281daf05" },
                TokenType = "Bearer",
                UserId = access_token.UserId
            };
            response.SetBody(JsonConvert.SerializeObject(token));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/iam/v3/public/users/me")]
        public static bool UsersMe(HttpRequest request, PC3Server.PC3Session session)
        {
            var auth = session.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            Me me = new()
            {
                AvatarUrl = new("https://nebula.starbreeze.com/static/media/903924fd9d443f43e7b121d085062fbdd2064f25_full.e7d46514.jpg"),
                DeletionStatus = false,
                Bans = new(),
                Country = "HU",
                DisplayName = token.Name,
                EmailAddress = $"{token.Name}@pd3beta_emu.com",
                EmailVerified = true,
                Enabled = true,
                Namespace = "pd3beta",
                OldEmailAddress = $"{token.Name}@pd3beta_emu.com",
                PhoneVerified = true,
                Permissions = new(),
                UserId = token.UserId,
                UserName = token.Name,
                NamespaceRoles = new()
                {
                    new NamespaceRole()
                    {
                        Namespace = "*",
                        RoleId = "2251438839e948d783ec0e5281daf05"
                    }
                },
                Roles = new() { "2251438839e948d783ec0e5281daf05" }
            };
            response.SetBody(JsonConvert.SerializeObject(me));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("POST", "/iam/v3/public/namespaces/pd3beta/users/bulk/basic")]
        public static bool BulkBasic(HttpRequest request, PC3Server.PC3Session session)
        {
            var req = JsonConvert.DeserializeObject<BulkReq>(request.Body);

            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            UserBulk bulk = new()
            {
                Data = new()
                {
                }
            };

            foreach (var item in req.UserIds)
            {
                //  using UserController here to populate BulkData
                var user = UserController.GetUser(item);
                if (user == null)
                {
                    Debugger.PrintWarn($"User ({item}) not exist in UserController!");
                }
                else
                {
                    bulk.Data.Add(user.UserData);
                }
            }

            response.SetBody(JsonConvert.SerializeObject(bulk));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/iam/v3/public/namespaces/pd3beta/users?query={uname}&by=displayName&limit=100&offset=0")]
        public static bool UsersQuery(HttpRequest request, PC3Server.PC3Session session)
        {
            //Idk what is this but works
            var username = session.HttpParam["uname"];
            username = username.Split("&")[0].Split("=")[1];
            //magix shit end
            Debugger.PrintDebug("UserName to search: " + username);
            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            DataPaging<FriendsSearch> dataSearch = new()
            { 
                Data = new(),
                Paging = new()
                { 
                    First = "",
                    Last = "",
                    Next = "",
                    Previous = ""
                }
            };

            foreach (var item in UserController.GetUsers())
            {
                if (item.UserData.DisplayName.Contains(username))
                {
                    Debugger.PrintDebug("User found: " + item.UserData.DisplayName);

                    dataSearch.Data.Add(new()
                    {
                        createdAt = DateTime.UtcNow.ToString("o"),
                        DisplayName = item.UserData.DisplayName,
                        Namespace = "pd3beta",
                        UserId = item.UserData.UserId,
                        UserName = item.UserData.DisplayName
                    });
                }
            }

            response.SetBody(JsonConvert.SerializeObject(dataSearch));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}

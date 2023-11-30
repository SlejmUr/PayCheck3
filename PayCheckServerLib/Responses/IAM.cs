using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.Basic;
using System.Web;
using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;

namespace PayCheckServerLib.Responses
{
    public class IAM
    {
        [HTTP("POST", "/iam/v3/oauth/platforms/steam/token")]
        public static bool SteamToken(HttpRequest request, ServerStruct serverStruct)
        {
            ResponseCreator response = new();
            if (ConfigHelper.ServerConfig.InDevFeatures.UsePWInsteadSteamToken)
            {

                /*
                return status code 401 with
                 {
                        "clientId": "uuidv4",
                        "error": "platform_not_linked",
                        "linkingToken": "uuidv4",
                        "platformId": "steam"
                    }
                 to get game to allow email + password auth
                 */
                response.SetBody(JsonConvert.SerializeObject(new IAM_SteamError()));
                serverStruct.Response = response.GetResponse();
                serverStruct.SendResponse();
                return true;
            }
            Debugger.PrintDebug(request.Body);
            var splitted = request.Body.Split("&");
            Dictionary<string, string> bodyTokens = new();
            foreach (var item in splitted)
            {
                var it = item.Split("=");
                bodyTokens.Add(it[0], it[1]);
            }

            var platform_token = bodyTokens["platform_token"];
            if (platform_token.Contains("pd3"))
                platform_token = platform_token.Replace("pd3%3A","");

            var sai = UserIdHelper.getsai(platform_token);
            Debugger.PrintInfo(sai);
            if (!(sai == "1272080" || sai == "2478210"))
            {
                Debugger.PrintError("Auth is incorrect!");
                return true;
            }
            var steamId = UserIdHelper.GetSteamIDFromAUTH(platform_token);
            Debugger.PrintInfo("User with SteamID try to log in: " + steamId);

            var (access_token, refresh_token) = UserController.LoginUser(steamId, TokenHelper.TokenPlatform.Steam, serverStruct.Headers["namespace"]);

            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            /*
            response.SetHeader("cache-control", "no-cache, no-store, max-age=0, must-revalidate");
            response.SetHeader("expires", "Fri, 01 Jan 1990 00:00:00 GMT");
            response.SetHeader("pragma",    "no-cache");*/
            response.SetHeader("Set-Cookie", "refresh_token=" + refresh_token.ToBase64() + "; Path=/; HttpOnly; Secure; SameSite=None");
            response.SetHeader("Set-Cookie", "access_token=" + access_token.ToBase64() + "; Path=/; HttpOnly; Secure; SameSite=None");
            LoginToken LoginToken = new()
            {
                AccessToken = access_token.ToBase64(),
                Scope = "account commerce social publishing analytics",
                Bans = new() { },
                DisplayName = access_token.Name,
                ExpiresIn = 360000,
                IsComply = true,
                Jflgs = 1,
                Namespace = serverStruct.Headers["namespace"],
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
                RefreshExpiresIn = 8400000,
                RefreshToken = refresh_token.ToBase64(),
                Roles = new() { "2251438839e948d783ec0e5281daf05" },
                TokenType = "Bearer",
                UserId = access_token.UserId
            };
            response.SetBody(JsonConvert.SerializeObject(LoginToken));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("POST", "/iam/v3/oauth/platforms/device/token")]
        public static bool DeviceToken(HttpRequest request, ServerStruct serverStruct)
        {
            var deviceid = request.Body.Split('=')[1];
            Debugger.PrintDebug(deviceid);
            var (access_token, refresh_token) = UserController.LoginUser(deviceid, TokenHelper.TokenPlatform.Device, serverStruct.Headers["namespace"]);

            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            response.SetHeader("Set-Cookie", "refresh_token=" + refresh_token.ToBase64() + "; Path=/; HttpOnly; Secure; SameSite=None");
            response.SetHeader("Set-Cookie", "access_token=" + access_token.ToBase64() + "; Path=/; HttpOnly; Secure; SameSite=None");
            LoginToken token = new()
            {
                AccessToken = access_token.ToBase64(),
                Scope = "account commerce social publishing analytics",
                Bans = new() { },
                DisplayName = access_token.Name,
                ExpiresIn = 360000,
                IsComply = true,
                Jflgs = 4,
                Namespace = serverStruct.Headers["namespace"],
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
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }


        [HTTP("POST", "/iam/v3/oauth/platforms/live/token")]
        public static bool LiveToken(HttpRequest request, ServerStruct serverStruct)
        {
            var splitted = request.Body.Split("&");
            Dictionary<string, string> bodyTokens = new();
            foreach (var item in splitted)
            {
                var it = item.Split("=");
                bodyTokens.Add(it[0], it[1]);
            }

            var platform_token = bodyTokens["platform_token"];
            Debugger.PrintDebug(platform_token);
            platform_token = platform_token.Replace("XBL3.0%20x%3D", "");
            platform_token = platform_token.Split(";")[0];
            var (access_token, refresh_token) = UserController.LoginUser(platform_token, TokenHelper.TokenPlatform.Live, serverStruct.Headers["namespace"]);

            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            response.SetHeader("Set-Cookie", "refresh_token=" + refresh_token.ToBase64() + "; Path=/; HttpOnly; Secure; SameSite=None");
            response.SetHeader("Set-Cookie", "access_token=" + access_token.ToBase64() + "; Path=/; HttpOnly; Secure; SameSite=None");
            LoginToken token = new()
            {
                AccessToken = access_token.ToBase64(),
                Scope = "account commerce social publishing analytics",
                Bans = new() { },
                DisplayName = access_token.Name,
                ExpiresIn = 360000,
                IsComply = true,
                Jflgs = 4,
                Namespace = serverStruct.Headers["namespace"],
                NamespaceRoles = new()
                {
                    new NamespaceRole()
                    {
                        Namespace = "*",
                        RoleId = "2251438839e948d783ec0e5281daf05"
                    }
                },
                Permissions = new() { },
                PlatformId = "live",
                PlatformUserId = "",    //todo, how the fuck you got the userid from jwt
                RefreshExpiresIn = 86400,
                RefreshToken = refresh_token.ToBase64(),
                Roles = new() { "2251438839e948d783ec0e5281daf05" },
                TokenType = "Bearer",
                UserId = access_token.UserId
            };
            response.SetBody(JsonConvert.SerializeObject(token));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        // Logging in with email + password, also links steam to nebula account on official servers
        [HTTP("POST", "/iam/v3/authenticateWithLink")]
        public static bool AuthenticateWithLink(HttpRequest request, ServerStruct serverStruct)
        {
            var param = HttpUtility.ParseQueryString(request.Body);
            // either username or email entered
            var username = param["username"];
            // plain text
            var password = param["password"];
            var linking_token = param["linkingToken"];
            var client_id = param["client_id"];

            // request does not have a device id, client_id will do for now
            var (access_token, refresh_token) = UserController.LoginUser(client_id!, TokenHelper.TokenPlatform.Device, serverStruct.Headers["namespace"]);

            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            response.SetHeader("Set-Cookie", "refresh_token=" + refresh_token.ToBase64() + "; Path=/; HttpOnly; Secure; SameSite=None");
            response.SetHeader("Set-Cookie", "access_token=" + access_token.ToBase64() + "; Path=/; HttpOnly; Secure; SameSite=None");
            LoginToken token = new()
            {
                AccessToken = access_token.ToBase64(),
                Scope = "account commerce social publishing analytics",
                Bans = new() { },
                DisplayName = access_token.Name,
                ExpiresIn = 360000,
                IsComply = true,
                // Jflgs is 1 for this request
                Jflgs = 1,
                Namespace = "pd3",
                NamespaceRoles = new()
                {
                    new NamespaceRole()
                    {
                        Namespace = "*",
                        RoleId = "2251438839e948d783ec0e5281daf05"
                    }
                },
                Permissions = new() { },
                PlatformId = "",
                PlatformUserId = "",
                RefreshExpiresIn = 86400,
                RefreshToken = refresh_token.ToBase64(),
                Roles = new() { "2251438839e948d783ec0e5281daf05" },
                TokenType = "Bearer",
                UserId = access_token.UserId
            };
            response.SetBody(JsonConvert.SerializeObject(token));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }


        [HTTP("GET", "/iam/v3/public/users/me")]
        public static bool UsersMe(HttpRequest _, ServerStruct serverStruct)
        {
            var auth = serverStruct.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            Me me = new()
            {
                AvatarUrl = new("https://nebula.starbreeze.com/static/media/903924fd9d443f43e7b121d085062fbdd2064f25_full.e7d46514.jpg"),
                DeletionStatus = false,
                Bans = new(),
                Country = "HU",
                DisplayName = token.Name,
                EmailAddress = $"{token.Name}@pd3_emu.com",
                EmailVerified = true,
                Enabled = true,
                Namespace = serverStruct.Headers["namespace"],
                OldEmailAddress = $"{token.Name}@pd3_emu.com",
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
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("POST", "/iam/v3/public/namespaces/{namespace}/users/bulk/basic")]
        public static bool BulkBasic(HttpRequest request, ServerStruct serverStruct)
        {
            var req = JsonConvert.DeserializeObject<BulkReq>(request.Body) ?? throw new Exception("BulkBasic is null!");
            ResponseCreator response = new();
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
                    if (user.Namespace == serverStruct.Headers["namespace"])
                        bulk.Data.Add(user.UserData);
                }
            }

            response.SetBody(JsonConvert.SerializeObject(bulk));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("POST", "/iam/v3/public/namespaces/{namespace}/users/bulk/platforms")]
        public static bool BulkPlatforms(HttpRequest request, ServerStruct serverStruct)
        {
            var req = JsonConvert.DeserializeObject<BulkReq>(request.Body) ?? throw new Exception("BulkPlatforms is null!");
            ResponseCreator response = new();
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
                    if (user.Namespace == serverStruct.Headers["namespace"])
                        bulk.Data.Add(user.UserData);
                }
            }

            response.SetBody(JsonConvert.SerializeObject(bulk));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("GET", "/iam/v3/public/namespaces/{namespace}/users?query={uname}&by=displayName&limit=100&offset=0")]
        public static bool UsersQuery(HttpRequest _, ServerStruct serverStruct)
        {
            //Idk what is this but works
            var username = serverStruct.Parameters["uname"];
            username = username.Split("&")[0].Split("=")[1];
            //magix shit end
            Debugger.PrintDebug("UserName to search: " + username);
            ResponseCreator response = new();
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
                if (item.Namespace != serverStruct.Parameters["namespace"])
                    continue;

                if (item.UserData.DisplayName.Contains(username))
                {
                    Debugger.PrintDebug("User found: " + item.UserData.DisplayName);

                    dataSearch.Data.Add(new()
                    {
                        createdAt = DateTime.UtcNow.ToString("o"),
                        DisplayName = item.UserData.DisplayName,
                        Namespace = item.Namespace,
                        UserId = item.UserData.UserId,
                        UserName = item.UserData.DisplayName
                    });
                }
            }

            response.SetBody(JsonConvert.SerializeObject(dataSearch));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("POST", "/iam/v3/public/namespaces/{namespace}/platforms/steam/users?rawPUID=true")]
        public static bool GetSteamUsersWithPID(HttpRequest request, ServerStruct serverStruct)
        {
            var req = JsonConvert.DeserializeObject<SteamUsersReq>(request.Body);
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");

            SteamUsers steamUsers = new()
            {
                userIdPlatforms = new()
            };


            foreach (var id in req.platformUserIds)
            {
                //can replace the headers to serverStruct.Parameters["namespace"] and will works too
                if (UserController.CheckUser(id, TokenHelper.TokenPlatform.Steam, serverStruct.Headers["namespace"]))
                {
                    var user = UserController.GetUser(id, TokenHelper.TokenPlatform.Steam, serverStruct.Headers["namespace"]);
                    steamUsers.userIdPlatforms.Add(new()
                    {
                        platformId = "steam",
                        platformUserId = id,
                        userId = user.UserData.UserId

                    });

                }
            }

            response.SetBody(JsonConvert.SerializeObject(steamUsers));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("POST", "/iam/v3/oauth/token")]
        public static bool OauthToken(HttpRequest request, ServerStruct serverStruct)
        {
            var grant_type = request.Body.Split('=')[1];
            Debugger.PrintDebug(grant_type);
            //grant_type=client_credentials
            //grant_type=password&username=test&password=testj&device_id=ID

            ResponseCreator response = new(404);
            /*
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            oathToken token = new()
            {
                AccessToken = "randomtoken",
                Scope = "account commerce social publishing analytics",
                Bans = new() { },
                DisplayName = "",
                ExpiresIn = 3600,
                IsComply = true,
                Jflgs = 0,
                Namespace = serverStruct.Headers["namespace"],
                NamespaceRoles = null,
                Permissions = new() { },
                PlatformId = "",
                PlatformUserId = "",
                Roles = null,
                TokenType = "Bearer",
                UserId = ""
            };

            var ds_perms = JsonConvert.DeserializeObject<List<oathToken.Permission>>(File.ReadAllText("Files/DS_Permissions.json"));
            token.Permissions = ds_perms;
            response.SetBody(JsonConvert.SerializeObject(token));
            */
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }
    }
}

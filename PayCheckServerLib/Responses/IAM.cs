using NetCoreServer;
using Newtonsoft.Json;
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
            var access_token = TokenHelper.GenerateFromSteamToken(platform_token);
            TokenHelper.StoreToken(access_token);
            var refresh_token = TokenHelper.GenerateFromSteamToken(platform_token,"DefaultUser", false);
            refresh_token.UserId = access_token.UserId;
            TokenHelper.StoreToken(refresh_token);


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
                DisplayName = "Yeet",
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
            //todo deviceid is in cookie!
            var cookie = session.Headers["cookie"];

            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            response.SetCookie("refresh_token", "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            response.SetCookie("access_token", "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB");
            LoginToken token = new()
            {
                AccessToken = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB",
                Scope = "account commerce social publishing analytics",
                Bans = new() { },
                DisplayName = "Yeet",
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
                PlatformUserId = "c704fc89a13c956e58715e102d08ee6e",
                RefreshExpiresIn = 86400,
                RefreshToken = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                Roles = new() { "2251438839e948d783ec0e5281daf05" },
                TokenType = "Bearer",
                UserId = "29475976933497845197035744456968"
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
            Bulk bulk = new()
            { 
                Data = new()
                { 
                    /*
                    new()
                    { 
                        AvatarUrl = "",
                        DisplayName = "Yeet",
                        UserId = "29475976933497845197035744456968",
                        PlatformUserIds = new()
                        {
                            { "steam", "76561199227922074" }                        
                        }
                    }*/
                }
            };

            foreach (var item in req.UserIds)
            {
                if (TokenHelper.IsUserIdExist(item))
                {
                    var token = TokenHelper.ReadTokenFile(item);
                    Bulk.CData data = new()
                    { 
                        AvatarUrl = "",
                        DisplayName = token.Name,
                        UserId = item,
                        PlatformUserIds = new()
                        { 
                        
                        }
                    };
                    switch (token.PlatformType)
                    {
                        case TokenHelper.TokenPlatform.Steam:
                            data.PlatformUserIds.Add("steam", token.PlatformId);
                            break;
                        case TokenHelper.TokenPlatform.Device:
                        default:
                            data.PlatformUserIds.Add("device", token.PlatformId);
                            break;
                    }
                    bulk.Data.Add(data);
                }
                else
                {
                    Debugger.PrintWarn($"UserId ({item}) not exist in Tokens!");
                }
            }

            response.SetBody(JsonConvert.SerializeObject(bulk));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}

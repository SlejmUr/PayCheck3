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

            //TokenHelper.


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
                PlatformUserId = "76561199227922074",
                RefreshExpiresIn = 86400,
                RefreshToken = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                Roles = new() { "2251438839e948d783ec0e5281daf05" },
                TokenType = "Bearer",
                UserId = "29475976933497845197035744456968"
            };
            response.SetBody(JsonConvert.SerializeObject(token));
            session.SendResponse(response.GetResponse());
            Debugger.PrintDebug("Sent Response!");
            return true;
        }

        [HTTP("POST", "/iam/v3/oauth/platforms/device/token")]
        public static bool DeviceToken(HttpRequest request, PC3Server.PC3Session session)
        {
            var splitted = request.Body.Split("&");
            Dictionary<string, string> bodyTokens = new();
            foreach (var item in splitted)
            {
                var it = item.Split("=");
                bodyTokens.Add(it[0], it[1]);
            }



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
            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            Me me = new()
            { 
                AvatarUrl = new("https://nebula.starbreeze.com/static/media/903924fd9d443f43e7b121d085062fbdd2064f25_full.e7d46514.jpg"),
                DeletionStatus = false,
                Bans = new(),
                Country = "HU",
                DisplayName = "Yeet",
                EmailAddress = "yeet@yeet.com",
                EmailVerified = true,
                Enabled = true,
                Namespace = "pd3beta",
                OldEmailAddress = "yeet@yeet.com",
                PhoneVerified = true,
                Permissions = new(),
                UserId = "29475976933497845197035744456968",
                UserName = "Yeet",
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
            Console.WriteLine(request.Body);
            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            response.SetHeader("Connection", "keep-alive");
            Bulk bulk = new()
            { 
                Data = new()
                { 
                    new()
                    { 
                        AvatarUrl = "",
                        DisplayName = "Yeet",
                        UserId = "29475976933497845197035744456968",
                        PlatformUserIds = new()
                        {
                            { "steam", "76561199227922074" }                        
                        }
                    
                    }
                }
            };
            response.SetBody(JsonConvert.SerializeObject(bulk));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}

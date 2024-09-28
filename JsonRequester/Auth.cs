using RestSharp;
using SaveGetter;
using Steamworks;

namespace JsonRequester
{
    public static class Auth
    {
        public static LoginToken LoginToken;
        public static void AddDefault(this RestClient client)
        {
            client.AddDefaultHeader("Authorization", "Bearer " + LoginToken.AccessToken);
            client.AddDefaultHeader("Cookie", $"access_token={LoginToken.AccessToken};refresh_token={LoginToken.RefreshToken}");
            client.AddDefaultHeader("Namespace", LoginToken.Namespace);
            client.AddDefaultHeader("Accept-Encoding", "deflate, gzip");
            client.AddDefaultHeader("Content-Type", "application/json");
            client.AddDefaultHeader("Accept", "application/json");
            client.AddDefaultHeader("Game-Client-Version", "1.0.0.0");
            client.AddDefaultHeader("AccelByte-SDK-Version", "24.7.2");
            client.AddDefaultHeader("AccelByte-OSS-Version", "0.12.1-2");
            client.AddDefaultHeader("User-Agent", "PAYDAY3/++UE4+Release-4.27-CL-0 Windows/10.0.19045.1.256.64bit");
        }

        public static void Init()
        {

            if (!SteamAPI.Init())
            {
                Console.WriteLine("Steam not running, make sure it is running in the background!");
                Environment.Exit(0);
            }
            byte[] buffer = new byte[1024];
            _ = SteamUser.GetAuthSessionTicket(buffer, 1024, out uint tikcet);
            var Token = BitConverter.ToString(buffer[..(int)tikcet]).Replace("-", string.Empty);
            LoginToken = DoAuth(Token);
        }


        public static LoginToken? DoAuth(string Token)
        {
            var client = new RestClient(Consts.URL + "/iam/v3/oauth/platforms/steam/token");
            client.AddDefaultHeader("Authorization", "Basic MGIzYmZkZjVhMjVmNDUyZmJkMzNhMzYxMzNhMmRlYWI6");
            client.AddDefaultHeader("Namespace", "pd3");
            client.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded");
            var request = new RestRequest();
            request.AddBody($"platform_token={Token}");
            return Rest.Post<LoginToken>(client, request);
        }

    }
}

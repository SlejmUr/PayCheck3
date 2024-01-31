using Newtonsoft.Json;
using Steamworks;

namespace SaveGetter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Getting your save!");

            if (!SteamAPI.Init())
            {
                Console.WriteLine("Steam not running, make sure it is running in the background!");
                Environment.Exit(0);
            }
            byte[] buffer = new byte[1024];
            _ = SteamUser.GetAuthSessionTicket(buffer, 1024, out uint tikcet);
            var Token = BitConverter.ToString(buffer[..(int)tikcet]).Replace("-", string.Empty);
            //Console.WriteLine(Token);
            var token = Auth(Token);
            GetSave(token);
            Exit();
        }

        static LoginToken Auth(string Token)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://nebula.starbreeze.com");

            var formvalues = new Dictionary<string, string>
            {
                { "platform_token", Token }
            };
            var content = new FormUrlEncodedContent(formvalues);
            //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Add("Authorization", "Basic MGIzYmZkZjVhMjVmNDUyZmJkMzNhMzYxMzNhMmRlYWI6");
            client.DefaultRequestHeaders.Add("Namespace", "pd3");
            var resp = client.PostAsync("/iam/v3/oauth/platforms/steam/token", content).Result;
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("SUCCESS! Got your Login Tokens");
                var json = JsonConvert.DeserializeObject<LoginToken>(resp.Content.ReadAsStringAsync().Result);
                //Console.WriteLine(JsonConvert.SerializeObject(json));
                return json;
            }
            else
            {
                Console.WriteLine("Error!");
                Console.WriteLine(resp.StatusCode);
                Console.WriteLine(resp.Content.ReadAsStringAsync().Result);
                Exit();
                return null;
            }
        }

        static void GetSave(LoginToken token)
        {
            string save = "cloudsave/v1/namespaces/__namespace__/users/__userId__/records/progressionsavegame";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://nebula.starbreeze.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);
            client.DefaultRequestHeaders.Add("Cookie", $"access_token={token.AccessToken};refresh_token={token.RefreshToken}");
            client.DefaultRequestHeaders.Add("Namespace", token.Namespace);

            var result = client.GetAsync(save.Replace("__namespace__", token.Namespace).Replace("__userId__", token.UserId)).Result;

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("SUCCESS! Your save saved!");
                File.WriteAllText($"{token.UserId}.save.txt", result.Content.ReadAsStringAsync().Result);
            }
            else
            {
                Console.WriteLine("Error!");
                Exit();
            }
        }

        static void Exit()
        {
            Console.WriteLine("Quitting..");
            SteamAPI.Shutdown();
            Environment.Exit(0);
        }
    }
}

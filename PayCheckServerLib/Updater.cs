using Newtonsoft.Json;
using System.Security.Cryptography;

namespace PayCheckServerLib
{
    public class Updater
    {
        static readonly string FilesUrl = @"https://github.com/MoolahModding/PayCheck3Files/raw/main/";

        public static async void CheckForJsonUpdates()
        {
            Dictionary<string, string> LocalFiles = new();
            foreach (var file in Directory.GetFiles("./Files"))
            {
                string hash = BitConverter.ToString(SHA256.HashData(File.ReadAllBytes(file))).Replace("-", "").ToLower();
                LocalFiles.Add(file.Replace("./Files\\", ""), hash);
            }

            Dictionary<string, string> Files = new();
            if (FilesUrl.StartsWith("http"))
            {
                try
                {
                    HttpClient client = new();
                    var FilesData = await client.GetStringAsync(FilesUrl);
                    Files = JsonConvert.DeserializeObject<Dictionary<string, string>>(FilesData)!;
                }
                catch
                {
                    Debugger.PrintWarn("Unable to fetch hash list for updates", "Updater");
                    return;
                }
            }
            else
            {
                Files = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Path.Join(FilesUrl, "Hashes.json")))!;
            }

            foreach (var KeyPair in Files)
            {
                try
                {
                    if (LocalFiles[KeyPair.Key] != Files[KeyPair.Key])
                    {
                        Debugger.PrintInfo(KeyPair.Key + " is out of date", "Updater");
                    }
                }
                catch
                {
                }
            }
        }
    }
}

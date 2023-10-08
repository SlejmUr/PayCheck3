using Newtonsoft.Json;
using System.Security.Cryptography;

namespace PayCheckServerLib
{
    public class Updater
    {
        // that or https://raw.githubusercontent.com/MoolahModding/PayCheck3Files/main
        static readonly string FilesUrl = @"https://github.com/MoolahModding/PayCheck3Files/raw/main/";

        public static void CheckForJsonUpdates(bool UIHandleUpdate = false)
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
                    var FilesData = client.GetStringAsync(FilesUrl + "Hashes.json").Result;
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
                        Debugger.PrintInfo(LocalFiles[KeyPair.Key] + " " + Files[KeyPair.Key]);
                        Debugger.PrintInfo(KeyPair.Key + " is out of date", "Updater");
                        if (UIHandleUpdate)
                        {
                            //send stuff to ui to update
                            UpdateWithUI?.Invoke(null, KeyPair.Key);
                        }
                        else
                        {
                            Console.WriteLine("You want to update?\n Y/y = Yes , N/n = No");

                            var inp = Console.ReadLine();

                            if (string.IsNullOrEmpty(inp))
                            {
                                //Console.WriteLine("Your input was wrong, skipping");
                                continue;
                            }

                            inp = inp.ToLower();
                            if (inp == "y")
                            {
                                Debugger.PrintInfo("Updating started!");
                                HttpClient client = new();
                                var FilesData = client.GetStringAsync(FilesUrl + KeyPair.Key).Result;
                                File.WriteAllText("Files/" + KeyPair.Key, FilesData);
                            }
                            else
                            {
                                //Debugger.PrintInfo("Not want to update, Skipping");
                                continue;
                            }


                        }
                    }
                }
                catch (Exception ex)
                {
                    Debugger.PrintWarn(ex.ToString());
                    Debugger.PrintWarn("Unable to fetch get file to update", "Updater");
                }
            }
            Debugger.PrintInfo("Update Finished!");
        }


        public static void DownloadBetaFiles()
        {
            Dictionary<string, string> LocalFiles = new();
            foreach (var file in Directory.GetFiles("./Files"))
            {
                string hash = BitConverter.ToString(SHA256.HashData(File.ReadAllBytes(file))).Replace("-", "").ToLower();
                LocalFiles.Add(file.Replace("./Files\\", ""), hash);
            }
            HttpClient client = new();
            var FilesData = client.GetStringAsync(FilesUrl.Replace("main", "pd3beta") + "Hashes.json").Result;
            Dictionary<string, string> Files = JsonConvert.DeserializeObject<Dictionary<string, string>>(FilesData)!;
            foreach (var KeyPair in Files)
            {
                if (LocalFiles[KeyPair.Key] != Files[KeyPair.Key])
                {
                    client = new();
                    var data = client.GetStringAsync(FilesUrl.Replace("main", "pd3beta") + KeyPair.Key).Result;
                    File.WriteAllText("Files/" + KeyPair.Key, data);
                }
                else
                {
                    Debugger.PrintInfo("File " + KeyPair.Key + " is already downgraded to Beta!");
                }

            }
            Debugger.PrintInfo("Downgrade Finished!");
        }

        public static event EventHandler<string> UpdateWithUI;


    }
}

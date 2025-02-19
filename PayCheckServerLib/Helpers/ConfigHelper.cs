using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Helpers
{
    public class ConfigHelper
    {
        public static ServerConfig ServerConfig { get; internal set; }

        static ConfigHelper()
        {
            ServerConfig? conf = null;
            if (File.Exists("config.json"))
            {
                conf = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText("config.json"));
            }
            if (conf != null)
            {
                ServerConfig = conf;
            }
            else
            {
                ServerConfig = new()
                {
                    Hosting = new()
                    {
                        WSS = true,
                        Gstatic = true,
                        IP = "127.0.0.1",
                        Port = 443,
						CertificatePassword = "cert"
                    },
                    DS_Servers = new()
                    {
                        new()
                        {
                            Alias = "eu-central-1",
                            Ip = "127.0.0.1",
                            Port = 6969,
                            Region = "eu-central-1",
                            Status = "ACTIVE"
                        }
                    },
                    Saves = new()
                    {
                        Extension = "save",
                        SaveRequest = false,
                    },
                    InDevFeatures = new()
                    {
                        EnablePartySession = false,
                        UsePWInsteadSteamToken = false,
                        UseBasicWeaponTable = true
                    },
                    EnableAutoUpdate = true
                };
                File.WriteAllText("config.json", JsonConvert.SerializeObject(ServerConfig, Formatting.Indented));
                Debugger.PrintInfo("New ServerConfig created!", "ConfigHelper");
            }
        }
    }
}

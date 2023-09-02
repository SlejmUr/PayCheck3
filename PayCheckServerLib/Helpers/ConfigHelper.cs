using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Helpers
{
    public class ConfigHelper
    {
        public static ServerConfig ServerConfig { get; internal set; }

        static ConfigHelper()
        {
            var conf = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText("config.json"));
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
                        Server = true,
                        Gstatic = true,
                        Udp = true
                    },
                    Saves = new()
                    {
                        Extension = "save",
                        SaveRequest = true,
                    },
                    InDevFeatures = new()
                    {
                        EnablePartySession = false
                    },
                    EnableAutoUpdate = true
                };
                Debugger.PrintInfo("New ServerConfig created!", "ConfigHelper");
            }
        }
    }
}

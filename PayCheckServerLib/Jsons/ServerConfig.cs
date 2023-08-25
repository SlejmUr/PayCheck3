using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class ServerConfig
    {
        [JsonProperty("Saves")]
        public CSaves Saves { get; set; }

        [JsonProperty("Hosting")]
        public CHosting Hosting { get; set; }

        [JsonProperty("InDevFeatures")]
        public CInDevFeatures InDevFeatures { get; set; }

        public partial class CHosting
        {
            [JsonProperty("Server")]
            public bool Server { get; set; }

            [JsonProperty("UDP")]
            public bool Udp { get; set; }

            [JsonProperty("GSTATIC")]
            public bool Gstatic { get; set; }
        }

        public partial class CInDevFeatures
        {
            [JsonProperty("Enable_PartySession")]
            public bool EnablePartySession { get; set; }
        }

        public partial class CSaves
        {
            [JsonProperty("Extension")]
            public string Extension { get; set; }

            [JsonProperty("SaveRequest")]
            public bool SaveRequest { get; set; }
        }
    }
}

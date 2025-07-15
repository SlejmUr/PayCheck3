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

        [JsonProperty("DS_Servers")]
        public List<CDS_Server> DS_Servers { get; set; }

        [JsonProperty("EnableAutoUpdate")]
        public bool EnableAutoUpdate { get; set; }

        public partial class CHosting
        {
            [JsonProperty("IP")]
            public string IP { get; set; }

            [JsonProperty("Port")]
            public int Port { get; set; }

            [JsonProperty("Server")]
            public bool WSS { get; set; }

            [JsonProperty("GSTATIC")]
            public bool Gstatic { get; set; }

			[JsonProperty("SSLCertificatePassword")]
			public string CertificatePassword { get; set; }
        }

        public partial class CDS_Server
        {
            [JsonProperty("Ip")]
            public string Ip { get; set; }

            [JsonProperty("Port")]
            public int Port { get; set; }

            [JsonProperty("Alias")]
            public string Alias { get; set; }

            [JsonProperty("Region")]
            public string Region { get; set; }

            [JsonProperty("Status")]
            public string Status { get; set; }
        }

        public partial class CInDevFeatures
        {
            [JsonProperty("Enable_PartySession")]
            public bool EnablePartySession { get; set; }

            [JsonProperty("UsePWInsteadSteamToken")]
            public bool UsePWInsteadSteamToken { get; set; }

            [JsonProperty("UseBasicWeaponTable")]
            public bool UseBasicWeaponTable { get; set; }
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

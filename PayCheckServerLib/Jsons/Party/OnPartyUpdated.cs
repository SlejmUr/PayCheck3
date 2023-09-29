using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons.PartyStuff
{
    public class OnPartyUpdated
    {
        [JsonProperty("ID")]
        public string Id { get; set; }

        [JsonProperty("Namespace")]
        public string Namespace { get; set; }

        [JsonProperty("Members")]
        public List<PartyPost.Memberv2> Members { get; set; }

        [JsonProperty("Attributes")]
        public Dictionary<string, object> Attributes { get; set; }

        [JsonProperty("CreatedAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("UpdatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("Version")]
        public long Version { get; set; }

        [JsonProperty("Configuration")]
        public Configuration Configuration { get; set; }

        [JsonProperty("ConfigurationName")]
        public string ConfigurationName { get; set; }

        [JsonProperty("IsFull")]
        public bool IsFull { get; set; }

        [JsonProperty("LeaderID")]
        public string LeaderId { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }
    }

    public partial class Configuration
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Joinability")]
        public string Joinability { get; set; }

        [JsonProperty("MinPlayers")]
        public long MinPlayers { get; set; }

        [JsonProperty("MaxPlayers")]
        public long MaxPlayers { get; set; }

        [JsonProperty("ClientVersion")]
        public string ClientVersion { get; set; }

        [JsonProperty("RequestedRegions")]
        public List<string> RequestedRegions { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("InviteTimeout")]
        public long InviteTimeout { get; set; }

        [JsonProperty("InactiveTimeout")]
        public long InactiveTimeout { get; set; }
    }
}

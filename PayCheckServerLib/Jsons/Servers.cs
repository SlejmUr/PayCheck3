using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class JsonServers
    {
        [JsonProperty("servers")]
        public List<Server> Servers { get; set; }
    }
    public class Server
    {
        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("last_update")]
        public string LastUpdate { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}

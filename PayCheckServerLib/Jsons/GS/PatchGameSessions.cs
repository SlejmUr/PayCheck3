using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons.GS
{
    public class PatchGameSessions
    {
        [JsonProperty("attributes")]
        public Dictionary<string, object> Attributes { get; set; }

        [JsonProperty("version")]
        public int version { get; set; }

        [JsonProperty("teams")]
        public List<Team> Teams { get; set; }
    }
}

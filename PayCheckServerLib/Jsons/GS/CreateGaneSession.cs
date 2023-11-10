using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons.GS
{
    public class CreateGaneSession
    {
        [JsonProperty("configurationName")]
        public string ConfigurationName { get; set; }

        [JsonProperty("Attributes")]
        public Dictionary<string, object> Attributes { get; set; }

        [JsonProperty("maxPlayers")]
        public int MaxPlayers { get; set; }
    }
}

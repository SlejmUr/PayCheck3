using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class TitleData
    {
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("set_by")]
        public string SetBy { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("value")]
        public Dictionary<string,string> Value { get; set; }
    }
}

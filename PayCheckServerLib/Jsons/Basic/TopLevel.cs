using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons.Basic
{
    public class TopLevel<T> where T : class
    {
        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("set_by")]
        public string SetBy { get; set; }

        [JsonProperty("value")]
        public T Value { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }
}

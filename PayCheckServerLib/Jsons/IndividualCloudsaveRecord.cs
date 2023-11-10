using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class IndividualCloudsaveRecord
    {
        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("set_by")]
        public string SetBy { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }
    }
}

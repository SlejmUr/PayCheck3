using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class ProgressionSaveRSP
    {
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; } = false;

        [JsonProperty("key")]
        public string Key { get; set; } = "progressionsavegame";

        [JsonProperty("namespace")]
        public string Namespace { get; set; } = "pd3";

        [JsonProperty("set_by")]
        public string SetBy { get; set; } = "CLIENT";

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }
    }
}

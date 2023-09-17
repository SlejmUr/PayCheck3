using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class GetUserStatItemsData
    {
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("statCode")]
        public string StatCode { get; set; }

        [JsonProperty("statName")]
        public string StatName { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("value")]
        public float Value { get; set; }

        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }
    }
}

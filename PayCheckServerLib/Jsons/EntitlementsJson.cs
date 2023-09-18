using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class EntitlementsData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("clazz")]
        public string Clazz { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("sku", NullValueHandling = NullValueHandling.Ignore)]
        public string Sku { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        [JsonProperty("itemNamespace")]
        public string ItemNamespace { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("grantedAt")]
        public string GrantedAt { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("features", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Features { get; set; }

        [JsonProperty("useCount", NullValueHandling = NullValueHandling.Ignore)]
        public long? UseCount { get; set; }

        [JsonProperty("stackable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Stackable { get; set; }
    }
}

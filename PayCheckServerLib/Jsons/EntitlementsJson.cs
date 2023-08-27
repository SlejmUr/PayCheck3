using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class EntitlementsData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        // is clazz a typo?	-- No because class in C# code can be used to make a class so yeah
        [JsonProperty("clazz")]
        public string Clazz { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        [JsonProperty("itemNamespace")]
        public string ItemNamespace { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("useCount")]
        public int UseCount { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("stackable")]
        public bool Stackable { get; set; }

        // ISO8601 string
        [JsonProperty("grantedAt")]
        public string GrantedAt { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }
    }
    public class EntitlementPayloadJson
    {
        [JsonProperty("data")]
        public List<EntitlementsData> Data { get; set; }

        [JsonProperty("paging")]
        public object Paging { get; set; }
    }
}

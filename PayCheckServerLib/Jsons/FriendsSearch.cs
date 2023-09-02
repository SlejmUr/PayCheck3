using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class FriendsSearch
    {
        [JsonProperty("createdAt")]
        public string createdAt { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }
    }
}

using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class Bulk
    {
        [JsonProperty("data")]
        public List<CData> Data { get; set; }

        public partial class CData
        {
            [JsonProperty("avatarUrl")]
            public string AvatarUrl { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("platformUserIds")]
            public Dictionary<string,string> PlatformUserIds { get; set; }

            [JsonProperty("userId")]
            public string UserId { get; set; }
        }
    }
}

using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class UserBulk
    {
        [JsonProperty("data")]
        public List<UserBulkData> Data { get; set; }

        public partial class UserBulkData
        {
            [JsonProperty("avatarUrl")]
            public string AvatarUrl { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("platformUserIds")]
            public Dictionary<string, string> PlatformUserIds { get; set; }

            [JsonProperty("userId")]
            public string UserId { get; set; }
        }
    }
}

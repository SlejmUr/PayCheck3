using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class FriendAdd
    {
        [JsonProperty("friendIds")]
        public string[] FriendIds { get; set; }
    }
    public class FriendsPlatfrom
    {
        [JsonProperty("data")]
        public List<FriendsPlatfromData> Data { get; set; }

        public partial class FriendsPlatfromData
        {
            [JsonProperty("avatarUrl")]
            public string AvatarUrl { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("platformInfos")]
            public List<PlatformInfo> PlatformInfos { get; set; }

            [JsonProperty("userId")]
            public string UserId { get; set; }

            [JsonProperty("username")]
            public string Username { get; set; }
        }

        public partial class PlatformInfo
        {
            [JsonProperty("platformDisplayName")]
            public string PlatformDisplayName { get; set; }

            [JsonProperty("platformName")]
            public string PlatformName { get; set; }

            [JsonProperty("platformUserId")]
            public string PlatformUserId { get; set; }
        }
    }
}

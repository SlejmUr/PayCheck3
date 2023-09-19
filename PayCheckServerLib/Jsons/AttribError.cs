using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class AttribError
    {
        [JsonProperty("attributes")]
        public object Attributes { get; set; }

        [JsonProperty("errorCode")]
        public long ErrorCode { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class AttribSuccess
    {
        [JsonProperty("createdAt")]
        public string createdAt { get; set; } = "0001-01-01T00:00:00Z";

        [JsonProperty("deletedAt")]
        public string deletedAt { get; set; } = "0001-01-01T00:00:00Z";

        [JsonProperty("crossplayEnabled")]
        public bool CrossplayEnabled { get; set; }

        [JsonProperty("currentPlatform")]
        public string CurrentPlatform { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("platforms")]
        public List<Platform> Platforms { get; set; }

        [JsonProperty("userID")]
        public string UserId { get; set; }
    }

    public partial class Platform
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("userID")]
        public string UserId { get; set; }
    }

    public partial class AttribRequest
    {
        [JsonProperty("crossplayEnabled")]
        public bool CrossplayEnabled { get; set; }

        [JsonProperty("currentPlatform")]
        public string CurrentPlatform { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("platforms")]
        public List<Platform> Platforms { get; set; }
    }
}

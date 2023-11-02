using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class oathToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("bans")]
        public List<object> Bans { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("is_comply")]
        public bool IsComply { get; set; }

        [JsonProperty("jflgs")]
        public long Jflgs { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("namespace_roles")]
        public object NamespaceRoles { get; set; }

        [JsonProperty("permissions")]
        public List<Permission> Permissions { get; set; }

        [JsonProperty("platform_id")]
        public string PlatformId { get; set; }

        [JsonProperty("platform_user_id")]
        public string PlatformUserId { get; set; }

        [JsonProperty("roles")]
        public object Roles { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        public partial class Permission
        {
            [JsonProperty("resource")]
            public string Resource { get; set; }

            [JsonProperty("action")]
            public long Action { get; set; }
        }

    }
}

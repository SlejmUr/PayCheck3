using Newtonsoft.Json;

namespace SaveGetter
{
    public class LoginToken
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
        public List<NamespaceRole> NamespaceRoles { get; set; }

        [JsonProperty("permissions")]
        public List<object> Permissions { get; set; }

        [JsonProperty("platform_id")]
        public string PlatformId { get; set; }

        [JsonProperty("platform_user_id")]
        public string PlatformUserId { get; set; }

        [JsonProperty("refresh_expires_in")]
        public long RefreshExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("roles")]
        public List<string> Roles { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
    public partial class NamespaceRole
    {
        [JsonProperty("roleId")]
        public string RoleId { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }
    }
}

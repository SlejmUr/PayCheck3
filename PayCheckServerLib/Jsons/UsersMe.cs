using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class Me
    {
        [JsonProperty("authType")]
        public string AuthType { get; set; } = "EMAILPASSWD";

        [JsonProperty("bans")]
        public List<object> Bans { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; } = "0001-01-01T00:00:00Z";

        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; set; } = "0001-01-01T00:00:00Z";

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("deletionStatus")]
        public bool DeletionStatus { get; set; }

        [JsonProperty("emailVerified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("lastDateOfBirthChangedTime")]
        public string LastDateOfBirthChangedTime { get; set; } = "0001-01-01T00:00:00Z";

        [JsonProperty("lastEnabledChangedTime")]
        public string LastEnabledChangedTime { get; set; } = "0001-01-01T00:00:00Z";

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("namespaceRoles")]
        public List<NamespaceRole> NamespaceRoles { get; set; }

        [JsonProperty("oldEmailAddress")]
        public string OldEmailAddress { get; set; }

        [JsonProperty("permissions")]
        public List<object> Permissions { get; set; }

        [JsonProperty("phoneVerified")]
        public bool PhoneVerified { get; set; }

        [JsonProperty("roles")]
        public List<string> Roles { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("avatarUrl")]
        public Uri AvatarUrl { get; set; }
        
    }
}

using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class PartyPost
    {
        public partial class Basic
        {
            [JsonProperty("attributes")]
            public Dictionary<string, object> Attributes { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("configuration")]
            public Configuration Configuration { get; set; }

            [JsonProperty("createdAt")]
            public string CreatedAt { get; set; }

            [JsonProperty("createdBy")]
            public string CreatedBy { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("isActive")]
            public bool IsActive { get; set; }

            [JsonProperty("isFull")]
            public bool IsFull { get; set; }

            [JsonProperty("leaderID")]
            public string LeaderId { get; set; }

            [JsonProperty("members")]
            public List<Member> Members { get; set; }

            [JsonProperty("namespace")]
            public string Namespace { get; set; }

            [JsonProperty("updatedAt")]
            public string UpdatedAt { get; set; }

            [JsonProperty("version")]
            public long Version { get; set; }
        }

        public partial class Configuration
        {
            [JsonProperty("PSNBaseURL")]
            public string PsnBaseUrl { get; set; }

            [JsonProperty("autoJoin")]
            public bool AutoJoin { get; set; }

            [JsonProperty("clientVersion")]
            public string ClientVersion { get; set; }

            [JsonProperty("deployment")]
            public string Deployment { get; set; }

            [JsonProperty("dsSource")]
            public string DsSource { get; set; }

            [JsonProperty("fallbackClaimKeys")]
            public List<object> FallbackClaimKeys { get; set; }

            [JsonProperty("inactiveTimeout")]
            public long InactiveTimeout { get; set; }

            [JsonProperty("inviteTimeout")]
            public long InviteTimeout { get; set; }

            [JsonProperty("joinability")]
            public string Joinability { get; set; }

            [JsonProperty("maxPlayers")]
            public long MaxPlayers { get; set; }

            [JsonProperty("minPlayers")]
            public long MinPlayers { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("nativeSessionSetting")]
            public NativeSessionSetting NativeSessionSetting { get; set; }

            [JsonProperty("persistent")]
            public bool Persistent { get; set; }

            [JsonProperty("preferredClaimKeys")]
            public List<object> PreferredClaimKeys { get; set; }

            [JsonProperty("requestedRegions")]
            public List<string> RequestedRegions { get; set; }

            [JsonProperty("textChat")]
            public bool TextChat { get; set; }

            [JsonProperty("tieTeamsSessionLifetime")]
            public bool TieTeamsSessionLifetime { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public partial class NativeSessionSetting
        {
            [JsonProperty("PSNServiceLabel")]
            public long PsnServiceLabel { get; set; }

            [JsonProperty("PSNSupportedPlatforms")]
            public List<object> PsnSupportedPlatforms { get; set; }

            [JsonProperty("SessionTitle")]
            public string SessionTitle { get; set; }

            [JsonProperty("ShouldSync")]
            public bool ShouldSync { get; set; }

            [JsonProperty("XboxServiceConfigID")]
            public string XboxServiceConfigId { get; set; }

            [JsonProperty("XboxSessionTemplateName")]
            public string XboxSessionTemplateName { get; set; }

            [JsonProperty("localizedSessionName")]
            public LocalizedSessionName LocalizedSessionName { get; set; }
        }

        public partial class LocalizedSessionName
        {
            [JsonProperty("defaultLanguage")]
            public string DefaultLanguage { get; set; }

            [JsonProperty("localizedText")]
            public Dictionary<string, string> LocalizedText { get; set; }
        }

        public partial class Member
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("platformID")]
            public string PlatformId { get; set; }

            [JsonProperty("platformUserID")]
            public string PlatformUserId { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("statusV2")]
            public string StatusV2 { get; set; }

            [JsonProperty("updatedAt")]
            public string UpdatedAt { get; set; }
        }
    }
}

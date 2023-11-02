using Newtonsoft.Json;
using PayCheckServerLib.Jsons.Basic;

namespace PayCheckServerLib.Jsons
{
    public class PlatformBackendSettingsData : TopLevel<PlatformBackendSettingsData.PlatformBackendSettingsDataValue>
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }

        public partial class PlatformBackendSettingsDataValue
        {
            [JsonProperty("PlatformBackendSettings")]
            public PlatformBackendSettings PlatformBackendSettings { get; set; }
        }

        public partial class PlatformBackendSettings
        {
            [JsonProperty("platformBackendSettings")]
            public PlatformBackendSetting[] PlatformBackendSettingsPlatformBackendSettings { get; set; }
        }

        public partial class PlatformBackendSetting
        {
            [JsonProperty("bIsGameSenseEnabled")]
            public bool BIsGameSenseEnabled { get; set; }

            [JsonProperty("bIsTelemetryEnabled")]
            public bool BIsTelemetryEnabled { get; set; }

            [JsonProperty("platform")]
            public string Platform { get; set; }

            [JsonProperty("popupsShownBitmask")]
            public long PopupsShownBitmask { get; set; }
        }
    }
}

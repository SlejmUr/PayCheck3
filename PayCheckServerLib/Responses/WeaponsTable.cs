using Newtonsoft.Json;

namespace PayCheckServerLib.Responses
{
    public class WeaponsTableREQ
    {
        [JsonProperty("keys")]
        public List<string> Keys { get; set; }
    }
    public class WeaponsTable
    {
        [JsonProperty("data")]
        public List<CData> Data { get; set; }

        public partial class CData
        {
            [JsonProperty("created_at")]
            public string CreatedAt { get; set; } = "2023-06-27T12:18:00.00Z";

            [JsonProperty("key")]
            public string Key { get; set; }

            [JsonProperty("namespace")]
            public string Namespace { get; set; } = "pd3beta";

            [JsonProperty("set_by")]
            public string SetBy { get; set; } = "SERVER";

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; } = "2023-06-27T12:18:00.00Z";

            [JsonProperty("value")]
            public Value Value { get; set; }
        }

        public partial class Value
        {
            [JsonProperty("weapon-translation-table")]
            public List<WeaponTranslationTable> WeaponTranslationTable { get; set; }
        }

        public partial class WeaponTranslationTable
        {
            [JsonProperty("Weapon Level")]
            public long WeaponLevel { get; set; }

            [JsonProperty("Weapon Points")]
            public long WeaponPoints { get; set; }
        }
    }
}

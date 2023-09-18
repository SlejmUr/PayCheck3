using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class InfamyTranslationTable
    {
        public partial class Basic
        {
            [JsonProperty("created_at")]
            public string CreatedAt { get; set; } = "2023-06-27T12:18:00.00Z";

            [JsonProperty("key")]
            public string Key { get; set; } = "infamy-translation-table";

            [JsonProperty("namespace")]
            public string Namespace { get; set; } = "pd3";

            [JsonProperty("set_by")]
            public string SetBy { get; set; } = "SERVER";

            [JsonProperty("updated_at")]
            public string UpdatedAt { get; set; } = "2023-06-27T12:18:00.00Z";

            [JsonProperty("value")]
            public Value Value { get; set; }
        }

        public partial class Value
        {
            [JsonProperty("infamy-translation-table")]
            public List<CInfamyTranslationTable> InfamyTranslationTable { get; set; }
        }

        public partial class CInfamyTranslationTable
        {
            [JsonProperty("Infamy Level")]
            public long InfamyLevel { get; set; }

            [JsonProperty("Infamy Points")]
            public long InfamyPoints { get; set; }

            [JsonProperty("Infamy Rewards")]
            public List<InfamyReward> InfamyRewards { get; set; }
        }
        public partial class InfamyReward
        {
            [JsonProperty("Content Given Type")]
            public string ContentGivenType { get; set; }

            [JsonProperty("Content Given Value")]
            public long ContentGivenValue { get; set; }
        }
    }
}

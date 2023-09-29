using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class InfamyTranslationTable
    {

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

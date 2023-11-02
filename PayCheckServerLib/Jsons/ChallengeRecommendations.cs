using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    internal class ChallengeRecommendations
    {
        [JsonProperty("BlockArray")]
        public List<CBlockArray> BlockArray { get; set; }

        public class CBlockArray
        {
            [JsonProperty("ScreenName")]
            public string ScreenName { get; set; }

            [JsonProperty("SlotArray")]
            public List<CSlotArray> SlotArray { get; set; }
        }

        public class CSlotArray
        {
            [JsonProperty("CheckInfamyLevel")]
            public bool CheckInfamyLevel { get; set; }

            [JsonProperty("MandatoryTags")]
            public List<string> MandatoryTags { get; set; }

            [JsonProperty("challengeRecommandationsPriorityType")]
            public string ChallengeRecommandationsPriorityType { get; set; }
        }
    }
}

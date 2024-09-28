using Newtonsoft.Json;
using PayCheckServerLib.Jsons.Basic;

namespace PayCheckServerLib.Jsons
{
    public class ChallengeCloudSaveRecord_RSP : TopLevel<ChallengeCloudSaveRecord>
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }
    }

    public class ChallengeCloudSaveRecord
    {
        public int CurrentVersion { get; set; } = 23;
        public progression_save_challenges ProgressionSaveChallenges { get; set; } = new();
        public class progression_save_challenges
        {
            public _dailyChallengeBlockMap dailyChallengeBlockMap { get; set; } = new();
            public class _dailyChallengeBlockMap
            {
                public List<Challenege_NewStart> challengeArray { get; set; } = new();
            }

            public string dailyChallengePullDate { get; set; } = "2024.09.17-11.32.37";
            public bool fetchedFromAPI { get; set; } = true;
            public bool rerollAvailable { get; set; } = false;
            public List<ChallenegeID_Completed> savedChallenges { get; set; } = new();
        }
    }

    public class ChallenegeID_Completed
    {
        public string challengeId { get; set; }
        public bool challengeCompleted { get; set; }
    }

    public class Challenege_NewStart : ChallenegeID_Completed
    {
        public int creationObjectiveStartStatValue { get; set; }
    }

}

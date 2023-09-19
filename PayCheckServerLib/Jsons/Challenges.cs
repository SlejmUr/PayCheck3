using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public partial class ChallengesData
    {
        [JsonProperty("recordId")]
        public string RecordId { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("challenge")]
        public Challenge Challenge { get; set; }

        [JsonProperty("progress")]
        public Progress Progress { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

    public partial class Challenge
    {
        [JsonProperty("challengeId")]
        public string ChallengeId { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("prerequisite")]
        public ChallengePrerequisite Prerequisite { get; set; }

        [JsonProperty("objective")]
        public ChallengeObjective Objective { get; set; }

        [JsonProperty("reward")]
        public Reward Reward { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("orderNo")]
        public long OrderNo { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

    public partial class ChallengeObjective
    {
        [JsonProperty("stats")]
        public RewardStat[] Stats { get; set; }
    }

    public partial class RewardStat
    {
        [JsonProperty("statCode")]
        public string StatCode { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }
    }

    public partial class ChallengePrerequisite
    {
        [JsonProperty("stats")]
        public List<object> Stats { get; set; }

        [JsonProperty("items")]
        public List<object> Items { get; set; }

        [JsonProperty("completedChallengeIds")]
        public List<string> CompletedChallengeIds { get; set; }
    }

    public partial class Reward
    {
        [JsonProperty("rewardId")]
        public string RewardId { get; set; }

        [JsonProperty("stats")]
        public List<RewardStat> Stats { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

    public partial class Progress
    {
        [JsonProperty("prerequisite")]
        public ProgressPrerequisite Prerequisite { get; set; }

        [JsonProperty("objective")]
        public ProgressObjective Objective { get; set; }
    }

    public partial class ProgressObjective
    {
        [JsonProperty("stats")]
        public List<PurpleStat> Stats { get; set; }
    }

    public partial class PurpleStat
    {
        [JsonProperty("statCode")]
        public string StatCode { get; set; }

        [JsonProperty("currentValue")]
        public long CurrentValue { get; set; }

        [JsonProperty("targetValue")]
        public long TargetValue { get; set; }
    }

    public partial class ProgressPrerequisite
    {
        [JsonProperty("stats")]
        public List<object> Stats { get; set; }

        [JsonProperty("items")]
        public List<object> Items { get; set; }

        [JsonProperty("completedChallengeIds")]
        public List<CompletedChallengeId> CompletedChallengeIds { get; set; }
    }

    public partial class CompletedChallengeId
    {
        [JsonProperty("challengeId")]
        public string ChallengeId { get; set; }

        [JsonProperty("isCompleted")]
        public bool IsCompleted { get; set; }
    }


}

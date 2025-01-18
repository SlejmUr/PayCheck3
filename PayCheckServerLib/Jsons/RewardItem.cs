using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib.Jsons
{
	public class RewardItem
	{
		public class RewardCondition
		{
			public class RewardItem
			{
				[JsonProperty("itemId")]
				public string ItemId { get; set; }
				[JsonProperty("quantity")]
				public int Quantity { get; set; }
			}
			[JsonProperty("conditionName")]
			public string ConditionName { get; set; }
			[JsonProperty("condition")]
			public string Condition { get; set; }
			[JsonProperty("eventName")]
			public string EventName { get; set; }
			[JsonProperty("rewardItems")]
			public List<RewardItem> RewardItems { get; set; }
		}
		[JsonProperty("rewardId")]
		public string RewardId { get; set; }
		[JsonProperty("namespace")]
		public string Namespace { get; set; }
		[JsonProperty("rewardCode")]
		public string RewardCode { get; set; }
		[JsonProperty("description")]
		public string Description { get; set; }
		[JsonProperty("eventTopic")]
		public string EventTopic { get; set; }
		[JsonProperty("rewardConditions")]
		public List<RewardCondition> RewardConditions { get; set; }
		[JsonProperty("maxAwarded")]
		public int MaxAwarded { get; set; }
		[JsonProperty("maxAwardedPerUser")]
		public int MaxAwardedPerUser { get; set; }
		[JsonProperty("CreatedAt")]
		public string CreatedAt { get; set; }
		[JsonProperty("UpdatedAt")]
		public string UpdatedAt { get; set; }
	}
}

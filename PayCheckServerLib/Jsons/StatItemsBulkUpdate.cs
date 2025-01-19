using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
	public class BulkStatItemUpdateRequestData
	{
		[JsonProperty("statCode")]
		public string StatCode { get; set; }
		[JsonProperty("inc", NullValueHandling = NullValueHandling.Ignore)]
		public float? Inc { get; set; }
	}
	public class BulkStatItemUpdateResponseData
	{
		public class BulkStatItemUpdateResponseDataDetails
		{
			[JsonProperty("currentValue")]
			public float CurrentValue { get; set; }
		}
		[JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
		public string? UserId { get; set; }
		[JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
		public bool? Success { get; set; }
		[JsonProperty("statCode", NullValueHandling = NullValueHandling.Ignore)]
		public string? StatCode { get; set; }
		[JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)]
		public BulkStatItemUpdateResponseDataDetails Details { get; set; }
	}
}

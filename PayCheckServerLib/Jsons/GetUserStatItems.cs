using System;
using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons {
	public class GetUserStatItemsData {
		[JsonProperty("createdAt")]
		public string CreatedAt { get; set; }

		[JsonProperty("namespace")]
		public string Namespace { get; set; }

		[JsonProperty("statCode")]
		public string StatCode { get; set; }

		[JsonProperty("statName")]
		public string StatName { get; set; }

		[JsonProperty("updatedAt")]
		public string UpdatedAt { get; set; }

		[JsonProperty("userId")]
		public string UserId { get; set; }

		[JsonProperty("value")]
		public float Value { get; set; }
	}
	public class GetUserStatItems {
		[JsonProperty("data")]
		public GetUserStatItemsData[] Data { get; set; }

		[JsonProperty("paging")]
		public object Paging { get; set; }
	}
}

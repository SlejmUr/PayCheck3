using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib.Jsons.CloudSave
{
	public class CloudSaveDataWrapper<T> where T : class
	{
		[JsonProperty("value")]
		public T Value { get; set; }

		[JsonProperty("namespace")]
		public string Namespace { get; set; }

		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("set_by")]
		public string SetBy { get; set; }

		// Added to user records in a recent update to their server/AccelByte UE4 SDK. The game fails to fetch all user records if this is missing, as a side effect of not fetching the save file it's not possible to load into any levels.
		[JsonProperty("is_public", NullValueHandling = NullValueHandling.Ignore)]
		public bool? IsPublic { get; set; }

		// Added to user records in a recent update to their server/AccelByte UE4 SDK. The game fails to fetch all user records if this is missing, as a side effect of not fetching the save file it's not possible to load into any levels.
		[JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
		public string? UserId { get; set; }

		[JsonProperty("created_at")]
		public string CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public string UpdatedAt { get; set; }
	}
}

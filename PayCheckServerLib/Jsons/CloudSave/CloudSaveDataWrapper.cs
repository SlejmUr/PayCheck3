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

		[JsonProperty("created_at")]
		public string CreatedAt { get; set; }
		[JsonProperty("updated_at")]
		public string UpdatedAt { get; set; }
	}
}

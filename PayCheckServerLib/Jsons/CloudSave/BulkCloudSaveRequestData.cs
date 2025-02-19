using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib.Jsons.CloudSave
{
	public class BulkCloudSaveRequestData
	{
		[JsonProperty("keys")]
		public List<string> Keys { get; set; }
	}
	public class BulkCloudSaveRequestResponseData {
		[JsonProperty("data")]
		public List<CloudSaveDataWrapper<object>> Data { get; set; }
	}
}

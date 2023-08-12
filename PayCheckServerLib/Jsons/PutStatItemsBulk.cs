using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib.Jsons {
	public class PutStatItemsBulkDetailsData {
		[JsonProperty("currentValue")]
		public float CurrentValue { get; set; }
	}
	public class PutStatItemsBulk {
		[JsonProperty("details")]
		public PutStatItemsBulkDetailsData Details { get; set; }

		[JsonProperty("statCode")]
		public string StatCode { get; set; }

		[JsonProperty("success")]
		public bool Success { get; set; }
	}
}

using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons.Basic
{
    public class DataPaging<T> where T : class
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }

        [JsonProperty("paging", NullValueHandling = NullValueHandling.Ignore)]
        public PagingClass Paging { get; set; }
    }

    public class PagingClass
    {
		public PagingClass()
		{
			First = null;
			Last = null;
			Next = null;
			Previous = null;
		}
        [JsonProperty("first", NullValueHandling = NullValueHandling.Ignore)]
        public string? First { get; set; }

        [JsonProperty("last", NullValueHandling = NullValueHandling.Ignore)]
        public string? Last { get; set; }

        [JsonProperty("next", NullValueHandling = NullValueHandling.Ignore)]
        public string? Next { get; set; }

        [JsonProperty("previous", NullValueHandling = NullValueHandling.Ignore)]
        public string? Previous { get; set; }
    }
}

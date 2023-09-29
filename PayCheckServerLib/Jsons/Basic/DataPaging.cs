using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons.Basic
{
    public class DataPaging<T> where T : class
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }

        [JsonProperty("paging")]
        public PagingClass Paging { get; set; }
    }

    public class PagingClass
    {
        [JsonProperty("first")]
        public string First { get; set; }

        [JsonProperty("last")]
        public string Last { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("previous")]
        public string Previous { get; set; }
    }
}

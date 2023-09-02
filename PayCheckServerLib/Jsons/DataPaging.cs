using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class DataPaging<T> where T : class
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }

        [JsonProperty("paging")]
        public Paging Paging { get; set; }
    }
}

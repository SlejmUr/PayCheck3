using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class BulkReq
    {
        [JsonProperty("userIds")]
        public string[] UserIds { get; set; }
    }
}

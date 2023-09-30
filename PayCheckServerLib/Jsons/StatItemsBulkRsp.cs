using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class PutStatItemsBulkDetailsData
    {
        [JsonProperty("currentValue")]
        public float CurrentValue { get; set; }
    }
    public class StatItemsBulkRsp
    {
        [JsonProperty("details")]
        public PutStatItemsBulkDetailsData Details { get; set; }

        [JsonProperty("statCode")]
        public string StatCode { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public class StatItemsBulkReq
    {
        [JsonProperty("inc")]
        public int Inc { get; set; }

        [JsonProperty("statCode")]
        public string StatCode { get; set; }
    }
}

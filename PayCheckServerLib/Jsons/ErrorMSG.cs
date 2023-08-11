using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class ErrorMSG
    {
        [JsonProperty("errorCode")]
        public long ErrorCode { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}

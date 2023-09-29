using Newtonsoft.Json;
using PayCheckServerLib.Jsons.Basic;

namespace PayCheckServerLib.Jsons
{
    public class ProgressionSaveRSP : TopLevel<object>
    {
        [JsonProperty("is_public")]
        public bool IsPublic { get; set; } = false;

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}

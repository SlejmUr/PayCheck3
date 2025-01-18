using Newtonsoft.Json;
using PayCheckServerLib.Jsons.Basic;
using PayCheckServerLib.Jsons.CloudSave;

namespace PayCheckServerLib.Jsons
{
    public class ProgressionSaveRSP : CloudSaveDataWrapper<object>
    {
        [JsonProperty("is_public")]
        public bool IsPublic { get; set; } = false;

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}

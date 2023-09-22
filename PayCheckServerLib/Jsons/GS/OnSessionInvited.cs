using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons.GS
{
    public class OnSessionInvited
    {
        [JsonProperty("SessionID")]
        public string SessionID { get; set; }

        [JsonProperty("SenderID")]
        public string SenderID { get; set; }
    }
}

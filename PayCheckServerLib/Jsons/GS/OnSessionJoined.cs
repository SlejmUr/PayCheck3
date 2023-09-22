using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons.GS
{
    public class OnSessionJoined
    {
        [JsonProperty("SessionID")]
        public string SessionID { get; set; }

        [JsonProperty("Members")]
        public List<PartyStuff.PartyPost.Member> Members { get; set; }

        [JsonProperty("TextChat")]
        public bool TextChat { get; set; }
    }
}

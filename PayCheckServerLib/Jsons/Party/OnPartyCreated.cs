using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons.PartyStuff
{
    public class OnPartyCreated
    {
        [JsonProperty("CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("PartyID")]
        public string PartyId { get; set; }

        [JsonProperty("TextChat")]
        public bool TextChat { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }
    }
}

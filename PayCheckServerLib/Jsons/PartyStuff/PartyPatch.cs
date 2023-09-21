using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons.PartyStuff
{
    public class PartyPatch
    {
        [JsonProperty("attributes")]
        public Dictionary<string,object> Attributes { get; set; }

        [JsonProperty("inactiveTimeout", NullValueHandling = NullValueHandling.Ignore)]
        public int? InactiveTimeout { get; set; }

        [JsonProperty("inviteTimeout", NullValueHandling = NullValueHandling.Ignore)]
        public int? InviteTimeout { get; set; }

        [JsonProperty("minPlayers", NullValueHandling = NullValueHandling.Ignore)]
        public int? MinPlayers { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }
    }
}

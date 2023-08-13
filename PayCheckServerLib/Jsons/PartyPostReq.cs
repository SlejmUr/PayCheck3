using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class PartyPostReq
    {
        public partial class Basic
        {
            [JsonProperty("members")]
            public List<Member> Members { get; set; }

            [JsonProperty("attributes")]
            public Dictionary<string, object> Attributes { get; set; }

            [JsonProperty("configurationName")]
            public string ConfigurationName { get; set; }

            [JsonProperty("textChat")]
            public bool TextChat { get; set; }
        }

        public partial class Member
        {
            [JsonProperty("iD")]
            public string ID { get; set; }

            [JsonProperty("statusV2")]
            public string StatusV2 { get; set; }

            [JsonProperty("platformId")]
            public string PlatformId { get; set; }

            [JsonProperty("platformUserId")]
            public string PlatformUserId { get; set; }
        }
    }
}

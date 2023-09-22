using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons.GS
{
    public class OnMatchFound
    {
        [JsonProperty("ID")]
        public string ID { get; set; }

        [JsonProperty("Namespace")]
        public string Namespace { get; set; }

        [JsonProperty("CreatedAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("MatchPool")]
        public string MatchPool { get; set; }

        [JsonProperty("Teams")]
        public List<Team> Teams { get; set; }

        [JsonProperty("Tickets")]
        public List<Ticket> Tickets { get; set; }

        public class Team
        {
            [JsonProperty("UserIDs")]
            public List<string> UserIDs { get; set; }
        }

        public class Ticket
        {
            [JsonProperty("TicketID")]
            public string TicketID { get; set; }
        }
    }
}

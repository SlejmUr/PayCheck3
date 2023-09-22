using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons.GS
{
    public class OnDSStatusChanged
    {
        public class Basic
        {
            [JsonProperty("SessionID")]
            public string SessionID { get; set; }

            [JsonProperty("GameServer")]
            public DSInformationServer GameServer { get; set; }

            [JsonProperty("Session")]
            public Session Session { get; set; }
        }

        public class DSInformation
        {
            [JsonProperty("Server")]
            public DSInformationServer Server { get; set; }

            [JsonProperty("Status")]
            public string Status { get; set; }

            [JsonProperty("StatusV2")]
            public string StatusV2 { get; set; }
        }

        public class Session
        {
            [JsonProperty("ID")]
            public string ID { get; set; }

            [JsonProperty("Namespace")]
            public string Namespace { get; set; }

            [JsonProperty("Members")]
            public List<PartyStuff.PartyPost.Memberv2> Members { get; set; }

            [JsonProperty("Attributes")]
            public Dictionary<string,object> Attributes { get; set; }

            [JsonProperty("CreatedAt")]
            public string CreatedAt { get; set; }

            [JsonProperty("CreatedBy")]
            public string CreatedBy { get; set; }

            [JsonProperty("UpdatedAt")]
            public string UpdatedAt { get; set; }

            [JsonProperty("Version")]
            public int Version { get; set; }

            [JsonProperty("Configuration")]
            public Dictionary<string, object> Configuration { get; set; }

            [JsonProperty("ConfigurationName")]
            public string ConfigurationName { get; set; }

            [JsonProperty("IsFull")]
            public bool IsFull { get; set; }

            [JsonProperty("LeaderID")]
            public string LeaderID { get; set; }

            [JsonProperty("BackfillTicketID")]
            public string BackfillTicketID { get; set; }

            [JsonProperty("Teams")]
            public List<Team> Teams { get; set; }

            [JsonProperty("DSInformation")]
            public DSInformation DSInformation { get; set; }

            [JsonProperty("MatchPool")]
            public string MatchPool { get; set; }

            [JsonProperty("Code")]
            public string Code { get; set; }

            [JsonProperty("GameMode")]
            public string GameMode { get; set; }
        }
    }
}

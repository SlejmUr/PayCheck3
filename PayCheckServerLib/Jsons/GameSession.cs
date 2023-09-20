using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class GameSession
    {
        [JsonProperty("DSInformation")]
        public DSInformation DSInformation { get; set; }

        [JsonProperty("attributes")]
        public Dictionary<string,object> Attributes { get; set; }

        [JsonProperty("backfillTicketID")]
        public string BackfillTicketID { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("configuration")]
        public PartyStuff.PartyPost.Configuration Configuration { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("isFull")]
        public bool IsFull { get; set; }

        [JsonProperty("leaderID")]
        public string LeaderID { get; set; }

        [JsonProperty("matchPool")]
        public string MatchPool { get; set; }

        [JsonProperty("members")]
        public List<PartyStuff.PartyPost.Member> Members { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("teams")]
        public List<Team> Teams { get; set; }

        [JsonProperty("ticketIDs")]
        public object TicketIDs { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }
    }
    public class Party
    {
        [JsonProperty("partyID")]
        public string PartyID { get; set; }

        [JsonProperty("userIDs")]
        public List<string> UserIDs { get; set; }
    }

    public class Ports
    {
        [JsonProperty("beacon")]
        public int Beacon { get; set; }
    }
    public class DSInformation
    {
        [JsonProperty("RequestedAt")]
        public string RequestedAt { get; set; }

        [JsonProperty("Server")]
        public DSInformationServer Server { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("StatusV2")]
        public string StatusV2 { get; set; }
    }
    public class DSInformationServer
    {
        [JsonProperty("alternate_ips")]
        public List<string> AlternateIps { get; set; }

        [JsonProperty("ams_protocol")]
        public object AmsProtocol { get; set; }

        [JsonProperty("custom_attribute")]
        public string CustomAttribute { get; set; }

        [JsonProperty("deployment")]
        public string Deployment { get; set; }

        [JsonProperty("game_version")]
        public string GameVersion { get; set; }

        [JsonProperty("image_version")]
        public string ImageVersion { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("is_override_game_version")]
        public bool IsOverrideGameVersion { get; set; }

        [JsonProperty("last_update")]
        public string LastUpdate { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("pod_name")]
        public string PodName { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("ports")]
        public Ports Ports { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class Team
    {
        [JsonProperty("UserIDs")]
        public List<string> UserIDs { get; set; }

        [JsonProperty("parties")]
        public List<Party> Parties { get; set; }
    }
}

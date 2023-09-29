using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons.WSS
{
    internal class Chats
    {
        public partial class ChatBase
        {
            [JsonProperty("jsonrpc")]
            public string Jsonrpc { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("method")]
            public string Method { get; set; }
        }

        public class actionQueryTopic : ChatBase
        {
            [JsonProperty("params")]
            public actionQueryTopicParams Params { get; set; }
        }

        public class actionQueryTopicRSP : ChatBase
        {
            [JsonProperty("result")]
            public actionQueryTopicRSPResult Params { get; set; }
        }

        public partial class actionQueryTopicRSPResult
        {
            [JsonProperty("processed")]
            public string Processed { get; set; }
        }
        public partial class actionQueryTopicParams
        {
            [JsonProperty("keyword")]
            public string Keyword { get; set; }

            [JsonProperty("offset")]
            public long Offset { get; set; }

            [JsonProperty("limit")]
            public long Limit { get; set; }

            [JsonProperty("namespace")]
            public string Namespace { get; set; }
        }
    }
}

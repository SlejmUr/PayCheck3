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

        public partial class ChatBase2
        {
            [JsonProperty("jsonrpc")]
            public string Jsonrpc { get; set; }

            [JsonProperty("method")]
            public string Method { get; set; }
        }

        public class actionQueryTopic : ChatBase
        {
            [JsonProperty("params")]
            public actionQueryTopicParams Params { get; set; }
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

        public class eventAddedToTopic : ChatBase2
        {
            [JsonProperty("params")]
            public eventAddedToTopicParams Params { get; set; }
        }

        public partial class eventAddedToTopicParams
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("topicId")]
            public string TopicId { get; set; }

            [JsonProperty("senderId")]
            public string SenderId { get; set; }

            [JsonProperty("userId")]
            public string UserId { get; set; }
        }


        public class actionQueryTopicById : ChatBase
        {
            [JsonProperty("params")]
            public actionQueryTopicByIdParams Params { get; set; }
        }
        public partial class actionQueryTopicByIdParams
        {
            [JsonProperty("topicId")]
            public string TopicId { get; set; }
        }


        public class actionQueryTopicByIdRSP : ChatBase
        {
            [JsonProperty("result")]
            public actionQueryTopicByIdRSPResult Result { get; set; }
        }

        public partial class actionQueryTopicByIdRSPResult
        {
            [JsonProperty("processed")]
            public string Processed { get; set; }

            [JsonProperty("data")]
            public actionQueryTopicByIdRSPResultData Data { get; set; }
        }

        public class actionQueryTopicByIdRSPResultData
        {
            [JsonProperty("topicId")]
            public string TopicId { get; set; }

            [JsonProperty("updatedAt")]
            public string UpdatedAt { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("members")]
            public List<string> Members { get; set; }
        }
    }
}

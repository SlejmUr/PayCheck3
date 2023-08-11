using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class Time
    {
        [JsonProperty("currentTime")]
        public string CurrentTime { get; set; }
    }
}

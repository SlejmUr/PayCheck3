namespace PayCheckServerLib.Jsons;

public class EdgeGapBeacons
{
    public List<Beacon> servers = [];

    public class Beacon
    {
        public string ip { get; set; } = "127.0.0.1";
        public string last_update { get; set; }
        public int port { get; set; } = 8888;
        public string region { get; set; } = "sydney";
        public string status { get; set; } = "ACTIVE";
    }

}

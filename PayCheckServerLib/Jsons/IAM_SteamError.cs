namespace PayCheckServerLib.Jsons
{
    public class IAM_SteamError
    {
        public string clientId { get; set; } = "uuidv4";
        public string error { get; set; } = "platform_not_linked";
        public string linkingToken { get; set; } = "uuidv4";
        public string platformId { get; set; } = "steam";
    }
}

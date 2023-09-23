namespace PayCheckServerLib.Jsons
{
    public class SteamUsers
    {
        public List<UserIdPlatform> userIdPlatforms { get; set; }

        public class UserIdPlatform
        {
            public string platformId { get; set; }
            public string platformUserId { get; set; }
            public string userId { get; set; }
        }
    }

    public class SteamUsersReq
    {
        public List<string> platformUserIds { get; set; }

    }
}

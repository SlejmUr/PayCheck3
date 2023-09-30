namespace PayCheckServerLib.Jsons
{
    public class User
    {
        public UserBulk.UserBulkData UserData;
        public List<FriendsPlatfrom.FriendsPlatfromData> Friends;
        public FStatus Status;
        //public string Namespace;

        public class FStatus
        {
            public string availability;
            public string activity;
            public string platform;
            public string lastSeenAt;
        }
    }
}

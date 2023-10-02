namespace PayCheckServerLib.Helpers
{
    public class TimeHelper
    {
        public static long GetEpochTime()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)t.TotalSeconds;
        }

        public static string GetZTime()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        public static string GetOTime()
        {
            return DateTime.UtcNow.ToString("o");
        }
    }
}

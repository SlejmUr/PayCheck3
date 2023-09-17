namespace PayCheckServerLib.Helpers
{
    public class BranchHelper
    {
        public static string GetBranch()
        {
            return GetFile().Split("\n")[0];
        }

        public static string GetBuildDate()
        {
            return GetFile().Split("\n")[1];
        }

        public static string GetCommitId()
        {
            return GetFile().Split("\n")[2];
        }

        static string GetFile()
        {
            return Properties.Resources.BuildDate;
        }
    }
}

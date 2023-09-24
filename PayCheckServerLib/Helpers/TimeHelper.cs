using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib.Helpers
{
    public class TimeHelper
    {
        public static long GetEpochTime()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)t.TotalSeconds;
        }
    }
}

using System.Security.Cryptography;
using System.Text;

namespace PayCheckServerLib
{
    /// <summary>
    /// Help to Generate UserId's
    /// </summary>
    public class UserIdHelper
    {
		/// <summary>
		/// Check if a given id does not contain any characters for path traversal, and can be interpreted as a valid UUID
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public static bool IsValidUserId(string userId)
		{
			if (userId.Contains("/") || userId.Contains("\\") || userId.Contains("."))
				return false;

			return true;
		}

		/// <summary>
		/// Generate New UserId
		/// </summary>
		/// <returns>New UserId</returns>
		public static string CreateNewID()
        {
            Random rand = new();
            int stringlen = 32;
            int randValue;
            string str = "";
            char letter;
            for (int i = 0; i < stringlen; i++)
            {
                randValue = rand.Next(0, 26);
                letter = Convert.ToChar(randValue + 65);
                str = str + letter;
            }
            string md5_str = ConvertStringtoMD5(str);
            return md5_str;
        }


        /// <summary>
        /// Gemerating Code for Party
        /// </summary>
        /// <returns></returns>
        public static string CreateCode()
        {
            Random rand = new();
            int stringlen = 7;
            int randValue;
            string str = "";
            char letter;
            for (int i = 0; i < stringlen; i++)
            {
                randValue = rand.Next(0, 26);
                letter = Convert.ToChar(randValue + 65);
                str = str + letter;
            }
            return str;
        }

        /// <summary>
        /// Creating String from MD5
        /// </summary>
        /// <param name="strword"></param>
        /// <returns></returns>
        private static string ConvertStringtoMD5(string strword)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(strword);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));
            return sb.ToString();
        }


        public static string getsai(string str)
        {
            return BitConverter.ToUInt32(Convert.FromHexString(str)[72..(72 + 4)]).ToString();
        }

        /// <summary>
        /// Getting the SteamID from IAM Login body
        /// </summary>
        /// <param name="AUTH">platform_ticket</param>
        /// <returns>SteamID</returns>
        public static string GetSteamIDFromAUTH(string AUTH)
        {
            var hex = Convert.FromHexString(AUTH);
            var sid = BitConverter.ToUInt64(hex[12..(12 + 8)]);
            return sid.ToString();
        }
    }
}

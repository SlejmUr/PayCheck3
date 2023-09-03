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

        /// <summary>
        /// Getting SteamID from the .txt file
        /// </summary>
        /// <returns>SteamID</returns>
        public static string GetSteamID()
        {
            var id = File.ReadAllLines("Files/steamid.txt")[0];
            if (id.Contains("\n"))
            {
                id.Replace("\n", "");
            }
            if (id.Contains("\r"))
            {
                id.Replace("\r", "");
            }
            if (id.Contains(" "))
            {
                id.Replace(" ", "");
            }
            return id;
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

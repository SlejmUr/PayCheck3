using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib
{
    public class UserIdHelper
    {
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

        public static string ConvertStringtoMD5(string strword)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(strword);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));
            return sb.ToString();
        }

        public static string GetSteamID()
        {
            var id = File.ReadAllLines("Files/steamid.txt")[0];
            if (id.Contains("\n"))
            {
                id.Replace("\n","");
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

        public static string GetSteamIdFromAUTH(string AUTH)
        {
            var hex = Convert.FromHexString(AUTH);
            var sid = BitConverter.ToUInt64(hex[12..(12 + 8)]);

            return sid.ToString();
        }
    }
}

namespace PayCheckServerLib
{
    public class TokenHelper
    {
        public struct Token
        {
            public string Name;
            public string PlatformId;
            public string UserId;
            public bool IsAccessToken;
        }

        public static Token GenerateNewToken(bool IsAccessToken = true)
        { 
            Token token = new()
            { 
                Name = "DefaultUser",
                PlatformId = UserIdHelper.GetSteamID(),
                UserId = UserIdHelper.CreateNewID(),
                IsAccessToken = IsAccessToken
            };

            return token;
        }

        public static byte[] TokenToBArray(Token token)
        {
            MemoryStream ms = new();
            ms.Write(System.Text.Encoding.UTF8.GetBytes(token.Name));
            ms.Write(Convert.FromHexString(token.PlatformId));
            ms.Write(Convert.FromHexString(token.UserId));
            ms.Write(BitConverter.GetBytes(token.IsAccessToken));
            return ms.ToArray();
        }


        public static void StoreToken(Token token)
        {
            if (!Directory.Exists("Tokens")) { Directory.CreateDirectory("Tokens"); }

            //File.WriteAllText();
        }
    }

    public static class TokenExt
    {
        public static string ToString(this TokenHelper.Token token)
        {
            return $"Name: {token.Name}, SteamId: {token.PlatformId}, UserId: {token.UserId}, IsAccessToken: {token.IsAccessToken}";
        }

        public static byte[] ToBytes(this TokenHelper.Token token)
        { 
            return TokenHelper.TokenToBArray(token);
        }

        public static string ToBase64(this TokenHelper.Token token)
        {
            return Convert.ToBase64String(TokenHelper.TokenToBArray(token));
        }

    }
}

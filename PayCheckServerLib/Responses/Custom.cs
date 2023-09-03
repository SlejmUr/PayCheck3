using NetCoreServer;
using PayCheckServerLib.Helpers;

namespace PayCheckServerLib.Responses
{
    public class Custom
    {
        [HTTP("POST", "/register?username={username}&platformId={pid}&platformType={ptype}")]
        public static bool RegisterUser(HttpRequest request, PC3Server.PC3Session session)
        {
            var username = session.HttpParam["username"];
            var platformId = session.HttpParam["pid"];
            var platformType = session.HttpParam["ptype"];
            var platform = (TokenHelper.TokenPlatform)uint.Parse(platformType);

            UserController.RegisterUser(platformId, platform, username);
            session.SendResponse(session.Response.MakeOkResponse());
            return true;
        }
    }
}

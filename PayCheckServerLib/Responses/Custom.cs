using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using PayCheckServerLib.Helpers;

namespace PayCheckServerLib.Responses
{
    public class Custom
    {
        [HTTP("POST", "/register?username={username}&platformId={pid}&platformType={ptype}&nspace={nspace}")]
        public static bool RegisterUser(HttpRequest request, PC3Server.PC3Session session)
        {
            var username = session.HttpParam["username"];
            var platformId = session.HttpParam["pid"];
            var platformType = session.HttpParam["ptype"];
            var platform = (TokenHelper.TokenPlatform)uint.Parse(platformType);
            var nspace = session.HttpParam["nspace"];

            UserController.RegisterUser(platformId, platform, username, nspace);
            session.SendResponse(session.Response.MakeOkResponse());
            return true;
        }
    }
}

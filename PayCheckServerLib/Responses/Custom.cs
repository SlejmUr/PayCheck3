using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using PayCheckServerLib.Helpers;

namespace PayCheckServerLib.Responses;

public class Custom
{
    [HTTP("POST", "/register?username={username}&platformId={pid}&platformType={ptype}&nspace={nspace}")]
    public static bool RegisterUser(HttpRequest _, ServerStruct serverStruct)
    {
        var username = serverStruct.Parameters["username"];
        var platformId = serverStruct.Parameters["pid"];
        var platformType = serverStruct.Parameters["ptype"];
        var platform = (TokenHelper.TokenPlatform)uint.Parse(platformType);
        var nspace = serverStruct.Parameters["nspace"];

        UserController.RegisterUser(platformId, platform, username, nspace);
        serverStruct.Response.MakeOkResponse();
        serverStruct.SendResponse();
        return true;
    }
}

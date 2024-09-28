using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;

namespace PayCheckServerLib.Responses;

public class Telemetry
{
    [HTTP("POST", "/game-telemetry/v1/protected/events")]
    public static bool ProtectedEvents(HttpRequest request, ServerStruct serverStruct)
    {
        if (!Directory.Exists("Telemetry")) { Directory.CreateDirectory("Telemetry"); }
        try
        {
            File.WriteAllText("Telemetry/" + DateTime.Now.ToString("s").Replace(":", "-") + ".json", request.Body);
        }
        catch (IOException e)
        {
            Debugger.PrintError(String.Format("Exception occurred while writing telemetry: {0}", e.ToString()));
        }
        serverStruct.Response.MakeOkResponse();
        serverStruct.SendResponse();
        return true;
    }
}

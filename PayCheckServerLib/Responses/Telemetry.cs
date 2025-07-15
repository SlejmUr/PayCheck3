using ModdableWebServer;
using ModdableWebServer.Attributes;
using NetCoreServer;

namespace PayCheckServerLib.Responses;

public class Telemetry
{
    [HTTP("POST", "/game-telemetry/v1/protected/events")]
    public static bool ProtectedEvents(HttpRequest request, ServerStruct serverStruct)
    {
		// Saving telemetry is useless in PayCheck3
        serverStruct.Response.MakeOkResponse();
		serverStruct.Response.SetBody("{}");
        serverStruct.SendResponse();
        return true;
    }
	// https://analytics.starbreeze.com/payday3/v1/events/batch
	[HTTP("POST", "/{telemetryNamespace}/v1/events/batch")]
	public static bool PostBatchTelemetryEvents(HttpRequest request, ServerStruct serverStruct)
	{
		serverStruct.Response.MakeOkResponse();
		serverStruct.Response.SetBody("{}");
		serverStruct.SendResponse();
		return true;
	}
}

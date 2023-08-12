using NetCoreServer;

namespace PayCheckServerLib.Responses
{
    public class Telemetry
    {
        [HTTP("POST", "/game-telemetry/v1/protected/events")]
        public static bool ProtectedEvents(HttpRequest request, PC3Server.PC3Session session)
        {
            if (!Directory.Exists("Telemetry")) { Directory.CreateDirectory("Telemetry"); }
            File.WriteAllText("Telemetry/" + DateTime.Now.ToString("s").Replace(":", "-") + ".json", request.Body);
            session.SendResponse(session.Response.MakeOkResponse());
            return true;
        }
    }
}

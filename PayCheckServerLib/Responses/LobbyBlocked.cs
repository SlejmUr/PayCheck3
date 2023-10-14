using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;

namespace PayCheckServerLib.Responses
{
    public class LobbyBlocked
    {
        [HTTP("GET", "/lobby/v1/public/player/namespaces/{namespace}/users/me/blocked-by")]
        public static bool Blocked(HttpRequest _, ServerStruct serverStruct)
        {
            ResponseCreator response = new();
            response.SetBody("{\r\n    \"data\": []\r\n}");
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("GET", "/lobby/v1/public/player/namespaces/{namespace}/users/me/blocked")]
        public static bool BlockedBy(HttpRequest _, ServerStruct serverStruct)
        {
            ResponseCreator response = new();
            response.SetBody("{\r\n    \"data\": []\r\n}");
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }
    }
}

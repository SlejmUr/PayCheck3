using ModdableWebServer;
using ModdableWebServer.Attributes;
using NetCoreServer;
using Newtonsoft.Json;

namespace PayCheckServerLib.Responses;

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
    public static bool BlockedBy(HttpRequest _, ServerStruct serverStruct) => Blocked(_, serverStruct);


	// https://docs.accelbyte.io/api-explorer/#Lobby%20-%20Friends,%20Presence%20and%20Notifications/UsersPresenceHandlerV1
	class UserPresenceResponse
	{
		[JsonProperty("away")]
		public int Away { get; set; }
		[JsonProperty("busy")]
		public int Busy { get; set; }
		[JsonProperty("invisible")]
		public int Invisible { get; set; }
		[JsonProperty("offline")]
		public int Offline { get; set; }
		[JsonProperty("online")]
		public int Online { get; set; }
		public class PerUserPresenceData
		{
			[JsonProperty("activity")]
			public string Activity { get; set; }
			[JsonProperty("availability")]
			public string Availability { get; set; }
			[JsonProperty("lastSeenAt")]
			public string LastSeenAt { get; set; }
			[JsonProperty("namespace")]
			public string Namespace { get; set; }
			[JsonProperty("platform")]
			public string Platform { get; set; }
			[JsonProperty("userID")]
			public string UserId { get; set; }
		}
		[JsonProperty("data")]
		public List<PerUserPresenceData> Data { get; set; }
	}
	[HTTP("GET", "/lobby/v1/public/presence/namespaces/{namespace}/users/presence?countOnly={countOnly}&userIds={userIds}")]
	public static bool GetUsersPresence(HttpRequest _, ServerStruct serverStruct)
	{
		var response = new ResponseCreator();

		response.SetBody(JsonConvert.SerializeObject(new UserPresenceResponse()));

		serverStruct.Response = response.GetResponse();
		serverStruct.SendResponse();
		return true;
	}
}

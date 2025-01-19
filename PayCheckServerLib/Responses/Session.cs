using ModdableWebServer;
using ModdableWebServer.Attributes;
using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons.Basic;

namespace PayCheckServerLib.Responses
{
    public class Session
    {

		[HTTP("GET", "/session/v1/public/namespaces/pd3/gamesessions")]
		public static bool GetGameSessions(HttpRequest _, ServerStruct serverStruct)
		{
			ResponseCreator response = new ResponseCreator();
			response.SetHeader("Content-Type", "application/json");
			response.SetBody("{\"data\":[]}");
			serverStruct.Response = response.GetResponse();
			serverStruct.SendResponse();
			return true;
		}
		[HTTP("POST", "/session/v1/public/namespaces/pd3/gamesessions")]
		public static bool PostGameSession(HttpRequest _, ServerStruct serverStruct)
		{
			ResponseCreator response = new ResponseCreator();
			response.SetHeader("Content-Type", "application/json");
			response.SetBody("{\"data\":[]}");
			serverStruct.Response = response.GetResponse();
			serverStruct.SendResponse();
			return true;
		}


		[HTTP("GET", "/session/v1/public/namespaces/pd3/recent-player?limit={limit}")]
        public static bool GetRecentPlayersLimit(HttpRequest _, ServerStruct serverStruct)
        {
          ResponseCreator response = new ResponseCreator();
          response.SetHeader("Content-Type", "application/json");
          response.SetBody("{\"data\":[]}");
          serverStruct.Response = response.GetResponse();
          serverStruct.SendResponse();
          return true;
        }

        [HTTP("GET", "/session/v1/public/namespaces/{namespace}/users/me/attributes")]
        public static bool GETSessionAttributes(HttpRequest _, ServerStruct serverStruct)
        {
            var auth = serverStruct.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            ResponseCreator response = new();
            AttribSuccess success = new()
            {
                CrossplayEnabled = true,
                CurrentPlatform = token.PlatformType.ToString().ToUpper(),
                Namespace = token.Namespace,
                Platforms = new()
                {
                    new()
                    {
                        Name = token.PlatformType.ToString().ToUpper(),
                        UserId = token.PlatformId
                    }
                },
                UserId = token.UserId
            };
            response.SetBody(JsonConvert.SerializeObject(success));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("POST", "/session/v1/public/namespaces/{namespace}/users/me/attributes")]
        public static bool POSTSessionAttributes(HttpRequest request, ServerStruct serverStruct)
        {
            var auth = serverStruct.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            var req = JsonConvert.DeserializeObject<AttribRequest>(request.Body) ?? throw new Exception("POSTSessionAttributes -> req is null!");
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            AttribSuccess success = new()
            {
                CrossplayEnabled = req.CrossplayEnabled,
                CurrentPlatform = req.CurrentPlatform,
                Namespace = token.Namespace,
                Platforms = req.Platforms,
                UserId = token.UserId
            };
            response.SetBody(JsonConvert.SerializeObject(success));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("GET", "/session/v1/public/namespaces/{namespace}/users/me/parties")]
        public static bool SessionsParties(HttpRequest _, ServerStruct serverStruct)
        {
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            DataPaging<object> gamesessions = new()
            {
                Paging = new()
                {
                    First = "",
                    Last = "",
                    Previous = "",
                    Next = ""
                },
                Data = new()
            };
            response.SetBody(JsonConvert.SerializeObject(gamesessions));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("GET", "/session/v1/public/namespaces/{namespace}/users/me/gamesessions")]
        public static bool SessionsUsersMeGamesessions(HttpRequest _, ServerStruct serverStruct)
        {
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            DataPaging<object> gamesessions = new()
            {
                Paging = new()
                {
                    First = "",
                    Last = "",
                    Previous = "",
                    Next = ""
                },
                Data = new()
            };
            response.SetBody(JsonConvert.SerializeObject(gamesessions));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }


        [HTTP("GET", "/session/v1/public/namespaces/{namespace}/gamesessions")]
        public static bool Sessionsgamesessions(HttpRequest _, ServerStruct serverStruct)
        {
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            DataPaging<object> gamesessions = new()
            {
                Paging = new()
                {
                    First = "",
                    Last = "",
                    Previous = "",
                    Next = ""
                },
                Data = new()
            };
            response.SetBody(JsonConvert.SerializeObject(gamesessions));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }


		// https://docs.accelbyte.io/api-explorer/#Session/publicGetBulkPlayerCurrentPlatform
		class GetPlayersCurrentPlatformInBulkRequestBody
		{
			[JsonProperty("userIDs")]
			public List<string> UserIds { get; set; }
		}
		class GetPlayersCurrentPlatformInBulkResponseBody
		{
			public class UserPlatformData
			{
				[JsonProperty("crossplayEnabled")]
				public bool CrossplayEnabled { get; set; }
				[JsonProperty("currentPlatform")]
				public string CurrentPlatform { get; set; }
				[JsonProperty("userID")]
				public string UserId { get; set; }
			}
		}
		[HTTP("POST", "/session/v1/public/namespaces/{namespace}/users/bulk/platform")]
		public static bool GetPlayersCurrentPlatformInBulk(HttpRequest _, ServerStruct serverStruct)
		{
			var playersToRequest = JsonConvert.DeserializeObject<GetPlayersCurrentPlatformInBulkRequestBody>(_.Body);

			var responseData = new DataPaging<GetPlayersCurrentPlatformInBulkResponseBody.UserPlatformData>();

			responseData.Data = new();
			foreach (var userId in playersToRequest.UserIds)
			{
				responseData.Data.Add(new()
				{
					CrossplayEnabled = true,
					CurrentPlatform = "STEAM",
					UserId = userId
				});
			}

			var response = new ResponseCreator();

			response.SetBody(JsonConvert.SerializeObject(responseData));
			serverStruct.Response = response.GetResponse();
			serverStruct.SendResponse();
			return true;
		}
	}
}

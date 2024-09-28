using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses;

public class BasicResponse
{
	[HTTP("GET", "/basic/v1/public/namespaces/{namespace}/profiles/public?userIds={userId}")]
	public static bool GetUserProfile(HttpRequest _, ServerStruct serverStruct)
	{
		ResponseCreator response = new ResponseCreator();
		response.SetHeader("Content-Type", "application/json");

		var resp = new Profile.ProfileResp()
		{
			UserId = serverStruct.Parameters["userId"],
			Namespace = serverStruct.Parameters["namespace"],
			PublicId = "AAAAAAAA"
		};

		response.SetBody(JsonConvert.SerializeObject(resp));
		serverStruct.Response = response.GetResponse();
		serverStruct.SendResponse();
		return true;
	}

	[HTTP("POST", "/basic/v1/public/namespaces/{namespace}/users/me/profiles")]
	public static bool PostCurrentUserProfile(HttpRequest _, ServerStruct serverStruct) => GetCurrentUserProfile(_, serverStruct);

	[HTTP("GET", "/basic/v1/public/namespaces/{namespace}/users/me/profiles")]
	public static bool GetCurrentUserProfile(HttpRequest _, ServerStruct serverStruct)
	{
		ResponseCreator response = new ResponseCreator();
		response.SetHeader("Content-Type", "application/json");

		var resp = new Profile.ProfileResp()
		{
			UserId = TokenHelper.ReadToken(serverStruct.Headers["authorization"].Split(" ")[1]).UserId,
        Namespace = serverStruct.Parameters["namespace"],
        PublicId = "AAAAAAAA"
		};

		response.SetBody(JsonConvert.SerializeObject(resp));
		serverStruct.Response = response.GetResponse();
		serverStruct.SendResponse();
		return true;
	}

	[HTTP("HEAD", "/generate_204")]
    public static bool Generate204(HttpRequest _, ServerStruct serverStruct)
    {
        serverStruct.Response.MakeOkResponse(204);
        serverStruct.SendResponse();
        return true;
    }


    [HTTP("GET", "/qosm/public/namespaces/pd3/qos?status=ACTIVE")]
    public static bool GetActiveQOS(HttpRequest _, ServerStruct serverStruct) => QOSM_Public_QOS(_, serverStruct);

    [HTTP("GET", "/qosm/public/qos")]
    public static bool QOSM_Public_QOS(HttpRequest _, ServerStruct serverStruct)
    {
        ResponseCreator response = new ResponseCreator();
        response.SetHeader("Content-Type", "application/json");
        response.SetHeader("Connection", "keep-alive");
        var rsp = new JsonServers()
        {
            Servers = new()
        };
        foreach (var server in ConfigHelper.ServerConfig.DS_Servers)
        {
            rsp.Servers.Add(new()
            {
                Alias = server.Alias,
                Status = server.Status,
                Ip = server.Ip,
                LastUpdate = "2023-08-06T10:00:00.000000000Z",
                Port = server.Port,
                Region = server.Region
            });
        }

        response.SetBody(JsonConvert.SerializeObject(rsp, Formatting.Indented));
        serverStruct.Response = response.GetResponse();
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("GET", "/basic/v1/public/misc/time")]
    public static bool Time(HttpRequest _, ServerStruct serverStruct)
    {
        if (serverStruct.Headers.ContainsKey("cookie"))
        {
            var tokens = TokenHelper.ReadFromHeader(serverStruct.Headers);
            Debugger.PrintDebug($"{tokens.AccessToken.UserId}({tokens.AccessToken.Name}) Is still in the server!");
        }

        ResponseCreator response = new ResponseCreator();
        response.SetHeader("Content-Type", "application/json");
        response.SetHeader("Connection", "keep-alive");
        var rsp = new Time()
        {
            CurrentTime = DateTime.UtcNow.ToString("o")
        };
        response.SetBody(JsonConvert.SerializeObject(rsp));
        serverStruct.Response = response.GetResponse();
        serverStruct.SendResponse();
        return true;
    }



    [HTTP("GET", "/lobby/v1/messages")]
    public static bool LobbyMessages(HttpRequest _, ServerStruct serverStruct)
    {
        ResponseCreator response = new ResponseCreator();
        response.SetHeader("Content-Type", "application/json");
        response.SetBody(File.ReadAllBytes("Files/messages.json"));
        serverStruct.Response = response.GetResponse();
        serverStruct.SendResponse();
        return true;
    }


    [HTTP("GET", "/iam/v3/location/country")]
    public static bool Country(HttpRequest _, ServerStruct serverStruct)
    {
        ResponseCreator response = new ResponseCreator();
        response.SetHeader("Content-Type", "application/json");
        response.SetBody(File.ReadAllBytes("Files/Country.json"));
        serverStruct.Response = response.GetResponse();
        serverStruct.SendResponse();
        return true;
    }
}

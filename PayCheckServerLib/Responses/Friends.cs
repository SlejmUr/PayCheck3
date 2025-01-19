using ModdableWebServer;
using ModdableWebServer.Attributes;
using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses;

public class Friends
{
    [HTTP("GET", "/friends/namespaces/{namespace}/me/platforms")]
    public static bool MePlatforms(HttpRequest _, ServerStruct serverStruct)
    {
        var auth = serverStruct.Headers["authorization"].Replace("Bearer ", "");
        var token = TokenHelper.ReadToken(auth);
		var MainUser = UserController.GetUser(token.UserId, token.Namespace);

        ResponseCreator response = new();
		if (MainUser == null)
		{
			return serverStruct.ReturnErrorHelper(ErrorHelper.Errors.ValidationError);
		}

        response.SetHeader("Content-Type", "application/json");
        FriendsPlatfrom friends = new FriendsPlatfrom()
        {
            Data = MainUser.Friends,
        };
        response.SetBody(JsonConvert.SerializeObject(friends));
        serverStruct.Response = response.GetResponse();
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("GET", "/friends/namespaces/{namespace}/me/platforms?limit={limit}&offset={offset}")]
    public static bool MePlatformsLimitOffset(HttpRequest _, ServerStruct serverStruct) => MePlatforms(_, serverStruct);

    [HTTP("POST", "/friends/namespaces/{namespace}/users/{userId}/add/bulk")]
    public static bool FriendAddBulk(HttpRequest request, ServerStruct serverStruct)
    {
        var auth = serverStruct.Headers["authorization"].Replace("Bearer ", "");
        var token = TokenHelper.ReadToken(auth);
        var MainUser = UserController.GetUser(token.UserId, token.Namespace);

		var response = new ResponseCreator(204);
		if (MainUser == null)
		{
			return serverStruct.ReturnErrorHelper(ErrorHelper.Errors.ValidationError);
		}

        var friends = JsonConvert.DeserializeObject<FriendAdd>(request.Body)!.FriendIds;

        //  Add func to UserC. for adding and checking friends infomation.
        foreach (var item in friends)
        {
            var user = UserController.GetUser(item, token.Namespace);
            if (user == null)
            {
                Debugger.PrintWarn($"UserId {item} not found in users!");
                continue;
            }

            FriendsPlatfrom.FriendsPlatfromData data = new()
            {
                AvatarUrl = user.UserData.AvatarUrl,
                DisplayName = user.UserData.DisplayName,
                UserId = user.UserData.UserId,
                Username = user.UserData.DisplayName,
                PlatformInfos = new()
            };

            foreach (var pids in user.UserData.PlatformUserIds)
            {
                data.PlatformInfos.Add(new()
                {
                    PlatformDisplayName = user.UserData.DisplayName,
                    PlatformName = pids.Key,
                    PlatformUserId = pids.Value
                });
            }
            MainUser.Friends.Add(data);
        }

        serverStruct.Response = response.GetResponse();
        serverStruct.SendResponse();
        return true;
    }
}

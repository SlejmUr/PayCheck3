using ModdableWebServer;
using ModdableWebServer.Attributes;
using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.Basic;

namespace PayCheckServerLib.Responses;

public class Challenge
{
    [HTTP("GET", "/challenge/v1/public/namespaces/{namespace}/users/{userId}/eligibility")]
    public static bool ChallengeEligibility(HttpRequest _, ServerStruct serverStruct)
    {
        ResponseCreator creator = new();
        creator.SetBody("{\n    \"isComply\": true\n}");
        serverStruct.Response = creator.GetResponse();
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("GET", "/challenge/v1/public/namespaces/{namespace}/users/me/records?limit={limit}&offset={offset}")]
    public static bool ChallengeRecordsSplit(HttpRequest _, ServerStruct serverStruct)
    {
        return false;
        var offset = int.Parse(serverStruct.Parameters["offset"]);
        var limit = int.Parse(serverStruct.Parameters["limit"]);
        var auth = serverStruct.Headers["authorization"].Replace("Bearer ", "");
        var token = TokenHelper.ReadToken(auth);
        ResponseCreator creator = new();
        var challenges = JsonConvert.DeserializeObject<DataPaging<ChallengesData>>(File.ReadAllText("Files/ChallengeRecords.json")) ?? throw new Exception("ChallengeRecords is null!");
        challenges.Data.Skip(offset).Take(limit);
        foreach (var item in challenges.Data)
        {
            item.UserId = token.UserId;
            item.RecordId = token.UserId;
            item.UpdatedAt = TimeHelper.GetOTime();
        }

        creator.SetBody(JsonConvert.SerializeObject(challenges));
        serverStruct.Response = creator.GetResponse();
        serverStruct.SendResponse();
        return true;
    }
}

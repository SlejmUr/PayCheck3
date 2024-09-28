using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.Basic;

namespace PayCheckServerLib.Responses;

public class Entitlements
{
    [HTTP("GET", "/platform/public/namespaces/{namespace}/users/{userId}/entitlements?limit={limit}")]
    public static bool GetUserEntitlements(HttpRequest _, ServerStruct serverStruct)
    {
        try
        {
            var time = TimeHelper.GetZTime();
            var responsecreator = new ResponseCreator();
            var entitlements = JsonConvert.DeserializeObject<DataPaging<EntitlementsData>>(File.ReadAllText("./Files/Entitlements.json")) ?? throw new Exception("Entitlements is null!");
            var newentitlements = new List<EntitlementsData>();
            foreach (var entitlement in entitlements.Data)
            {
                entitlement.UserId = serverStruct.Parameters["userId"];
                entitlement.UpdatedAt = time;
                newentitlements.Add(entitlement);
            }
            DataPaging<EntitlementsData> payload = new()
            {
                Data = newentitlements,
                Paging = new()
            };
            responsecreator.SetBody(JsonConvert.SerializeObject(payload));
            serverStruct.Response = responsecreator.GetResponse();
            serverStruct.SendResponse();
            return true;
        }
        catch (Exception ex)
        {
            Debugger.PrintError(ex.ToString());
        }
        return false;
    }

    [HTTP("GET", "/platform/public/namespaces/{namespace}/users/{userId}/entitlements?itemId={itemid}&limit={limit}")]
    public static bool GetUserEntitlementsByItemId(HttpRequest _, ServerStruct serverStruct)
    {
        try
        {
            var time = TimeHelper.GetZTime();
            var responsecreator = new ResponseCreator();
            var entitlements = JsonConvert.DeserializeObject<DataPaging<EntitlementsData>>(File.ReadAllText("./Files/Entitlements.json")) ?? throw new Exception("Entitlements is null!");
            var newentitlements = new List<EntitlementsData>();
            foreach (var entitlement in entitlements.Data)
            {
                if (entitlement.ItemId == serverStruct.Parameters["itemid"])
                {
                    entitlement.UserId = serverStruct.Parameters["userId"];
                    entitlement.UpdatedAt = time;
                    newentitlements.Add(entitlement);
                }

            }
            DataPaging<EntitlementsData> payload = new()
            {
                Data = newentitlements,
                Paging = new()
            };
            responsecreator.SetBody(JsonConvert.SerializeObject(payload));
            serverStruct.Response = responsecreator.GetResponse();
            serverStruct.SendResponse();
            return true;
        }
        catch (Exception ex)
        {
            Debugger.PrintError(ex.ToString());
        }
        return false;
    }
}

using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.Basic;

namespace PayCheckServerLib.Responses;

public class Items
{
    [HTTP("GET", "/platform/public/namespaces/{namespace}/items/byCriteria?limit={limit}&includeSubCategoryItem=false")]
    public static bool GetItemsByCriteria(HttpRequest _, ServerStruct serverStruct)
    {
        ResponseCreator creator = new();
        var items = JsonConvert.DeserializeObject<DataPaging<ItemDefinitionJson>>(File.ReadAllText("./Files/Items.json"));
        var timeMrFreeman = DateTime.UtcNow.ToString("o");
        foreach (var item in items.Data)
        {
            item.UpdatedAt = timeMrFreeman;
        }
        creator.SetBody(JsonConvert.SerializeObject(items));
        serverStruct.Response = creator.GetResponse();
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("GET", "/platform/public/namespaces/{namespace}/items/byCriteria?tags={tags}&limit={limit}&includeSubCategoryItem=false")]
    public static bool GetItemsByCriteriaByTags(HttpRequest _, ServerStruct serverStruct)
    {
        ResponseCreator creator = new();
        var items = JsonConvert.DeserializeObject<DataPaging<ItemDefinitionJson>>(File.ReadAllText("./Files/Items.json")) ?? throw new Exception("Items is null!");
        var finalitems = new List<ItemDefinitionJson>();
        var timeMrFreeman = DateTime.UtcNow.ToString("o");
        foreach (var item in items.Data)
        {
            item.UpdatedAt = timeMrFreeman;
            if (item.Tags != null)
            {
                if (item.Tags.Contains(serverStruct.Parameters["tags"]))
                {
                    finalitems.Add(item);
                }
            }
        }
        DataPaging<ItemDefinitionJson> tosend = new()
        {
            Data = finalitems,
            Paging = { }
        };
        creator.SetBody(JsonConvert.SerializeObject(tosend));
        serverStruct.Response = creator.GetResponse();
        serverStruct.SendResponse();
        return true;
    }
}

using ModdableWebServer;
using ModdableWebServer.Attributes;
using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.Basic;
using PayCheckServerLib.ModdableWebServerExtensions;

namespace PayCheckServerLib.Responses;

public class Items
{
    [HTTP("GET", "/platform/public/namespaces/{namespace}/items/byCriteria?limit={limit}&includeSubCategoryItem=false")]
	[AuthenticationRequired("", AuthenticationRequiredAttribute.Access.None)]
    public static bool GetItemsByCriteria(HttpRequest _, ServerStruct serverStruct)
    {
        ResponseCreator creator = new();
		var items = UserEntitlementHelper.GetItemDefinitions();
        var timeMrFreeman = DateTime.UtcNow.ToString("o");
        foreach (var item in items)
        {
            item.UpdatedAt = timeMrFreeman;
        }

        creator.SetBody(JsonConvert.SerializeObject(
			new DataPaging<ItemDefinitionJson>()
			{
				Data = items
			}
			));
        serverStruct.Response = creator.GetResponse();
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("GET", "/platform/public/namespaces/{namespace}/items/byCriteria?tags={tags}&limit={limit}&includeSubCategoryItem=false")]
	[AuthenticationRequired("", AuthenticationRequiredAttribute.Access.None)]
    public static bool GetItemsByCriteriaByTags(HttpRequest _, ServerStruct serverStruct)
    {
        ResponseCreator creator = new();
        var items = UserEntitlementHelper.GetItemDefinitions();
		var finalitems = new List<ItemDefinitionJson>();
        var timeMrFreeman = DateTime.UtcNow.ToString("o");
        foreach (var item in items)
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

		creator.SetBody(JsonConvert.SerializeObject(
			new DataPaging<ItemDefinitionJson>()
			{
				Data = items
			}
			));
		serverStruct.Response = creator.GetResponse();
        serverStruct.SendResponse();
        return true;
    }
}

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
    [HTTP("GET", "/platform/public/namespaces/{namespace}/items/byCriteria?offset={offset}&limit={limit}&includeSubCategoryItem=false")]
	[AuthenticationRequired("", AuthenticationRequiredAttribute.Access.None)]
	public static bool GetItemsByCriteriaOffset(HttpRequest _, ServerStruct serverStruct)
	{
		ResponseCreator creator = new();
		var items = UserEntitlementHelper.GetItemDefinitions();

		var limitedItems = new List<ItemDefinitionJson>();

		int offset = 0;
		int limit = 0;
		int.TryParse(serverStruct.Parameters["offset"], out offset);
		int.TryParse(serverStruct.Parameters["limit"], out limit);

		for (int i = offset; i < offset + limit; i++)
		{
			if (i < items.Count)
			{
				limitedItems.Add(items[i]);
			}
		}

		var timeMrFreeman = DateTime.UtcNow.ToString("o");
		foreach (var item in limitedItems)
		{
			item.UpdatedAt = timeMrFreeman;
		}

		creator.SetBody(JsonConvert.SerializeObject(
			new DataPaging<ItemDefinitionJson>()
			{
				Data = limitedItems
			}
			));
		serverStruct.Response = creator.GetResponse();
		serverStruct.SendResponse();
		return true;
	}
    [HTTP("GET", "/platform/public/namespaces/{namespace}/items/byCriteria?limit={limit}&includeSubCategoryItem=false")]
	[AuthenticationRequired("", AuthenticationRequiredAttribute.Access.None)]
    public static bool GetItemsByCriteria(HttpRequest _, ServerStruct serverStruct)
    {
        ResponseCreator creator = new();
		var items = UserEntitlementHelper.GetItemDefinitions();

		var limitedItems = new List<ItemDefinitionJson>();

		int limit = 0;
		int.TryParse(serverStruct.Parameters["limit"], out limit);

		for (int i = 0; i < limit; i++)
		{
			if (i < items.Count)
			{
				limitedItems.Add(items[i]);
			}
		}

        var timeMrFreeman = DateTime.UtcNow.ToString("o");
        foreach (var item in limitedItems)
        {
            item.UpdatedAt = timeMrFreeman;
        }

        creator.SetBody(JsonConvert.SerializeObject(
			new DataPaging<ItemDefinitionJson>()
			{
				Data = limitedItems
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

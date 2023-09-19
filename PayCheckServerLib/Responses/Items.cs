using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses
{
    public class Items
    {
        [HTTP("GET", "/platform/public/namespaces/pd3/items/byCriteria?limit={limit}&includeSubCategoryItem=false")]
        public static bool GetItemsByCriteria(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator creator = new();
            var items = JsonConvert.DeserializeObject<DataPaging<ItemDefinitionJson>>(File.ReadAllText("./Files/Items.json"));
            var timeMrFreeman = DateTime.UtcNow.ToString("o");
            foreach (var item in items.Data)
            {
                item.UpdatedAt = timeMrFreeman;
            }
            creator.SetBody(JsonConvert.SerializeObject(items));
            session.SendResponse(creator.GetResponse());
            return true;
        }

        [HTTP("GET", "/platform/public/namespaces/pd3/items/byCriteria?tags=WeaponPart&limit={limit}&includeSubCategoryItem=false")]
        public static bool GetItemsByCriteriaWeaponPart(HttpRequest _, PC3Server.PC3Session session)
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
                    if (item.Tags.Contains("WeaponPart"))
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
            session.SendResponse(creator.GetResponse());
            return true;
        }
    }
}

using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses 
{
	public class Items 
	{
		[HTTP("GET", "/platform/public/namespaces/pd3beta/items/byCriteria?limit=1000&includeSubCategoryItem=false")]
		public static bool GetItemsByCriteria(HttpRequest request, PC3Server.PC3Session session) 
		{
			ResponseCreator creator = new ResponseCreator();

			creator.SetBody(File.ReadAllText("./Files/Items.json"));
			session.SendResponse(creator.GetResponse());
			return true;
		}

		[HTTP("GET", "/platform/public/namespaces/pd3beta/items/byCriteria?tags=WeaponPart&limit=1000&includeSubCategoryItem=false")]
		public static bool GetItemsByCriteriaWeaponPart(HttpRequest request, PC3Server.PC3Session session) 
		{
			ResponseCreator creator = new ResponseCreator();
			var items = JsonConvert.DeserializeObject<ItemsJson>(File.ReadAllText("./Files/Items.json"));
			var finalitems = new List<ItemDefinitionJson>();
			foreach(var item in items.Data ) 
			{
				if (item.Tags != null) 
				{
					if (item.Tags.Contains("WeaponPart")) 
					{
						finalitems.Add(item);
					}
				}
			}
			var tosend = new ItemsJson()
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

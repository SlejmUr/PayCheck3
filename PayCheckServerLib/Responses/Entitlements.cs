using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib.Responses {
	public class Entitlements {
		[HTTP("GET", "/platform/public/namespaces/pd3beta/users/{userId}/entitlements?limit=1000")]
		public static bool GetUserEntitlements(HttpRequest request, PC3Server.PC3Session session) {
			var responsecreator = new ResponseCreator();
			var entitlements = JsonConvert.DeserializeObject<EntitlementPayloadJson>(File.ReadAllText("./Files/Entitlements.json"));
			var newentitlements = new List<EntitlementsData>();
			foreach(var entitlement in entitlements.Data) {
				entitlement.UserId = session.HttpParam["userId"];
				newentitlements.Add(entitlement);
			}
			var payload = new EntitlementPayloadJson() {
				Data = newentitlements.ToArray(),
				Paging = { }
			};
			responsecreator.SetBody(JsonConvert.SerializeObject(payload));
			session.SendResponse(responsecreator.GetResponse());
			return true;
		}
	}
}

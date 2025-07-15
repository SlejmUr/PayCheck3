using ModdableWebServer;
using ModdableWebServer.Attributes;
using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.Basic;
using PayCheckServerLib.ModdableWebServerExtensions;

namespace PayCheckServerLib.Responses;

public class Entitlements
{
    [HTTP("GET", "/platform/public/namespaces/{namespace}/users/{userId}/entitlements?limit={limit}")]
	[AuthenticationRequired("NAMESPACE:{namespace}:USER:{userId}:ENTITLEMENT", AuthenticationRequiredAttribute.Access.READ)]
    public static bool GetUserEntitlements(HttpRequest _, ServerStruct serverStruct)
    {
		var responsecreator = new ResponseCreator();
		DataPaging<EntitlementsData> payload = new()
		{
			Data = UserEntitlementHelper.GetEntitlementDataForUser(serverStruct.Parameters["userId"], false),
			Paging = new()
		};

		responsecreator.SetBody(JsonConvert.SerializeObject(payload));
		
		serverStruct.Response = responsecreator.GetResponse();
		serverStruct.SendResponse();
		return true;
    }

    [HTTP("GET", "/platform/public/namespaces/{namespace}/users/{userId}/entitlements?itemId={itemId}&limit={limit}")]
	[AuthenticationRequired("NAMESPACE:{namespace}:USER:{userId}:ENTITLEMENT", AuthenticationRequiredAttribute.Access.READ)]
    public static bool GetUserEntitlementsByItemId(HttpRequest _, ServerStruct serverStruct)
    {
		var filteredEntitlements = new List<EntitlementsData>();
		var userEntitlements = UserEntitlementHelper.GetEntitlementDataForUser(serverStruct.Parameters["userId"], false);

		foreach (var entitlement in userEntitlements)
		{
			if(entitlement.ItemId == serverStruct.Parameters["itemId"])
				filteredEntitlements.Add(entitlement);
		}

		DataPaging<EntitlementsData> payload = new()
		{
			Data = filteredEntitlements,
			Paging = new()
		};

		var response = new ResponseCreator();
		response.SetBody(JsonConvert.SerializeObject(payload));
		serverStruct.Response = response.GetResponse();
		serverStruct.SendResponse();

        return true;
    }

	[HTTP("PUT", "/platform/public/namespaces/{namespace}/users/{userId}/entitlements/{entitlementId}/decrement")]
	[AuthenticationRequired("NAMESPACE:{namespace}:USER:{userId}:ENTITLEMENT", AuthenticationRequiredAttribute.Access.UPDATE)]
	public static bool DecrementUserEntitlementByEntitlementId(HttpRequest _, ServerStruct serverStruct)
	{
		var responsecreator = new ResponseCreator();

		var entitlements = UserEntitlementHelper.GetEntitlementDataForUser(serverStruct.Parameters["userId"], false);

		var entitlementToDecrement = entitlements.Find(data => data.Id == serverStruct.Parameters["entitlementId"]);
		if (entitlementToDecrement == null)
		{
			Debugger.PrintError("Unable to find entitlement with id {0}", serverStruct.Parameters["entitlementId"]);
			return false;
		}

		Debugger.PrintDebug(String.Format("Game is attempted to decrement entitlement with the sku {0}, not gonna do anything though :)", entitlementToDecrement.Sku));

		responsecreator.SetBody(JsonConvert.SerializeObject(entitlementToDecrement));
		responsecreator.SetHeader("Content-Type", "application/json");
		serverStruct.Response = responsecreator.GetResponse();
		serverStruct.SendResponse();

		return true;
	}
}

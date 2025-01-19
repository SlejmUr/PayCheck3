using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.Basic;
using ModdableWebServer;
using ModdableWebServer.Attributes;

namespace PayCheckServerLib.Responses
{
	public class Social
	{

		[HTTP("GET", "/social/v1/public/namespaces/{namespace}/users/{userId}/statitems?limit={limit}&sortBy={sortBy}")]
		[AuthenticationRequired("NAMESPACE:{namespace}:USER:{userId}:STATITEM", AuthenticationRequiredAttribute.Access.READ)]
		public static bool ListAllStatItemsByPagination(HttpRequest _, ServerStruct serverStruct)
		{
			var userId = serverStruct.Parameters["userId"];

			var sort = serverStruct.Parameters["sortBy"];

			var limit = -1;
			if (!int.TryParse(serverStruct.Parameters["limit"], out limit))
			{
				return serverStruct.ReturnErrorHelper(ErrorHelper.Errors.ValidationError);
			}

			var sortBy = "";
			var descending = false;
			if (sort.Contains(":"))
			{
				var split = sort.Split(':');
				if (split.Length > 1)
				{
					sortBy = split[0];
					descending = split[1] == "desc";
				}
			}
			else
			{
				sortBy = sort;
			}

			var allStatItems = CloudSaveDataHelper.GetAllStatItemsForUser(userId);

			if (descending)
			{
				switch (sortBy)
				{
					case "statCode":
						allStatItems.Sort((statItemA, statItemB) => statItemA.StatCode.CompareTo(statItemB.StatCode));
						break;
					case "createdAt":
						allStatItems.Sort((statItemA, statItemB) => statItemA.CreatedAt.CompareTo(statItemB.CreatedAt));
						break;
					case "updatedAt":
						allStatItems.Sort((statItemA, statItemB) => statItemA.UpdatedAt.CompareTo(statItemB.UpdatedAt));
						break;
					default:
						break;
				}
			}
			else
			{
				switch (sortBy)
				{
					case "statCode":
						allStatItems.Sort((statItemA, statItemB) => statItemB.StatCode.CompareTo(statItemA.StatCode));
						break;
					case "createdAt":
						allStatItems.Sort((statItemA, statItemB) => statItemB.CreatedAt.CompareTo(statItemA.CreatedAt));
						break;
					case "updatedAt":
						allStatItems.Sort((statItemA, statItemB) => statItemB.UpdatedAt.CompareTo(statItemA.UpdatedAt));
						break;
					default:
						break;
				}
			}


			var response = new ResponseCreator();
			DataPaging<UserStatItemsData> responseData = new()
			{
				Data = allStatItems,
			};
			response.SetHeader("Content-Type", "application/json");
			response.SetBody(JsonConvert.SerializeObject(responseData));
			serverStruct.Response = response.GetResponse();
			serverStruct.SendResponse();
			return true;
		}

		[HTTP("PUT", "/social/v1/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk")]
		[AuthenticationRequired("NAMESPACE:{namespace}:USER:{userId}:STATITEM", AuthenticationRequiredAttribute.Access.UPDATE)]
		public static bool BulkUpdateUserStatItems(HttpRequest _, ServerStruct serverStruct)
		{
			var updateRequests = JsonConvert.DeserializeObject<List<BulkStatItemUpdateRequestData>>(_.Body);

			if (updateRequests == null)
				return serverStruct.ReturnErrorHelper(ErrorHelper.Errors.ValidationError);

			List<BulkStatItemUpdateResponseData> responseData = new();

			List<UserStatItemsData> userStatsForRewardChecking = new();
			foreach (var request in updateRequests)
			{
				if (request.Inc == null)
					continue;
				if (request.StatCode == null)
					continue;

				CloudSaveDataHelper.IncrementStatItemValueForUser(serverStruct.Parameters["namespace"], serverStruct.Parameters["userId"], request.StatCode, request.Inc.Value, out UserStatItemsData? updatedData);


				var respData = new BulkStatItemUpdateResponseData();

				respData.StatCode = request.StatCode;
				respData.Success = updatedData != null;

				if (updatedData != null)
				{
					respData.Details = new BulkStatItemUpdateResponseData.BulkStatItemUpdateResponseDataDetails();
					respData.Details.CurrentValue = updatedData.Value;

					userStatsForRewardChecking.Add(updatedData);
				}

				responseData.Add(respData);

			}
			UserEntitlementHelper.CheckForRewardsOnStatItemUpdate(serverStruct.Parameters["userId"], userStatsForRewardChecking);

			var response = new ResponseCreator();
			response.SetHeader("Content-Type", "application/json");
			response.SetBody(JsonConvert.SerializeObject(responseData));

			serverStruct.Response = response.GetResponse();
			serverStruct.SendResponse();

			return true;
		}
	}
}
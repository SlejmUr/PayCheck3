using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.Basic;
using ModdableWebServer;
using ModdableWebServer.Attributes;
using PayCheckServerLib.ModdableWebServerExtensions;
using PayCheckServerLib.Jsons.CloudSave;
using Newtonsoft.Json.Linq;

namespace PayCheckServerLib.Responses
{
	public class CloudSave
	{

		private static object GetWeaponRecordValue(string recordKey)
		{
			var weaponTables = CloudSaveDataHelper.GetStaticData<List<CloudSaveDataWrapper<object>>>("WeaponTables.json");

			var table = weaponTables.Find(obj => obj.Key == recordKey);

			if (table == null)
			{
				table = weaponTables[0];
			}
			return table.Value;
		}

		[HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/records/{recordtype}")]
		[AuthenticationRequired("NAMESPACE:{namespace}:CLOUDSAVE:RECORD", AuthenticationRequiredAttribute.Access.READ)]
		public static bool MainRecordsSplitter(HttpRequest request, ServerStruct serverStruct)
		{
			if (serverStruct.Parameters["recordtype"].Contains("weapon-translation-table-"))
			{
				serverStruct.Parameters.Add("weapon", serverStruct.Parameters["recordtype"].Replace("weapon-translation-table-", ""));
				return GetWeaponGameRecord(request, serverStruct);
			}

			object data = CloudSaveDataHelper.GetGlobalCloudSaveData(serverStruct.Parameters["recordtype"]);
			if (data != null)
			{
				ResponseCreator response = new();
				CloudSaveDataWrapper<object> wrappedCloudsaveData = new()
				{
					Value = data,
					Namespace = serverStruct.Parameters["namespace"],
					Key = serverStruct.Parameters["recordtype"],
					SetBy = "SERVER",
					CreatedAt = "0001-01-01T01:01:01.001Z00:00",
					UpdatedAt = "0001-01-01T01:01:01.001Z00:00"
				};
				response.SetHeader("Content-Type", "application/json");
				response.SetBody(JsonConvert.SerializeObject(wrappedCloudsaveData));
				serverStruct.Response = response.GetResponse();
				serverStruct.SendResponse();
				return true;
			}
			Debugger.PrintError(string.Format("Unknown cloudsave item: {0}", serverStruct.Parameters["recordtype"]));
			return false;
		}


		[HTTP("POST", "/cloudsave/v1/namespaces/{namespace}/records/bulk")]
		[AuthenticationRequired("NAMESPACE:{namespace}:CLOUDSAVE:RECORD", AuthenticationRequiredAttribute.Access.READ)]
		public static bool RecordsBulk(HttpRequest request, ServerStruct serverStruct)
		{
			var req = JsonConvert.DeserializeObject<BulkCloudSaveRequestData>(request.Body);

			var responseData = new BulkCloudSaveRequestResponseData();
			responseData.Data = new();

			foreach (string key in req.Keys) {
				if (key.StartsWith("weapon-translation-table-"))
				{
					responseData.Data.Add(new()
					{
						Value = GetWeaponRecordValue(key),
						Namespace = serverStruct.Parameters["namespace"],
						Key = key,
						SetBy = "SERVER",
						CreatedAt = "0001-01-01T01:01:01.001Z00:00",
						UpdatedAt = "0001-01-01T01:01:01.001Z00:00"
					});
				}
				else
				{
					object keyData = CloudSaveDataHelper.GetGlobalCloudSaveData(key);
					if (keyData != null)
					{
						responseData.Data.Add(new()
						{
							Value = keyData,
							Namespace = serverStruct.Parameters["namespace"],
							Key = key,
							SetBy = "SERVER",
							CreatedAt = "0001-01-01T01:01:01.001Z00:00",
							UpdatedAt = "0001-01-01T01:01:01.001Z00:00"
						}
						);
					}
				}
			}

			var response = new ResponseCreator();
			response.SetHeader("Content-Type", "application/json");
			response.SetBody(JsonConvert.SerializeObject(responseData));
			serverStruct.Response = response.GetResponse();
			serverStruct.SendResponse();
			return true;
		}
		public static bool GetWeaponGameRecord(HttpRequest _, ServerStruct serverStruct)
		{
			ResponseCreator response = new();

			CloudSaveDataWrapper<object> res = new()
			{
				Namespace = serverStruct.Parameters["namespace"],
				Key = "weapon-translation-table-" + serverStruct.Parameters["weapon"],
				SetBy = "SERVER",
				Value = GetWeaponRecordValue("weapon-translation-table-" + serverStruct.Parameters["weapon"]),
				CreatedAt = "0001-01-01T01:01:01.001Z00:00",
				UpdatedAt = "0001-01-01T01:01:01.001Z00:00"
			};
			response.SetHeader("Content-Type", "application/json");
			response.SetBody(JsonConvert.SerializeObject(res));
			serverStruct.Response = response.GetResponse();
			serverStruct.SendResponse();
			return true;
		}

		// TODO: implement public checking
		[HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/users/{userId}/records/{key}/public")]
		[AuthenticationRequired("NAMESPACE:{namespace}:USER:{userId}:PUBLIC:CLOUDSAVE:RECORD", AuthenticationRequiredAttribute.Access.READ)]
		public static bool GetPublicUserCloudSaveEntry(HttpRequest _, ServerStruct serverStruct) => GetUserCloudSaveEntry(_, serverStruct);

		[HTTP("PUT", "/cloudsave/v1/namespaces/{namespace}/users/{userId}/records/{key}/public")]
		[AuthenticationRequired("NAMESPACE:{namespace}:USER:{userId}:PUBLIC:CLOUDSAVE:RECORD", AuthenticationRequiredAttribute.Access.UPDATE)]
		public static bool CreateOrReplacePublicUserCloudSaveEntry(HttpRequest _, ServerStruct serverStruct) => CreateOrReplaceUserCloudSaveEntry(_, serverStruct);

		[HTTP("POST", "/cloudsave/v1/namespaces/{namespace}/users/{userId}/records/{key}/public")]
		[AuthenticationRequired("NAMESPACE:{namespace}:USER:{userId}:PUBLIC:CLOUDSAVE:RECORD", AuthenticationRequiredAttribute.Access.CREATE)]
		public static bool CreateOrAppendPublicUserCloudSaveEntry(HttpRequest _, ServerStruct serverStruct) => CreateOrAppendUserCloudSaveEntry(_, serverStruct);


		[HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/users/{userId}/records/{key}")]
		[AuthenticationRequired("NAMESPACE:{namespace}:USER:{userId}:CLOUDSAVE:RECORD", AuthenticationRequiredAttribute.Access.READ)]
		public static bool GetUserCloudSaveEntry(HttpRequest _, ServerStruct serverStruct)
		{
			var data = CloudSaveDataHelper.GetUserCloudSaveData(serverStruct.Parameters["userId"], serverStruct.Parameters["key"]);

			if (data == null)
			{
				var errorResponse = new ResponseCreator(404);
				ErrorMSG errorMSG = new()
				{
					ErrorCode = 18022,
					ErrorMessage = $"unable to get_player_record: player record not found, user ID: {serverStruct.Parameters["userId"]}, key: {serverStruct.Parameters["key"]}"
				};
				errorResponse.SetHeader("Content-Type", "application/json");
				errorResponse.SetBody(JsonConvert.SerializeObject(errorMSG));
				serverStruct.Response = errorResponse.GetResponse();
				serverStruct.SendResponse();
				return true;
			}

			var response = new ResponseCreator();

			CloudSaveDataWrapper<object> responseData = new()
			{
				Value = data,
				Namespace = serverStruct.Parameters["namespace"],
				Key = serverStruct.Parameters["key"],
				SetBy = "CLIENT",
				//IsPublic = false,
				//UserId = serverStruct.Parameters["userId"],
				CreatedAt = "0001-01-01T01:01:01.001Z00:00",
				UpdatedAt = "0001-01-01T01:01:01.001Z00:00"
			};

			response.SetHeader("Content-Type", "application/json");
			response.SetBody(JsonConvert.SerializeObject(responseData));
			serverStruct.Response = response.GetResponse();
			serverStruct.SendResponse();
			return true;
		}
		[HTTP("PUT", "/cloudsave/v1/namespaces/{namespace}/users/{userId}/records/{key}")]
		[AuthenticationRequired("NAMESPACE:{namespace}:USER:{userId}:CLOUDSAVE:RECORD", AuthenticationRequiredAttribute.Access.UPDATE)]
		public static bool CreateOrReplaceUserCloudSaveEntry(HttpRequest _, ServerStruct serverStruct)
		{
			// TODO: Implement the metadata required to set a player game record to public https://docs.accelbyte.io/api-explorer/#CloudSave/putPlayerRecordHandlerV1

			object data = JsonConvert.DeserializeObject<object>(_.Body);

			CloudSaveDataHelper.SetUserCloudSaveData(serverStruct.Parameters["userId"], serverStruct.Parameters["key"], data);

			var response = new ResponseCreator();

			CloudSaveDataWrapper<object> responseData = new()
			{
				Value = data,
				Namespace = serverStruct.Parameters["namespace"],
				Key = serverStruct.Parameters["key"],
				SetBy = "CLIENT",
				IsPublic = false,
				UserId = serverStruct.Parameters["userId"],
				CreatedAt = "0001-01-01T01:01:01.001Z00:00",
				UpdatedAt = "0001-01-01T01:01:01.001Z00:00"
			};

			response.SetBody(JsonConvert.SerializeObject(responseData));
			serverStruct.Response = response.GetResponse();
			serverStruct.SendResponse();
			return true;
		}

		// https://docs.accelbyte.io/api-explorer/#CloudSave/postPlayerRecordHandlerV1
		[HTTP("POST", "/cloudsave/v1/namespaces/{namespace}/users/{userId}/records/{key}")]
		[AuthenticationRequired("NAMESPACE:{namespace}:USER:{userId}:CLOUDSAVE:RECORD", AuthenticationRequiredAttribute.Access.CREATE)]
		public static bool CreateOrAppendUserCloudSaveEntry(HttpRequest _, ServerStruct serverStruct)
		{
			var newData = JObject.Parse(_.Body);

			object oldDataObj = CloudSaveDataHelper.GetUserCloudSaveData(serverStruct.Parameters["userId"], serverStruct.Parameters["key"]);

			var oldData = new JObject();
			if(oldDataObj != null)
				oldData = JObject.Parse(JsonConvert.SerializeObject(oldDataObj));

			oldData.Merge(newData, new JsonMergeSettings
			{
				MergeArrayHandling = MergeArrayHandling.Union
			});

			object mergedObject = JsonConvert.DeserializeObject(oldData.ToString(Formatting.None));

			CloudSaveDataHelper.SetUserCloudSaveData(serverStruct.Parameters["userId"], serverStruct.Parameters["key"], mergedObject);

			var response = new ResponseCreator();

			CloudSaveDataWrapper<object> responseData = new()
			{
				Value = mergedObject,
				Namespace = serverStruct.Parameters["namespace"],
				Key = serverStruct.Parameters["key"],
				SetBy = "SERVER",
				CreatedAt = "0001-01-01T01:01:01.001Z00:00",
				UpdatedAt = "0001-01-01T01:01:01.001Z00:00"
			};

			response.SetBody(JsonConvert.SerializeObject(responseData));
			serverStruct.Response = response.GetResponse();
			serverStruct.SendResponse();
			return true;
		}
    }
}

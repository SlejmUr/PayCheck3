using Newtonsoft.Json;
using PayCheckServerLib.Jsons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib.Helpers
{
	/// <summary>
	/// Class containing all functions relevant to: global cloud save records, user cloud save records and user stat items.
	/// </summary>
	public class CloudSaveDataHelper
	{
		public static bool PathContainsIllegalCharacters(string path)
		{
			if (path.Contains("..") || path.Contains("./") || path.Contains(":") || path.Contains("\\"))
				return true;
			return false;
		}

		#region Global CloudSave Data And Static Data
		public static object? GetGlobalCloudSaveData(string key)
		{
			return GetStaticData<object>(String.Format("GlobalCloudsaveRecords/{0}.json", key));
		}
		public static T? GetStaticData<T>(string filename) where T : class
		{
			if (PathContainsIllegalCharacters(filename))
				return null;

			//Debugger.PrintInfo(String.Format("Loaded static data ./Files/{0}", filename));
			if (File.Exists(String.Format("./Files/{0}", filename)))
			{
				return JsonConvert.DeserializeObject<T>(FileReadWriteHelper.ReadAllText(String.Format("./Files/{0}", filename)));
			}
			else
			{
				Debugger.PrintError(String.Format("Static data ./Files/{0} does not exist.", filename));
			}

			return null;
		}
		#endregion

		#region User CloudSave Data
		private static string GetUserCloudSaveFolderForUserId(string userId)
		{
			return String.Format("./UserData/{0}/CloudSave", userId);
		}
		private static string GetUserCloudSavePathForUserIdAndKey(string userId, string key)
		{
			return String.Format("{0}/{1}.json", GetUserCloudSaveFolderForUserId(userId), key);
		}

		public static object? GetUserCloudSaveData(string userId, string key)
		{
			if (!UserIdHelper.IsValidUserId(userId) || PathContainsIllegalCharacters(key))
				return null;

			if (PathContainsIllegalCharacters(key))
				return null;

			string path = GetUserCloudSavePathForUserIdAndKey(userId, key);

			if (!FileReadWriteHelper.Exists(path))
				return null;


			return JsonConvert.DeserializeObject(FileReadWriteHelper.ReadAllText(path));
		}
		public static void SetUserCloudSaveData(string userId, string key, object data)
		{
			if (!UserIdHelper.IsValidUserId(userId) || PathContainsIllegalCharacters(key))
				return;

			if (PathContainsIllegalCharacters(key))
				return;


			string path = GetUserCloudSavePathForUserIdAndKey(userId, key);

			FileReadWriteHelper.WriteAllText(path, JsonConvert.SerializeObject(data));
		}
		#endregion

		#region User StatItems
		private static string GetStatItemsFolderForUserId(string userId)
		{
			return String.Format("./UserData/{0}/StatItems", userId);
		}
		private static string GetUserStatItemsPathForUserIdAndStatCode(string userId, string statCode)
		{
			return String.Format("{0}/{1}.json", GetStatItemsFolderForUserId(userId), statCode);
		}
		public static UserStatItemsData? GetUserStatItemData(string userId, string statCode)
		{
			if (!UserIdHelper.IsValidUserId(userId) || PathContainsIllegalCharacters(statCode))
				return null;

			if (PathContainsIllegalCharacters(statCode))
			{
				return null;
			}

			string path = GetUserStatItemsPathForUserIdAndStatCode(userId, statCode);

			if(!FileReadWriteHelper.Exists(path))
				return null;

			return JsonConvert.DeserializeObject<UserStatItemsData>(FileReadWriteHelper.ReadAllText(path));
		}
		public static List<UserStatItemsData> GetAllStatItemsForUser(string userId)
		{
			if (!UserIdHelper.IsValidUserId(userId) || PathContainsIllegalCharacters(userId))
				return [];


			string folder = GetStatItemsFolderForUserId(userId);
			if (!Directory.Exists(folder))
				return [];

			var files = Directory.EnumerateFiles(folder);

			var returnList = new List<UserStatItemsData>();

			foreach(var file in files)
			{
				var statItemData = JsonConvert.DeserializeObject<UserStatItemsData>(FileReadWriteHelper.ReadAllText(file));

				if(statItemData != null)
					returnList.Add(statItemData);
			}

			return returnList;
		}
		public static void IncrementStatItemValueForUser(string namespace_, string userId, string statCode, float inc, out UserStatItemsData? updatedStatItemData)
		{
			if (!UserIdHelper.IsValidUserId(userId) || PathContainsIllegalCharacters(userId))
			{
				updatedStatItemData = null;
				return;
			}

			if (PathContainsIllegalCharacters(statCode))
			{
				updatedStatItemData = null;
				return;
			}

			string path = GetUserStatItemsPathForUserIdAndStatCode(userId, statCode);

			var existingData = GetUserStatItemData(userId, statCode);

			if(existingData == null)
			{
				updatedStatItemData = new UserStatItemsData()
				{
					CreatedAt = TimeHelper.GetZTime(),
					Namespace = namespace_,
					StatCode = statCode,
					StatName = statCode,
					UpdatedAt = TimeHelper.GetZTime(),
					UserId = userId,
					Value = inc,
				};

				FileReadWriteHelper.WriteAllText(path, JsonConvert.SerializeObject(updatedStatItemData));

				return;
			}

			existingData.UpdatedAt = TimeHelper.GetZTime();
			existingData.Value += inc;

			FileReadWriteHelper.WriteAllText(path, JsonConvert.SerializeObject(existingData));

			updatedStatItemData = existingData;

			return;
		}
		#endregion
	}
}

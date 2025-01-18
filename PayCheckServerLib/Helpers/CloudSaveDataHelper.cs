using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib.Helpers
{
	public class CloudSaveDataHelper
	{
		public static bool PathContainsIllegalCharacters(string path)
		{
			if (path.Contains("..") || path.Contains("./") || path.Contains(":") || path.Contains("\\"))
				return true;
			return false;
		}
		public static object? GetGlobalCloudSaveData(string key)
		{
			return GetStaticData<object>(String.Format("GlobalStatItems/{0}.json", key));
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

		private static string GetUserCloudSavePathForUserIdAndKey(string userId, string key)
		{
			return String.Format("./UserData/{0}/CloudSave/{1}.json", userId, key);
		}

		public static object? GetUserCloudSaveData(string userId, string key)
		{
			if (!UserIdHelper.IsValidUserId(userId) || PathContainsIllegalCharacters(key))
				return null;

			string path = GetUserCloudSavePathForUserIdAndKey(userId, key);

			if (!File.Exists(path))
				return null;


			return JsonConvert.DeserializeObject(FileReadWriteHelper.ReadAllText(path));
		}
		public static void SetUserCloudSaveData(string userId, string key, object data)
		{
			if (!UserIdHelper.IsValidUserId(userId) || PathContainsIllegalCharacters(key))
				return;

			string path = GetUserCloudSavePathForUserIdAndKey(userId, key);

			if (!File.Exists(path))
				File.Create(path);

			FileReadWriteHelper.WriteAllText(path, JsonConvert.SerializeObject(data));
		}
	}
}

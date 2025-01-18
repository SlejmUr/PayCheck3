using Newtonsoft.Json;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.Basic;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib.Helpers
{
	/// <summary>
	/// Class containing all functions relevant to items and user entitlements
	/// </summary>
	public class UserEntitlementHelper
	{
		private static string GetEntitlementsPathForUserId(string userId)
		{
			return String.Format("./UserData/{0}/Entitlements.json", userId);
		}

		private static List<ItemDefinitionJson> ItemDefinitions { get; set; } = new List<ItemDefinitionJson>();
		private static List<RewardItem> Rewards { get; set; } = new List<RewardItem>();
		private static void TryLoadItemDefinitions()
		{
			if (ItemDefinitions.Count != 0)
				return; // already loaded

			var loadedData = CloudSaveDataHelper.GetStaticData<DataPaging<ItemDefinitionJson>>("Items.json");

			if (loadedData != null)
				ItemDefinitions = loadedData.Data;
		}
		private static void TryLoadRewards()
		{
			if (Rewards.Count != 0)
				return;

			var loadedData = CloudSaveDataHelper.GetStaticData<DataPaging<RewardItem>>("Rewards.json");
			
			if (loadedData != null)
				Rewards = loadedData.Data;
		}

		public static List<ItemDefinitionJson> GetItemDefinitions()
		{
			TryLoadItemDefinitions();
			return ItemDefinitions;
		}

		public static List<EntitlementsData> GetEntitlementDataForUser(string userId)
		{
			if(!UserIdHelper.IsValidUserId(userId))
			{
				return new();
			}

			string path = GetEntitlementsPathForUserId(userId);
			if (!File.Exists(path))
				return new();

			var data = JsonConvert.DeserializeObject<List<EntitlementsData>>(File.ReadAllText(path));

			if (data == null)
				return new();

			return data;
		}

		private static void SetEntitlementDataForUser(string userId, List<EntitlementsData> data)
		{
			if (!UserIdHelper.IsValidUserId(userId))
				return;

			string path = GetEntitlementsPathForUserId(userId);
			if(!File.Exists(path))
				File.Create(path);

			
			File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));
		}

		/// <summary>
		/// Adds an item with a provided sku to the user
		/// </summary>
		/// <param name="userId">The user to grant the item to</param>
		/// <param name="namespace_">The namespace that the item should be granted in</param>
		/// <param name="sku">The SKU of the item, if the provided sku does not exist in the item list, a fake entitlement will be created</param>
		/// <param name="quantity">The number of items to grant</param>
		/// <param name="source">Can be "PURCHASE" or "REWARD"</param>
		public static void AddEntitlementToUserViaSKU(string userId, string namespace_, string sku, int quantity, out EntitlementsData? addedEntitlement, string source = "PURCHASE")
		{
			if (!UserIdHelper.IsValidUserId(userId))
			{
				addedEntitlement = null;
				return;
			}
			TryLoadItemDefinitions();

			var userEntitlements = GetEntitlementDataForUser(userId);

			var itemInUserEntitlements = userEntitlements.Find(entitlement => entitlement.Sku == sku);
			if(itemInUserEntitlements != null)
			{
				var index = userEntitlements.IndexOf(itemInUserEntitlements);
				userEntitlements[index].UseCount += quantity;
				SetEntitlementDataForUser(userId, userEntitlements);
				addedEntitlement = userEntitlements[index];
				return;
			}

			var itemData = ItemDefinitions.Find(item => item.Sku == sku);
			if(itemData == null)
			{
				// sku is not present in the items list, adding it anyway with a random item id

				var entitlement = new EntitlementsData()
				{
					Id = UserIdHelper.CreateNewID(), // uuidv4
					Namespace = namespace_,
					Clazz = "ENTITLEMENT",
					Type = "CONSUMABLE",
					Status = "ACTIVE",
					Sku = sku,
					UserId = userId,
					ItemId = UserIdHelper.CreateNewID(),
					ItemNamespace = namespace_,
					Name = sku,
					Source = source,
					GrantedAt = TimeHelper.GetZTime(),
					CreatedAt = TimeHelper.GetZTime(),
					UpdatedAt = TimeHelper.GetZTime(),
					UseCount = quantity,
					Stackable = true
				};
				addedEntitlement = entitlement;
				userEntitlements.Add(entitlement);
			} else
			{
				var entitlement = new EntitlementsData()
				{
					Id = UserIdHelper.CreateNewID(), // uuidv4
					Namespace = namespace_,
					Clazz = "ENTITLEMENT",
					Type = "CONSUMABLE",
					Status = "ACTIVE",
					Sku = sku,
					UserId = userId,
					ItemId = itemData.ItemId,
					ItemNamespace = namespace_,
					Name = sku,
					Source = source,
					GrantedAt = TimeHelper.GetZTime(),
					CreatedAt = TimeHelper.GetZTime(),
					UpdatedAt = TimeHelper.GetZTime(),
					UseCount = quantity,
					Stackable = true
				};

				addedEntitlement = entitlement;
				userEntitlements.Add(entitlement);
			}
			SetEntitlementDataForUser(userId, userEntitlements);
		}

		public static void CheckForRewardsOnStatItemUpdate(string userId, string statItem)
		{
			TryLoadRewards();
		}
	}
}

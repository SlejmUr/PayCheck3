using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
		/// Adds items with the provided Item IDs and quantities to the user
		/// </summary>
		/// <param name="userId">The user to grant the item to</param>
		/// <param name="namespace_">The namespace that the item should be granted in</param>
		/// <param name="itemIds">The id of items to be added, must have the same number of entries as the quantities list</param>
		/// <param name="quantities">The quantity of items to be added, must have the same number of entries as the item id list</param>
		/// <param name="source">Can be "PURCHASE" or "REWARD"</param>
		public static void AddBulkEntitlementToUserViaItemId(string userId, string namespace_, List<string> itemIds, List<int> quantities, out List<EntitlementsData> addedEntitlements, string source = "PURCHASE")
		{
			addedEntitlements = new List<EntitlementsData>();
			if (!UserIdHelper.IsValidUserId(userId))
			{
				return;
			}

			if (itemIds.Count != quantities.Count)
			{
				return;
			}

			var userEntitlements = GetEntitlementDataForUser(userId);

			for (int i = 0; i < itemIds.Count; i++)
			{
				var itemId = itemIds[i];
				var quantity = quantities[i];

				var itemInUserEntitlements = userEntitlements.FindIndex(entitlement => entitlement.ItemId == itemId);
				if (itemInUserEntitlements != -1)
				{
					userEntitlements[itemInUserEntitlements].UseCount += quantity;
					addedEntitlements.Add(userEntitlements[itemInUserEntitlements]);
					continue;
				}

				var itemData = ItemDefinitions.Find(item => item.ItemId == itemId);

				if (itemData == null)
					continue;

				var entitlement = new EntitlementsData()
				{
					Id = itemData.ItemId,
					Namespace = namespace_,
					Clazz = "ENTITLEMENT",
					Type = itemData.EntitlementType,
					Status = "ACTIVE",
					Sku = itemData.Sku,
					UserId = userId,
					ItemId = itemData.ItemId,
					ItemNamespace = namespace_,
					Name = itemData.Name,
					Source = source,
					GrantedAt = TimeHelper.GetZTime(),
					CreatedAt = TimeHelper.GetZTime(),
					UpdatedAt = TimeHelper.GetZTime(),
					UseCount = quantity,
					Stackable = true
				};
				userEntitlements.Add(entitlement);
				addedEntitlements.Add(entitlement);
			}
			SetEntitlementDataForUser(userId, userEntitlements);
		}
		public static void AddEntitlementToUserViaItemId(string userId, string namespace_, string itemId, int quantity, out EntitlementsData? addedEntitlement, string source = "PURCHASE")
		{
			List<EntitlementsData>? addedEntitlements = null;
			AddBulkEntitlementToUserViaItemId(userId, namespace_, [itemId], [quantity], out addedEntitlements, source);

			if (addedEntitlements.Count > 0)
			{
				addedEntitlement = addedEntitlements[0];
			}
			else
			{
				addedEntitlement = null;
			}
			return;
		}
		/// <summary>
		/// Adds items with the provided SKUs and quantities to the user
		/// </summary>
		/// <param name="userId">The user to grant the item to</param>
		/// <param name="namespace_">The namespace that the item should be granted in</param>
		/// <param name="skus">The sku of items to be added, must have the same number of entries as the quantities list</param>
		/// <param name="quantities">The quantity of items to be added, must have the same number of entries as the sku list</param>
		/// <param name="source">Can be "PURCHASE" or "REWARD"</param>
		public static void AddBulkEntitlementToUserViaSKU(string userId, string namespace_, List<string> skus, List<int> quantities, out List<EntitlementsData> addedEntitlements, string source = "PURCHASE")
		{
			addedEntitlements = new List<EntitlementsData>();
			if (!UserIdHelper.IsValidUserId(userId))
			{
				return;
			}

			if (skus.Count != quantities.Count)
			{
				return;
			}

			var userEntitlements = GetEntitlementDataForUser(userId);

			for (int i = 0; i < skus.Count; i++)
			{
				var sku = skus[i];
				var quantity = quantities[i];

				var itemInUserEntitlements = userEntitlements.FindIndex(entitlement => entitlement.Sku == sku);
				if (itemInUserEntitlements != -1)
				{
					userEntitlements[itemInUserEntitlements].UseCount += quantity;
					addedEntitlements.Add(userEntitlements[itemInUserEntitlements]);
					continue;
				}

				var itemData = ItemDefinitions.Find(item => item.Sku == sku);
				if (itemData == null)
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
					userEntitlements.Add(entitlement);
					addedEntitlements.Add(entitlement);
				} else
				{

					var entitlement = new EntitlementsData()
					{
						Id = UserIdHelper.CreateNewID(), // uuidv4
						Namespace = namespace_,
						Clazz = "ENTITLEMENT",
						Type = itemData.EntitlementType,
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
					userEntitlements.Add(entitlement);
					addedEntitlements.Add(entitlement);
				}
			}
			SetEntitlementDataForUser(userId, userEntitlements);
		}
		public static void AddEntitlementToUserViaSKU(string userId, string namespace_, string sku, int quantity, out EntitlementsData? addedEntitlement, string source = "PURCHASE")
		{
			List<EntitlementsData>? addedEntitlements = null;
			AddBulkEntitlementToUserViaSKU(userId, namespace_, [sku], [quantity], out addedEntitlements, source);

			if (addedEntitlements.Count > 0)
			{
				addedEntitlement = addedEntitlements[0];
			} else
			{
				addedEntitlement = null;
			}
			return;
		}

		public static void CheckForRewardsOnStatItemUpdate(string userId, List<UserStatItemsData> statItems)
		{
			TryLoadRewards();

			if (!UserIdHelper.IsValidUserId(userId))
				return;

			if (statItems.Count == 0)
				return; // Don't try to reward stat items if there's none

			var jsonPathTestArray = JArray.Parse(JsonConvert.SerializeObject(statItems));

			var statisticRewards = Rewards.FindAll(reward => reward.EventTopic == "statistic");

			/*
			 *	var testobj = JArray.Parse("[{'namespace': 'pd3','challengeId': '63e505437f4d322dc61d2dd8'}]");
			 *	JToken? selected = testobj.SelectToken("$.[?(@.namespace == 'pd3' && @.challengeId == '63e505437f4d322dc61d2dd8')]");
			 */

		}
	}
}

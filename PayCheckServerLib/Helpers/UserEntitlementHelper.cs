using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.Basic;
using PayCheckServerLib.Responses;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
		private static string GetAwardedRewardsPathForUserId(string userId)
		{
			return String.Format("./UserData/{0}/AwardedRewards.json", userId);
		}

		#region Loading Static Data
		private static List<ItemDefinitionJson> ItemDefinitions { get; set; } = new List<ItemDefinitionJson>();
		private static List<RewardItem> Rewards { get; set; } = new List<RewardItem>();
		private static void TryLoadItemDefinitions()
		{
			if (ItemDefinitions.Count != 0)
				return; // already loaded

			var loadedData = CloudSaveDataHelper.GetStaticData<List<ItemDefinitionJson>>("Items.json");

			if (loadedData != null)
				ItemDefinitions = loadedData;
		}
		private static void TryLoadRewards()
		{
			if (Rewards.Count != 0)
				return;

			var loadedData = CloudSaveDataHelper.GetStaticData<List<RewardItem>>("Rewards.json");
			
			if (loadedData != null)
				Rewards = loadedData;
		}
		#endregion

		public static List<ItemDefinitionJson> GetItemDefinitions()
		{
			TryLoadItemDefinitions();
			return ItemDefinitions;
		}

		#region Raw Entitlement Data Access
		private static int EntitlementAccessLocked { get; set; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="lockAccess">Enter a locked state where other parts of the program must wait for you to finished what you are doing before getting entitlement info. A locked state is ended when SetEntitlementDataForUser is called.</param>
		/// <returns></returns>
		public static List<EntitlementsData> GetEntitlementDataForUser(string userId, bool lockAccess)
		{
			while (EntitlementAccessLocked > 0) { }
			if(lockAccess)
				EntitlementAccessLocked++;

			if(!UserIdHelper.IsValidUserId(userId))
			{
				return new();
			}

			string path = GetEntitlementsPathForUserId(userId);

			if (!FileReadWriteHelper.Exists(path))
				return new();

			var data = JsonConvert.DeserializeObject<List<EntitlementsData>>(FileReadWriteHelper.ReadAllText(path));

			if (data == null)
				return new();

			return data;
		}

		private static void SetEntitlementDataForUser(string userId, List<EntitlementsData> data)
		{
			if (!UserIdHelper.IsValidUserId(userId))
				return;

			string path = GetEntitlementsPathForUserId(userId);

			
			FileReadWriteHelper.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));
			if (EntitlementAccessLocked > 0)
				EntitlementAccessLocked--;
		}
		#endregion

		#region Granting Entitlements

		private static List<EntitlementsData> AddItemToReferencedEntitlementListByItemDefinition(string userId, ref List<EntitlementsData> entitlements, ItemDefinitionJson itemDefinition, int quantity, string source)
		{
			TryLoadItemDefinitions();
			var itemInUserEntitlements = entitlements.FindIndex(entitlement => entitlement.ItemId == itemDefinition.ItemId);
			if (itemInUserEntitlements != -1)
			{
				entitlements[itemInUserEntitlements].UseCount += quantity;
				return [entitlements[itemInUserEntitlements]];
			}

			List<EntitlementsData> addedItems = new List<EntitlementsData>();
			if (itemDefinition.ItemType == "BUNDLE")
			{
				Dictionary<string, int> bundleContents = new Dictionary<string, int>();
				foreach(var itemId in itemDefinition.ItemIds)
				{
					bundleContents.TryAdd(itemId, 0);
				}
				foreach(var itemQuantity in itemDefinition.ItemQty)
				{
					bundleContents[itemQuantity.Key] = itemQuantity.Value;
				}

				foreach(var bundleItem in bundleContents)
				{
					var bundleItemDefinition = ItemDefinitions.Find(itemData => itemData.ItemId == bundleItem.Key);

					if(bundleItemDefinition != null)
					{
						addedItems = addedItems.Concat(AddItemToReferencedEntitlementListByItemDefinition(userId, ref entitlements, bundleItemDefinition, bundleItem.Value, source)).ToList();
					}
				}
			}
			else
			{
				var entitlement = new EntitlementsData()
				{
					Id = itemDefinition.ItemId,
					Namespace = itemDefinition.Namespace,
					Clazz = "ENTITLEMENT",
					Type = itemDefinition.EntitlementType,
					Status = "ACTIVE",
					Sku = itemDefinition.Sku,
					UserId = userId,
					ItemId = itemDefinition.ItemId,
					ItemNamespace = itemDefinition.Namespace,
					Name = itemDefinition.Name,
					Source = source,
					GrantedAt = TimeHelper.GetZTime(),
					CreatedAt = TimeHelper.GetZTime(),
					UpdatedAt = TimeHelper.GetZTime(),
					UseCount = quantity,
					Stackable = true
				};
				entitlements.Add(entitlement);
				addedItems.Add(entitlement);
			}
			return addedItems;
		}
		private static List<EntitlementsData> AddItemToReferencedEntitlementListByItemId(string userId, ref List<EntitlementsData> entitlements, string itemId, int quantity, string source)
		{
			TryLoadItemDefinitions();

			var item = ItemDefinitions.Find(item => item.ItemId == itemId);

			if (item == null)
				return [];

			return AddItemToReferencedEntitlementListByItemDefinition(userId, ref entitlements, item, quantity, source);
		}
		private static List<EntitlementsData> AddItemToReferencedEntitlementListBySKU(string userId, ref List<EntitlementsData> entitlements, string sku, int quantity, string source)
		{
			TryLoadItemDefinitions();

			var item = ItemDefinitions.Find(item => item.Sku == sku);

			if (item == null)
				return [];

			return AddItemToReferencedEntitlementListByItemDefinition(userId, ref entitlements, item, quantity, source);
		}

		/// <summary>
		/// Adds items with the provided Item IDs and quantities to the user
		/// </summary>
		/// <param name="userId">The user to grant the item to</param>
		/// <param name="namespace_">The namespace that the item should be granted in</param>
		/// <param name="items">A list of key/value pairs of item ids and quantities</param>
		/// <param name="source">Can be "PURCHASE" or "REWARD"</param>
		public static void AddBulkEntitlementToUserViaItemId(string userId, string namespace_, List<KeyValuePair<string, int>> items, out List<EntitlementsData> addedEntitlements, string source = "PURCHASE")
		{
			TryLoadItemDefinitions();

			addedEntitlements = new List<EntitlementsData>();
			if (!UserIdHelper.IsValidUserId(userId))
			{
				return;
			}

			var userEntitlements = GetEntitlementDataForUser(userId, true);

			foreach (var item in items)
			{
				var itemId = item.Key;
				var quantity = item.Value;

				var entitlement = AddItemToReferencedEntitlementListByItemId(userId, ref userEntitlements, itemId, quantity, source);
				addedEntitlements = addedEntitlements.Concat(entitlement).ToList();
			}
			SetEntitlementDataForUser(userId, userEntitlements);
		}
		public static void AddEntitlementToUserViaItemId(string userId, string namespace_, string itemId, int quantity, out List<EntitlementsData> addedEntitlement, string source = "PURCHASE")
		{
			AddBulkEntitlementToUserViaItemId(userId, namespace_, [new(itemId, quantity)], out addedEntitlement, source);
		}
		/// <summary>
		/// Adds items with the provided SKUs and quantities to the user
		/// </summary>
		/// <param name="userId">The user to grant the item to</param>
		/// <param name="namespace_">The namespace that the item should be granted in</param>
		/// <param name="items">A list of key/value pairs of item skus and quantities</param>
		/// <param name="source">Can be "PURCHASE" or "REWARD"</param>
		public static void AddBulkEntitlementToUserViaSKU(string userId, string namespace_, List<KeyValuePair<string, int>> items, out List<EntitlementsData> addedEntitlements, string source = "PURCHASE")
		{
			TryLoadItemDefinitions();

			addedEntitlements = new List<EntitlementsData>();
			if (!UserIdHelper.IsValidUserId(userId))
			{
				return;
			}

			var userEntitlements = GetEntitlementDataForUser(userId, true);

			foreach(var item in items)
			{
				var sku = item.Key;
				var quantity = item.Value;

				var entitlement = AddItemToReferencedEntitlementListBySKU(userId, ref userEntitlements, sku, quantity, source);
				addedEntitlements = addedEntitlements.Concat(entitlement).ToList();
			}
			SetEntitlementDataForUser(userId, userEntitlements);
		}
		public static void AddEntitlementToUserViaSKU(string userId, string namespace_, string sku, int quantity, out List<EntitlementsData> addedEntitlement, string source = "PURCHASE")
		{
			AddBulkEntitlementToUserViaSKU(userId, namespace_, [new(sku, quantity)], out addedEntitlement, source);
		}
		#endregion

		#region Rewards
		private static List<string> GetAwardedRewardIdsForUser(string userId)
		{
			if (!UserIdHelper.IsValidUserId(userId))
				return [];

			string path = GetAwardedRewardsPathForUserId(userId);

			if (!File.Exists(path))
				return [];

			return JsonConvert.DeserializeObject<List<string>>(FileReadWriteHelper.ReadAllText(path));
		}

		private static void AddAwardedRewardIdsToUser(string userId, List<string> awardedRewardIds)
		{
			if (!UserIdHelper.IsValidUserId(userId))
				return;

			string path = GetAwardedRewardsPathForUserId(userId);

			var newAwardedList = new List<string>();

			if (File.Exists(path))
				newAwardedList = JsonConvert.DeserializeObject<List<string>>(FileReadWriteHelper.ReadAllText(path));

			if(newAwardedList == null)
			{
				newAwardedList = awardedRewardIds;
			} else
			{
				newAwardedList = newAwardedList.Concat(awardedRewardIds).ToList();
			}

			FileReadWriteHelper.WriteAllText(path, JsonConvert.SerializeObject(newAwardedList));
		}

		public static void CheckForRewardsOnStatItemUpdate(string namespace_, string userId, List<UserStatItemsData> statItems)
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

			// dictionary of item ids to item quantities
			var itemsToAward = new Dictionary<string, int>();

			var alreadyAwardedRewards = GetAwardedRewardIdsForUser(userId);

			var newAwardedRewards = new List<string>();
			foreach (var reward in statisticRewards)
			{
				if (alreadyAwardedRewards.Contains(reward.RewardId))
					continue; // User already has reward, skip checking if they should have it awarded

				foreach(var rewardCondition in reward.RewardConditions) // None of the rewards that Starbreeze have added have more than one reward condition
				{
					if (rewardCondition.EventName != "statItemUpdated")
						continue;


					bool passedCondition = false;

					/// Lazy solution, just checking if the condition string even contains the stat code
					foreach(var updatedStatCode in statItems)
					{
						if (rewardCondition.Condition.Contains(updatedStatCode.StatCode))
						{
							passedCondition = true;
						}
					}

					/// Proper JSONPath based solution
					//JToken? conditionToken = jsonPathTestArray.SelectToken(rewardCondition.Condition);
					/*if (conditionToken != null)
					{
						var conditionTokenArray = conditionToken as JArray;
						if (conditionTokenArray != null)
						{
							if (conditionTokenArray.Count > 0) // JSONPath condition was successful, user has passed the reward condition
							{
								passedCondition = true;
							}
						}
					}*/

					if (passedCondition)
					{
						newAwardedRewards.Add(reward.RewardId);

						foreach(var awardedItem in rewardCondition.RewardItems)
						{
							if (!itemsToAward.ContainsKey(awardedItem.ItemId))
								itemsToAward.Add(awardedItem.ItemId, 0);
							itemsToAward[awardedItem.ItemId] += awardedItem.Quantity;
						}
					}
				}
			}


			AddBulkEntitlementToUserViaItemId(userId, namespace_, itemsToAward.ToList(), out List<EntitlementsData> newEntitlements, "REWARD");
			AddAwardedRewardIdsToUser(userId, newAwardedRewards);
		}

		#endregion
	}
}

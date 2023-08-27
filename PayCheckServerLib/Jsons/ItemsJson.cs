using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class ItemDefinitionJsonRegionData
    {
        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("discountPercentage")]
        public int DiscountPercentage { get; set; }

        [JsonProperty("discountAmount")]
        public int DiscountAmout { get; set; }

        [JsonProperty("discountedPrice")]
        public int DiscountedPrice { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("currencyType")]
        public string CurrencyType { get; set; }

        [JsonProperty("currencyNamespace")]
        public string CurrencyNamespace { get; set; }

        [JsonProperty("purchaseAt")]
        public string PurchaseAt { get; set; }

        [JsonProperty("discountPurchaseAt")]
        public string DiscountPurchaseAt { get; set; }
    }

    public class ItemDefinitionJson
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("entitlementType")]
        public string EntitlementType { get; set; }

        [JsonProperty("useCount")]
        public int UseCount { get; set; }

        [JsonProperty("stackable")]
        public bool Stackable { get; set; }

        [JsonProperty("categoryPath")]
        public string CategoryPath { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("listable")]
        public bool Listable { get; set; }

        [JsonProperty("purchasable")]
        public bool Purchasable { get; set; }

        [JsonProperty("itemType")]
        public string ItemType { get; set; }

        [JsonProperty("targetCurrencyCode")]
        public string TargetCurrencyCode { get; set; }

        [JsonProperty("regionData")]
        public List<ItemDefinitionJsonRegionData> RegionData { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("features")]
        public List<string> Features { get; set; }

        [JsonProperty("maxCountPerUser")]
        public int MaxCountPerUser { get; set; }

        [JsonProperty("maxCount")]
        public int MaxCount { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }
    }
    public class ItemsJson
    {
        [JsonProperty("data")]
        public List<ItemDefinitionJson> Data { get; set; }

        [JsonProperty("paging")]
        public object Paging { get; set; }
    }
}

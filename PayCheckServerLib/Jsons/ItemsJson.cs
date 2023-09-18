using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class ItemDefinitionJson
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        [JsonProperty("sku", NullValueHandling = NullValueHandling.Ignore)]
        public string Sku { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("entitlementType")]
        public string EntitlementType { get; set; }

        [JsonProperty("useCount", NullValueHandling = NullValueHandling.Ignore)]
        public long? UseCount { get; set; }

        [JsonProperty("stackable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Stackable { get; set; }

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

        [JsonProperty("targetCurrencyCode", NullValueHandling = NullValueHandling.Ignore)]
        public string? TargetCurrencyCode { get; set; }

        [JsonProperty("regionData")]
        public List<RegionDatum> RegionData { get; set; }

        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }

        [JsonProperty("features", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Features { get; set; }

        [JsonProperty("maxCountPerUser")]
        public long MaxCountPerUser { get; set; }

        [JsonProperty("maxCount")]
        public long MaxCount { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("baseAppId", NullValueHandling = NullValueHandling.Ignore)]
        public string BaseAppId { get; set; }

        [JsonProperty("longDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string LongDescription { get; set; }

        [JsonProperty("boundItemIds", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> BoundItemIds { get; set; }

        [JsonProperty("itemIds", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ItemIds { get; set; }

        [JsonProperty("itemQty", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, long> ItemQty { get; set; }

        [JsonProperty("displayOrder", NullValueHandling = NullValueHandling.Ignore)]
        public long? DisplayOrder { get; set; }

        [JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
        public List<Image> Images { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("as")]
        public string As { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("imageUrl")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("smallImageUrl")]
        public Uri SmallImageUrl { get; set; }
    }

    public partial class RegionDatum
    {
        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("discountPercentage")]
        public long DiscountPercentage { get; set; }

        [JsonProperty("discountAmount")]
        public long DiscountAmount { get; set; }

        [JsonProperty("discountedPrice")]
        public long DiscountedPrice { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("currencyType")]
        public string CurrencyType { get; set; }

        [JsonProperty("currencyNamespace")]
        public string CurrencyNamespace { get; set; }

        [JsonProperty("purchaseAt", NullValueHandling = NullValueHandling.Ignore)]
        public string? PurchaseAt { get; set; }

        [JsonProperty("discountPurchaseAt", NullValueHandling = NullValueHandling.Ignore)]
        public string? DiscountPurchaseAt { get; set; }
    }
}

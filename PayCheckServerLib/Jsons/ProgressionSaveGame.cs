using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons
{
    public class ProgressionSaveRSP
    {
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; } = false;

        [JsonProperty("key")]
        public string Key { get; set; } = "progressionsavegame";

        [JsonProperty("namespace")]
        public string Namespace { get; set; } = "pd3beta";

        [JsonProperty("set_by")]
        public string SetBy { get; set; } = "CLIENT";

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("value")]
        public ProgressionSave Value { get; set; }
    }


    public class ProgressionSave
    {
        [JsonProperty("CurrentVersion")]
        public long CurrentVersion { get; set; }

        [JsonProperty("ProgressionSaveGame")]
        public ProgressionSaveGame ProgressionSaveGame { get; set; }
    }
    public partial class ProgressionSaveGame
    {
        [JsonProperty("activeLoadoutIndex")]
        public long ActiveLoadoutIndex { get; set; }

        [JsonProperty("currentRequestedPriceProgressionLevel")]
        public CurrentRequestedPriceProgressionLevel CurrentRequestedPriceProgressionLevel { get; set; }

        [JsonProperty("gloveConfigInventorySaveData")]
        public GloveConfigInventorySaveData GloveConfigInventorySaveData { get; set; }

        [JsonProperty("infamySaveData")]
        public InfamySaveData InfamySaveData { get; set; }

        [JsonProperty("itemConfigInventorySaveData")]
        public ItemConfigInventorySaveData ItemConfigInventorySaveData { get; set; }

        [JsonProperty("lastTimeEventCheck")]
        public long LastTimeEventCheck { get; set; }

        [JsonProperty("loadout")]
        public Loadout Loadout { get; set; }

        [JsonProperty("maskConfigInventorySaveData")]
        public MaskConfigInventorySaveData MaskConfigInventorySaveData { get; set; }

        [JsonProperty("playerCosmeticsConfig")]
        public PlayerCosmeticsConfig PlayerCosmeticsConfig { get; set; }

        [JsonProperty("playerLoadoutConfigArray")]
        public PlayerLoadoutConfigArray[] PlayerLoadoutConfigArray { get; set; }

        [JsonProperty("playerPreferredCharacterArray")]
        public string[] PlayerPreferredCharacterArray { get; set; }

        [JsonProperty("researchMarker")]
        public string ResearchMarker { get; set; }

        [JsonProperty("suitConfigInventorySaveData")]
        public SuitConfigInventorySaveData SuitConfigInventorySaveData { get; set; }
    }

    public partial class CurrentRequestedPriceProgressionLevel
    {
    }

    public partial class GloveConfigInventorySaveData
    {
        [JsonProperty("gloveConfigSlots")]
        public GloveConfigSlot[] GloveConfigSlots { get; set; }
    }

    public partial class GloveConfigSlot
    {
        [JsonProperty("configSlotEntitlementId")]
        public string ConfigSlotEntitlementId { get; set; }

        [JsonProperty("configSlotItemId")]
        public string ConfigSlotItemId { get; set; }

        [JsonProperty("gloveData")]
        public string GloveData { get; set; }

        [JsonProperty("gloveInSlotAccelByteItemId")]
        public string GloveInSlotAccelByteItemId { get; set; }

        [JsonProperty("gloveInSlotEntitlementId")]
        public string GloveInSlotEntitlementId { get; set; }

        [JsonProperty("itemInventorySlotAvailability")]
        public string ItemInventorySlotAvailability { get; set; }
    }

    public partial class InfamySaveData
    {
        [JsonProperty("infamyExperience")]
        public long InfamyExperience { get; set; }

        [JsonProperty("infamyLevel")]
        public long InfamyLevel { get; set; }
    }

    public partial class ItemConfigInventorySaveData
    {
        [JsonProperty("overkillWeaponConfigSlots")]
        public object[] OverkillWeaponConfigSlots { get; set; }

        [JsonProperty("primaryWeaponConfigSlots")]
        public AryWeaponConfigSlot[] PrimaryWeaponConfigSlots { get; set; }

        [JsonProperty("secondaryWeaponConfigSlots")]
        public AryWeaponConfigSlot[] SecondaryWeaponConfigSlots { get; set; }
    }

    public partial class AryWeaponConfigSlot
    {
        [JsonProperty("configSlotEntitlementId")]
        public string ConfigSlotEntitlementId { get; set; }

        [JsonProperty("configSlotItemId")]
        public string ConfigSlotItemId { get; set; }

        [JsonProperty("itemInventorySlotAvailability")]
        public string ItemInventorySlotAvailability { get; set; }

        [JsonProperty("weaponConfigInventorySlot")]
        public WeaponConfigInventorySlot WeaponConfigInventorySlot { get; set; }

        [JsonProperty("weaponInSlotAccelByteItemId")]
        public string WeaponInSlotAccelByteItemId { get; set; }

        [JsonProperty("weaponInSlotAccelByteItemSku")]
        public string WeaponInSlotAccelByteItemSku { get; set; }

        [JsonProperty("weaponInSlotEntitlementId")]
        public string WeaponInSlotEntitlementId { get; set; }

        [JsonProperty("weaponInventorySlotType")]
        public string WeaponInventorySlotType { get; set; }

        [JsonProperty("weaponPresetConfigInventorySlot")]
        public WeaponPresetConfigInventorySlot WeaponPresetConfigInventorySlot { get; set; }
    }

    public partial class WeaponConfigInventorySlot
    {
        [JsonProperty("equippableConfig")]
        public OverkillWeaponConfig EquippableConfig { get; set; }

        [JsonProperty("payedWeaponPartAttachmentItemIdArray")]
        public object[] PayedWeaponPartAttachmentItemIdArray { get; set; }
    }

    public partial class OverkillWeaponConfig
    {
        [JsonProperty("equippableData")]
        public string EquippableData { get; set; }

        [JsonProperty("modDataMap")]
        public CurrentRequestedPriceProgressionLevel ModDataMap { get; set; }
    }

    public partial class WeaponPresetConfigInventorySlot
    {
        [JsonProperty("weaponPresetConfigData")]
        public string WeaponPresetConfigData { get; set; }
    }

    public partial class Loadout
    {
        [JsonProperty("armorData")]
        public string ArmorData { get; set; }

        [JsonProperty("equippableConfigArray")]
        public OverkillWeaponConfig[] EquippableConfigArray { get; set; }

        [JsonProperty("gloveData")]
        public string GloveData { get; set; }

        [JsonProperty("maskConfig")]
        public MaskConfig MaskConfig { get; set; }

        [JsonProperty("maskData")]
        public string MaskData { get; set; }

        [JsonProperty("modifiableLoadoutDataArray")]
        public ModifiableLoadoutDataArray[] ModifiableLoadoutDataArray { get; set; }

        [JsonProperty("overkillWeaponConfig")]
        public OverkillWeaponConfig OverkillWeaponConfig { get; set; }

        [JsonProperty("preferredCharacterDataArray")]
        public string[] PreferredCharacterDataArray { get; set; }

        [JsonProperty("skillArray")]
        public object[] SkillArray { get; set; }

        [JsonProperty("suitConfig")]
        public SuitConfig SuitConfig { get; set; }

        [JsonProperty("suitData")]
        public string SuitData { get; set; }

        [JsonProperty("throwableConfigArray")]
        public ThrowableConfigArray[] ThrowableConfigArray { get; set; }
    }

    public partial class MaskConfig
    {
        [JsonProperty("maskData")]
        public string MaskData { get; set; }

        [JsonProperty("modDataMap")]
        public CurrentRequestedPriceProgressionLevel ModDataMap { get; set; }
    }

    public partial class ModifiableLoadoutDataArray
    {
        [JsonProperty("toolData")]
        public string ToolData { get; set; }
    }

    public partial class SuitConfig
    {
        [JsonProperty("modDataMapArray")]
        public ModDataMapArray[] ModDataMapArray { get; set; }

        [JsonProperty("suitBaseData")]
        public string SuitBaseData { get; set; }

        [JsonProperty("suitData")]
        public string SuitData { get; set; }
    }

    public partial class ModDataMapArray
    {
        [JsonProperty("modDataMap")]
        public CurrentRequestedPriceProgressionLevel ModDataMap { get; set; }
    }

    public partial class ThrowableConfigArray
    {
        [JsonProperty("data")]
        public string Data { get; set; }
    }

    public partial class MaskConfigInventorySaveData
    {
        [JsonProperty("maskConfigSlots")]
        public MaskConfigSlot[] MaskConfigSlots { get; set; }
    }

    public partial class MaskConfigSlot
    {
        [JsonProperty("configSlotEntitlementId")]
        public string ConfigSlotEntitlementId { get; set; }

        [JsonProperty("configSlotItemId")]
        public string ConfigSlotItemId { get; set; }

        [JsonProperty("itemInventorySlotAvailability")]
        public string ItemInventorySlotAvailability { get; set; }

        [JsonProperty("maskConfig")]
        public MaskConfig MaskConfig { get; set; }

        [JsonProperty("maskInSlotAccelByteItemId")]
        public string MaskInSlotAccelByteItemId { get; set; }

        [JsonProperty("maskInSlotEntitlementId")]
        public string MaskInSlotEntitlementId { get; set; }

        [JsonProperty("maskInventorySlotType")]
        public string MaskInventorySlotType { get; set; }

        [JsonProperty("maskPresetConfig")]
        public MaskPresetConfig MaskPresetConfig { get; set; }
    }

    public partial class MaskPresetConfig
    {
        [JsonProperty("maskPresetData")]
        public string MaskPresetData { get; set; }
    }

    public partial class PlayerCosmeticsConfig
    {
        [JsonProperty("gloveConfigSlotIndex")]
        public long GloveConfigSlotIndex { get; set; }

        [JsonProperty("maskConfigSlotIndex")]
        public long MaskConfigSlotIndex { get; set; }

        [JsonProperty("suitConfigSlotIndex")]
        public long SuitConfigSlotIndex { get; set; }
    }

    public partial class PlayerLoadoutConfigArray
    {
        [JsonProperty("armor")]
        public string Armor { get; set; }

        [JsonProperty("loadoutName")]
        public string LoadoutName { get; set; }

        [JsonProperty("overkillWeapon")]
        public string OverkillWeapon { get; set; }

        [JsonProperty("placeable")]
        public string Placeable { get; set; }

        [JsonProperty("primaryWeaponConfigSlotIndex")]
        public long PrimaryWeaponConfigSlotIndex { get; set; }

        [JsonProperty("secondaryWeaponConfigSlotIndex")]
        public long SecondaryWeaponConfigSlotIndex { get; set; }

        [JsonProperty("skills")]
        public object[] Skills { get; set; }

        [JsonProperty("throwable")]
        public string Throwable { get; set; }

        [JsonProperty("tool")]
        public string Tool { get; set; }
    }

    public partial class SuitConfigInventorySaveData
    {
        [JsonProperty("suitConfigSlots")]
        public SuitConfigSlot[] SuitConfigSlots { get; set; }
    }

    public partial class SuitConfigSlot
    {
        [JsonProperty("configSlotEntitlementId")]
        public string ConfigSlotEntitlementId { get; set; }

        [JsonProperty("configSlotItemId")]
        public string ConfigSlotItemId { get; set; }

        [JsonProperty("itemInventorySlotAvailability")]
        public string ItemInventorySlotAvailability { get; set; }

        [JsonProperty("suitConfig")]
        public SuitConfig SuitConfig { get; set; }

        [JsonProperty("suitInSlotAccelByteItemId")]
        public string SuitInSlotAccelByteItemId { get; set; }

        [JsonProperty("suitInSlotEntitlementId")]
        public string SuitInSlotEntitlementId { get; set; }

        [JsonProperty("suitInventorySlotType")]
        public string SuitInventorySlotType { get; set; }

        [JsonProperty("suitPresetConfig")]
        public SuitPresetConfig SuitPresetConfig { get; set; }
    }

    public partial class SuitPresetConfig
    {
        [JsonProperty("suitPresetData")]
        public string SuitPresetData { get; set; }
    }
}

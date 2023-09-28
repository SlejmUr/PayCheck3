using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PayCheckServerLib.Jsons
{
    public class Progression
    {
        public partial class Basic
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
            public Dictionary<string, int> CurrentRequestedPriceProgressionLevel { get; set; }

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

            [JsonProperty("storyProgression")]
            public long StoryProgression { get; set; }

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
            public PrimaryWeaponConfigSlot[] PrimaryWeaponConfigSlots { get; set; }

            [JsonProperty("secondaryWeaponConfigSlots")]
            public SecondaryWeaponConfigSlot[] SecondaryWeaponConfigSlots { get; set; }
        }

        public partial class PrimaryWeaponConfigSlot
        {
            [JsonProperty("configSlotEntitlementId")]
            public string ConfigSlotEntitlementId { get; set; }

            [JsonProperty("configSlotItemId")]
            public string ConfigSlotItemId { get; set; }

            [JsonProperty("itemInventorySlotAvailability")]
            public string ItemInventorySlotAvailability { get; set; }

            [JsonProperty("weaponConfigInventorySlot")]
            public PrimaryWeaponConfigSlotWeaponConfigInventorySlot WeaponConfigInventorySlot { get; set; }

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

        public partial class PrimaryWeaponConfigSlotWeaponConfigInventorySlot
        {
            [JsonProperty("equippableConfig")]
            public Config EquippableConfig { get; set; }

            [JsonProperty("payedWeaponPartAttachmentItemIdArray")]
            public object[] PayedWeaponPartAttachmentItemIdArray { get; set; }
        }

        public partial class Config
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

        public partial class SecondaryWeaponConfigSlot
        {
            [JsonProperty("configSlotEntitlementId")]
            public string ConfigSlotEntitlementId { get; set; }

            [JsonProperty("configSlotItemId")]
            public string ConfigSlotItemId { get; set; }

            [JsonProperty("itemInventorySlotAvailability")]
            public string ItemInventorySlotAvailability { get; set; }

            [JsonProperty("weaponConfigInventorySlot")]
            public SecondaryWeaponConfigSlotWeaponConfigInventorySlot WeaponConfigInventorySlot { get; set; }

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

        public partial class SecondaryWeaponConfigSlotWeaponConfigInventorySlot
        {
            [JsonProperty("equippableConfig")]
            public EquippableConfig EquippableConfig { get; set; }

            [JsonProperty("payedWeaponPartAttachmentItemIdArray")]
            public string[] PayedWeaponPartAttachmentItemIdArray { get; set; }
        }

        public partial class EquippableConfig
        {
            [JsonProperty("equippableData")]
            public string EquippableData { get; set; }

            [JsonProperty("modDataMap")]
            public EquippableConfigModDataMap ModDataMap { get; set; }
        }

        public partial class EquippableConfigModDataMap
        {
            [JsonProperty("SBZModularPartSlot'/Game/Weapons/WeaponPartSlots/SLOT_BarrelExtension_xdotx_SLOT_BarrelExtension'", NullValueHandling = NullValueHandling.Ignore)]
            public Sbz SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotBarrelExtensionXdotxSlotBarrelExtension { get; set; }
        }

        public partial class Sbz
        {
            [JsonProperty("config")]
            public ConfigEnum Config { get; set; }

            [JsonProperty("part")]
            public string Part { get; set; }
        }

        public partial class Loadout
        {
            [JsonProperty("armorData")]
            public string ArmorData { get; set; }

            [JsonProperty("equippableConfigArray")]
            public EquippableConfigArray[] EquippableConfigArray { get; set; }

            [JsonProperty("gloveData")]
            public string GloveData { get; set; }

            [JsonProperty("maskConfig")]
            public MaskConfig MaskConfig { get; set; }

            [JsonProperty("maskData")]
            public string MaskData { get; set; }

            [JsonProperty("modifiableLoadoutDataArray")]
            public ModifiableLoadoutDataArray[] ModifiableLoadoutDataArray { get; set; }

            [JsonProperty("overkillWeaponConfig")]
            public Config OverkillWeaponConfig { get; set; }

            [JsonProperty("preferredCharacterDataArray")]
            public string[] PreferredCharacterDataArray { get; set; }

            [JsonProperty("skillArray")]
            public string[] SkillArray { get; set; }

            [JsonProperty("suitConfig")]
            public SuitConfig SuitConfig { get; set; }

            [JsonProperty("suitData")]
            public string SuitData { get; set; }

            [JsonProperty("throwableConfigArray")]
            public ThrowableConfigArray[] ThrowableConfigArray { get; set; }
        }

        public partial class EquippableConfigArray
        {
            [JsonProperty("equippableData")]
            public string EquippableData { get; set; }

            [JsonProperty("modDataMap")]
            public EquippableConfigArrayModDataMap ModDataMap { get; set; }
        }

        public partial class EquippableConfigArrayModDataMap
        {
            [JsonProperty("SBZModularPartSlot'/Game/Weapons/WeaponPartSlots/SLOT_BarrelExtension_xdotx_SLOT_BarrelExtension'", NullValueHandling = NullValueHandling.Ignore)]
            public SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotBarrelExtensionXdotxSlotBarrelExtension SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotBarrelExtensionXdotxSlotBarrelExtension { get; set; }

            [JsonProperty("SBZModularPartSlot'/Game/Weapons/WeaponPartSlots/SLOT_Barrel_xdotx_SLOT_Barrel'", NullValueHandling = NullValueHandling.Ignore)]
            public SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotBarrelXdotxSlotBarrel SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotBarrelXdotxSlotBarrel { get; set; }

            [JsonProperty("SBZModularPartSlot'/Game/Weapons/WeaponPartSlots/SLOT_Body_xdotx_SLOT_Body'", NullValueHandling = NullValueHandling.Ignore)]
            public SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotBodyXdotxSlotBody SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotBodyXdotxSlotBody { get; set; }

            [JsonProperty("SBZModularPartSlot'/Game/Weapons/WeaponPartSlots/SLOT_ChargingHandle_xdotx_SLOT_ChargingHandle'", NullValueHandling = NullValueHandling.Ignore)]
            public SbzModularPartSlotGameWeaponsWeaponPartSlotsSlot SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotChargingHandleXdotxSlotChargingHandle { get; set; }

            [JsonProperty("SBZModularPartSlot'/Game/Weapons/WeaponPartSlots/SLOT_ForeGrip_xdotx_SLOT_ForeGrip'", NullValueHandling = NullValueHandling.Ignore)]
            public SbzModularPartSlotGameWeaponsWeaponPartSlotsSlot SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotForeGripXdotxSlotForeGrip { get; set; }

            [JsonProperty("SBZModularPartSlot'/Game/Weapons/WeaponPartSlots/SLOT_FrontSight_xdotx_SLOT_FrontSight'", NullValueHandling = NullValueHandling.Ignore)]
            public SbzModularPartSlotGameWeaponsWeaponPartSlotsSlot SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotFrontSightXdotxSlotFrontSight { get; set; }

            [JsonProperty("SBZModularPartSlot'/Game/Weapons/WeaponPartSlots/SLOT_Grip_xdotx_SLOT_Grip'", NullValueHandling = NullValueHandling.Ignore)]
            public SbzModularPartSlotGameWeaponsWeaponPartSlotsSlot SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotGripXdotxSlotGrip { get; set; }

            [JsonProperty("SBZModularPartSlot'/Game/Weapons/WeaponPartSlots/SLOT_Mag_xdotx_SLOT_Mag'", NullValueHandling = NullValueHandling.Ignore)]
            public SbzModularPartSlotGameWeaponsWeaponPartSlotsSlot SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotMagXdotxSlotMag { get; set; }

            [JsonProperty("SBZModularPartSlot'/Game/Weapons/WeaponPartSlots/SLOT_StockMount_xdotx_SLOT_StockMount'", NullValueHandling = NullValueHandling.Ignore)]
            public SbzModularPartSlotGameWeaponsWeaponPartSlotsSlot SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotStockMountXdotxSlotStockMount { get; set; }

            [JsonProperty("SBZModularPartSlot'/Game/Weapons/WeaponPartSlots/SLOT_Stock_xdotx_SLOT_Stock'", NullValueHandling = NullValueHandling.Ignore)]
            public SbzModularPartSlotGameWeaponsWeaponPartSlotsSlot SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotStockXdotxSlotStock { get; set; }
        }

        public partial class SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotBarrelExtensionXdotxSlotBarrelExtension
        {
            [JsonProperty("config")]
            public ConfigUnion Config { get; set; }

            [JsonProperty("part")]
            public string Part { get; set; }
        }

        public partial class ConfigConfig
        {
            [JsonProperty("_ClassName")]
            public string ClassName { get; set; }

            [JsonProperty("bAddMeshes")]
            public bool BAddMeshes { get; set; }

            [JsonProperty("meshes")]
            public Mesh[] Meshes { get; set; }

            [JsonProperty("nativeClass")]
            public string NativeClass { get; set; }

            [JsonProperty("overriddenWeaponMaterialPerMesh")]
            public CurrentRequestedPriceProgressionLevel OverriddenWeaponMaterialPerMesh { get; set; }

            [JsonProperty("overriddenWeaponPart")]
            public string OverriddenWeaponPart { get; set; }
        }

        public partial class Mesh
        {
            [JsonProperty("appliedPartBoneToRig")]
            public object[] AppliedPartBoneToRig { get; set; }

            [JsonProperty("attachBoneParentOverride")]
            public ConfigEnum AttachBoneParentOverride { get; set; }

            [JsonProperty("bModifiesBaseMesh")]
            public bool BModifiesBaseMesh { get; set; }

            [JsonProperty("bNotAnimatedPart")]
            public bool BNotAnimatedPart { get; set; }

            [JsonProperty("bVisibilityTagState")]
            public bool BVisibilityTagState { get; set; }

            [JsonProperty("dontApplyPartBone")]
            public object[] DontApplyPartBone { get; set; }

            [JsonProperty("dontApplyPartBoneIfAlreadyChanged")]
            public object[] DontApplyPartBoneIfAlreadyChanged { get; set; }

            [JsonProperty("dontSpawnIfTagIsPresent")]
            public DontSpawnIfTagIsPresent DontSpawnIfTagIsPresent { get; set; }

            [JsonProperty("mapPartBoneToRigBone")]
            public CurrentRequestedPriceProgressionLevel MapPartBoneToRigBone { get; set; }

            [JsonProperty("rigBoneAnimConstraints")]
            public object[] RigBoneAnimConstraints { get; set; }

            [JsonProperty("skeletalMesh")]
            public string SkeletalMesh { get; set; }

            [JsonProperty("skeletalMeshClass")]
            public SkeletalMeshClass SkeletalMeshClass { get; set; }

            [JsonProperty("spawnOnlyIfAllTagsArePresent")]
            public DontSpawnIfTagIsPresent SpawnOnlyIfAllTagsArePresent { get; set; }

            [JsonProperty("spawnSlot")]
            public string SpawnSlot { get; set; }

            [JsonProperty("spawnStep")]
            public SpawnStep SpawnStep { get; set; }

            [JsonProperty("tagContainer")]
            public DontSpawnIfTagIsPresent TagContainer { get; set; }

            [JsonProperty("visibilityTag")]
            public YTag VisibilityTag { get; set; }
        }

        public partial class DontSpawnIfTagIsPresent
        {
            [JsonProperty("gameplayTags")]
            public YTag[] GameplayTags { get; set; }
        }

        public partial class YTag
        {
            [JsonProperty("tagName")]
            public string TagName { get; set; }
        }

        public partial class SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotBarrelXdotxSlotBarrel
        {
            [JsonProperty("config")]
            public SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotBarrelXdotxSlotBarrelConfig Config { get; set; }

            [JsonProperty("part")]
            public string Part { get; set; }
        }

        public partial class SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotBarrelXdotxSlotBarrelConfig
        {
            [JsonProperty("_ClassName")]
            public string ClassName { get; set; }

            [JsonProperty("bAddMeshes")]
            public bool BAddMeshes { get; set; }

            [JsonProperty("meshes")]
            public Mesh[] Meshes { get; set; }

            [JsonProperty("nativeClass")]
            public string NativeClass { get; set; }

            [JsonProperty("overriddenWeaponMaterialPerMesh")]
            public PurpleOverriddenWeaponMaterialPerMesh OverriddenWeaponMaterialPerMesh { get; set; }

            [JsonProperty("overriddenWeaponPart")]
            public string OverriddenWeaponPart { get; set; }
        }

        public partial class PurpleOverriddenWeaponMaterialPerMesh
        {
            [JsonProperty("SkeletalMesh'/Game/Weapons/AssaultRifles/CAR4/SK_WP_AssaultRifle_CAR4_Barrel_xdotx_SK_WP_AssaultRifle_CAR4_Barrel'")]
            public SkeletalMeshGameWeaponsAssaultRiflesCar4SkWpAssaultRifleCar4 SkeletalMeshGameWeaponsAssaultRiflesCar4SkWpAssaultRifleCar4BarrelXdotxSkWpAssaultRifleCar4Barrel { get; set; }
        }

        public partial class SkeletalMeshGameWeaponsAssaultRiflesCar4SkWpAssaultRifleCar4
        {
            [JsonProperty("weaponMaterialPerMesh")]
            public string[] WeaponMaterialPerMesh { get; set; }
        }

        public partial class SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotBodyXdotxSlotBody
        {
            [JsonProperty("config")]
            public SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotBodyXdotxSlotBodyConfig Config { get; set; }

            [JsonProperty("part")]
            public string Part { get; set; }
        }

        public partial class SbzModularPartSlotGameWeaponsWeaponPartSlotsSlotBodyXdotxSlotBodyConfig
        {
            [JsonProperty("_ClassName")]
            public string ClassName { get; set; }

            [JsonProperty("bAddMeshes")]
            public bool BAddMeshes { get; set; }

            [JsonProperty("meshes")]
            public Mesh[] Meshes { get; set; }

            [JsonProperty("nativeClass")]
            public string NativeClass { get; set; }

            [JsonProperty("overriddenWeaponMaterialPerMesh")]
            public FluffyOverriddenWeaponMaterialPerMesh OverriddenWeaponMaterialPerMesh { get; set; }

            [JsonProperty("overriddenWeaponPart")]
            public string OverriddenWeaponPart { get; set; }
        }

        public partial class FluffyOverriddenWeaponMaterialPerMesh
        {
            [JsonProperty("SkeletalMesh'/Game/Weapons/AssaultRifles/CAR4/SK_WP_AssaultRifle_CAR4_LowerReceiver_xdotx_SK_WP_AssaultRifle_CAR4_LowerReceiver'")]
            public SkeletalMeshGameWeaponsAssaultRiflesCar4SkWpAssaultRifleCar4 SkeletalMeshGameWeaponsAssaultRiflesCar4SkWpAssaultRifleCar4LowerReceiverXdotxSkWpAssaultRifleCar4LowerReceiver { get; set; }
        }

        public partial class SbzModularPartSlotGameWeaponsWeaponPartSlotsSlot
        {
            [JsonProperty("config")]
            public ConfigConfig Config { get; set; }

            [JsonProperty("part")]
            public string Part { get; set; }
        }

        public partial class MaskConfig
        {
            [JsonProperty("maskData")]
            public string MaskData { get; set; }

            [JsonProperty("modDataMap")]
            public MaskConfigModDataMap ModDataMap { get; set; }
        }

        public partial class MaskConfigModDataMap
        {
            [JsonProperty("SBZCosmeticsPartSlot'/Game/Gameplay/Cosmetics/CosmeticsPartSlots/SLOT_MaskMould_xdotx_SLOT_MaskMould'", NullValueHandling = NullValueHandling.Ignore)]
            public Sbz SbzCosmeticsPartSlotGameGameplayCosmeticsCosmeticsPartSlotsSlotMaskMouldXdotxSlotMaskMould { get; set; }
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
            public ConfigEnum MaskPresetData { get; set; }
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
            public string[] Skills { get; set; }

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
            public ConfigEnum SuitPresetData { get; set; }
        }

        public enum ConfigEnum { None };

        public enum SkeletalMeshClass { ClassScriptEngineXdotxSkeletalMeshComponent };

        public enum SpawnStep { Adapter, Default, Mount };

        public partial struct ConfigUnion
        {
            public ConfigConfig ConfigConfig;
            public ConfigEnum? Enum;

            public static implicit operator ConfigUnion(ConfigConfig ConfigConfig) => new ConfigUnion { ConfigConfig = ConfigConfig };
            public static implicit operator ConfigUnion(ConfigEnum Enum) => new ConfigUnion { Enum = Enum };
        }

        public partial class Basic
        {
            public static Basic FromJson(string json) => JsonConvert.DeserializeObject<Basic>(json, Converter.Settings);
        }

        public static class Serialize
        {
            public static string ToJson(Basic self) => JsonConvert.SerializeObject(self, Converter.Settings);
        }

        public static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters = {
                ConfigEnumConverter.Singleton,
                ConfigUnionConverter.Singleton,
                SkeletalMeshClassConverter.Singleton,
                SpawnStepConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            };
        }

        internal class ConfigEnumConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(ConfigEnum) || t == typeof(ConfigEnum?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                if (value == "None")
                {
                    return ConfigEnum.None;
                }
                throw new Exception("Cannot unmarshal type ConfigEnum");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (ConfigEnum)untypedValue;
                if (value == ConfigEnum.None)
                {
                    serializer.Serialize(writer, "None");
                    return;
                }
                throw new Exception("Cannot marshal type ConfigEnum");
            }

            public static readonly ConfigEnumConverter Singleton = new ConfigEnumConverter();
        }

        internal class ConfigUnionConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(ConfigUnion) || t == typeof(ConfigUnion?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.String:
                    case JsonToken.Date:
                        var stringValue = serializer.Deserialize<string>(reader);
                        if (stringValue == "None")
                        {
                            return new ConfigUnion { Enum = ConfigEnum.None };
                        }
                        break;
                    case JsonToken.StartObject:
                        var objectValue = serializer.Deserialize<ConfigConfig>(reader);
                        return new ConfigUnion { ConfigConfig = objectValue };
                }
                throw new Exception("Cannot unmarshal type ConfigUnion");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                var value = (ConfigUnion)untypedValue;
                if (value.Enum != null)
                {
                    if (value.Enum == ConfigEnum.None)
                    {
                        serializer.Serialize(writer, "None");
                        return;
                    }
                }
                if (value.ConfigConfig != null)
                {
                    serializer.Serialize(writer, value.ConfigConfig);
                    return;
                }
                throw new Exception("Cannot marshal type ConfigUnion");
            }

            public static readonly ConfigUnionConverter Singleton = new ConfigUnionConverter();
        }

        internal class SkeletalMeshClassConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(SkeletalMeshClass) || t == typeof(SkeletalMeshClass?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                if (value == "Class'/Script/Engine_xdotx_SkeletalMeshComponent'")
                {
                    return SkeletalMeshClass.ClassScriptEngineXdotxSkeletalMeshComponent;
                }
                throw new Exception("Cannot unmarshal type SkeletalMeshClass");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (SkeletalMeshClass)untypedValue;
                if (value == SkeletalMeshClass.ClassScriptEngineXdotxSkeletalMeshComponent)
                {
                    serializer.Serialize(writer, "Class'/Script/Engine_xdotx_SkeletalMeshComponent'");
                    return;
                }
                throw new Exception("Cannot marshal type SkeletalMeshClass");
            }

            public static readonly SkeletalMeshClassConverter Singleton = new SkeletalMeshClassConverter();
        }

        internal class SpawnStepConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(SpawnStep) || t == typeof(SpawnStep?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "Adapter":
                        return SpawnStep.Adapter;
                    case "Default":
                        return SpawnStep.Default;
                    case "Mount":
                        return SpawnStep.Mount;
                }
                throw new Exception("Cannot unmarshal type SpawnStep");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (SpawnStep)untypedValue;
                switch (value)
                {
                    case SpawnStep.Adapter:
                        serializer.Serialize(writer, "Adapter");
                        return;
                    case SpawnStep.Default:
                        serializer.Serialize(writer, "Default");
                        return;
                    case SpawnStep.Mount:
                        serializer.Serialize(writer, "Mount");
                        return;
                }
                throw new Exception("Cannot marshal type SpawnStep");
            }

            public static readonly SpawnStepConverter Singleton = new SpawnStepConverter();
        }
    }
}

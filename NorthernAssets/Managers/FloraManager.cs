using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx.Configuration;
using HarmonyLib;
using ItemManager;
using NorthernAssets;
using NorthernAssets.Behaviors;
using UnityEngine;

namespace Managers;

public abstract class FloraManager
{
    private static readonly List<Vegetation> m_vegetation = new();
    private static readonly List<TreeLogFlora> m_treeLogFlora = new();
    private static readonly List<Stub> m_destructibleFlora = new();
    private static readonly List<GameObject> Extras = new();

    public static void RegisterAsset(AssetBundle assetBundle, string prefabName)
    {
        if (assetBundle.LoadAsset<GameObject>(prefabName) is {} prefab) Extras.Add(prefab);
        else Debug.LogWarning(prefabName + " is null");
    }
    
    static FloraManager()
    {
        Harmony harmony = new("org.bepinex.helpers.FloraManager");
        harmony.Patch(AccessTools.DeclaredMethod(typeof(ZNetScene), nameof(ZNetScene.Awake)), postfix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(FloraManager), nameof(Patch_ZNetScene_Awake))));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(ZoneSystem), nameof(ZoneSystem.SetupLocations)), postfix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(FloraManager), nameof(Patch_ZoneSystem_SetupLocations_Patch))));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(FejdStartup), nameof(FejdStartup.Awake)), new HarmonyMethod(AccessTools.DeclaredMethod(typeof(FloraManager), nameof(Patch_FejdStartup))));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(ObjectDB), nameof(ObjectDB.Awake)), postfix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(FloraManager), nameof(Patch_ObjectDB_Awake))));
    }

    internal static void Patch_FejdStartup()
    {
        foreach (Vegetation flora in m_vegetation)
        {
            flora.config.Enabled = NorthernAssetsPlugin._Plugin.config(flora.ConfigGroup, "_Enabled", flora.Enabled, "If on, vegetation will spawn");
            flora.config.Biome = NorthernAssetsPlugin._Plugin.config(flora.ConfigGroup, "Biome", flora.Biome, $"Set biome where {flora.Prefab.name} can spawn");
            flora.config.BiomeArea = NorthernAssetsPlugin._Plugin.config(flora.ConfigGroup, "Biome Area", flora.BiomeArea, "Set biome area");
            flora.config.MinSpawn = NorthernAssetsPlugin._Plugin.config(flora.ConfigGroup, "Mininum", flora.MinSpawn, "Set minimum to spawn in a zone");
            flora.config.MaxSpawn = NorthernAssetsPlugin._Plugin.config(flora.ConfigGroup, "Maximum", flora.MaxSpawn, "Set maximum to spawn in a zone");
            flora.config.MinAltitude = NorthernAssetsPlugin._Plugin.config(flora.ConfigGroup, "Min Altitude", flora.MinAltitude, "Set minimum altitude");
            flora.config.MaxAltitude = NorthernAssetsPlugin._Plugin.config(flora.ConfigGroup, "Max Altitude", flora.MaxAltitude, "Set maximum altitude");

            void ConfigChanged(object o, EventArgs e)
            {
                ZoneSystem.ZoneVegetation vegetation = ZoneSystem.instance.m_vegetation.Find(x => x.m_name == flora.m_name);
                if (vegetation == null) return;
                vegetation.m_enable = flora.config.Enabled.Value is NorthernAssetsPlugin.Toggle.On;
                vegetation.m_biome = flora.config.Biome.Value;
                vegetation.m_biomeArea = flora.config.BiomeArea.Value;
                vegetation.m_min = flora.config.MinSpawn.Value;
                vegetation.m_max = flora.config.MaxSpawn.Value;
                vegetation.m_minAltitude = flora.config.MinAltitude.Value;
                vegetation.m_maxAltitude = flora.config.MaxAltitude.Value;
            }

            flora.config.Enabled.SettingChanged += ConfigChanged;
            flora.config.Biome.SettingChanged += ConfigChanged;
            flora.config.BiomeArea.SettingChanged += ConfigChanged;
            flora.config.MinSpawn.SettingChanged += ConfigChanged;
            flora.config.MaxSpawn.SettingChanged += ConfigChanged;
            flora.config.MinAltitude.SettingChanged += ConfigChanged;
            flora.config.MaxAltitude.SettingChanged += ConfigChanged;
        }

        foreach (var flora in m_treeLogFlora)
        {
            flora.config.Drops = NorthernAssetsPlugin._Plugin.config(flora.ConfigGroup, "Log Drops", ConvertDropData(flora.GetDrops()), $"Define {flora.ConfigGroup} log drops");

            void ConfigChanged(object o, EventArgs e)
            {
                if (!flora.Prefab.TryGetComponent(out TreeLog component)) return;
                component.m_dropWhenDestroyed = GetDropTable(GetFloraDrops(flora.config.Drops.Value), flora.GetDropTableData());
            }

            flora.config.Drops.SettingChanged += ConfigChanged;
        }

        foreach (var flora in m_destructibleFlora)
        {
            flora.config.Drops = NorthernAssetsPlugin._Plugin.config(flora.ConfigGroup, "Stub Drops", ConvertDropData(flora.GetDrops()), $"Define {flora.ConfigGroup} stub drops");
            void ConfigChanged(object o, EventArgs e)
            {
                if (!flora.Prefab.TryGetComponent(out DropOnDestroyed component)) return;
                component.m_dropWhenDestroyed = GetDropTable(GetFloraDrops(flora.config.Drops.Value), flora.GetDropTableData());
            }

            flora.config.Drops.SettingChanged += ConfigChanged;
        }
    }

    private static List<FloraDrop> GetFloraDrops(string config)
    {
        string[] drops = config.Split(',');
        List<FloraDrop> output = new();
        foreach (var drop in drops)
        {
            string[] values = drop.Split(':');
            if (values.Length < 4) continue;
            var name = values[0];
            if (!int.TryParse(values[1], out int min)) continue;
            if (!int.TryParse(values[2], out int max)) continue;
            if (!float.TryParse(values[3], out float weight)) continue;
            output.Add(new FloraDrop(name, min, max, weight));
        }

        return output;
    }
    
    private static string ConvertDropData(List<FloraDrop> data)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (var index = 0; index < data.Count; index++)
        {
            var drop = data[index];
            stringBuilder.AppendFormat("{0}:{1}:{2}:{3}", drop.m_itemName, drop.m_min, drop.m_max, drop.m_weight);
            if (index < data.Count - 1) stringBuilder.Append(",");
        }

        return stringBuilder.ToString();
    }

    internal static void Patch_ZNetScene_Awake()
    {
        foreach(var prefab in Extras) Register(prefab);
    }

    [HarmonyPriority(Priority.Last)]
    internal static void Patch_ObjectDB_Awake(ObjectDB __instance)
    {
        if (!__instance || !ZNetScene.instance) return;
        foreach (Vegetation flora in m_vegetation)
        {
            AddPickableData(flora);
            AddTreeBaseData(flora);
            AddTreeHoverText(flora);
            Register(flora.Prefab);
        }
        
        foreach (var destructible in m_destructibleFlora)
        {
            AddDestructibleData(destructible);
            AddDropOnDestroyedData(destructible);
            Register(destructible.Prefab);
        }
        foreach (var log in m_treeLogFlora)
        {
            AddTreeLogData(log);
            Register(log.Prefab);
        }
    }

    private static void AddPickableData(Vegetation flora)
    {
        if (!flora.Prefab.TryGetComponent(out Pickable component)) return;
        component.m_pickEffector = GetEffects(flora.GetPickEffects());
    }

    private static void AddTreeHoverText(Vegetation flora)
    {
        if (!flora.Prefab.TryGetComponent(out HoverText component)) return;
        component.m_text = "$" + flora.Name.Key;
    }

    private static void AddTreeBaseData(Vegetation flora)
    {
        if (!flora.Prefab.TryGetComponent(out TreeBase component)) return;
        component.m_destroyedEffect = GetEffects(flora.m_treeBase.GetDestroyedEffects());
        component.m_hitEffect = GetEffects(flora.m_treeBase.GetHitEffects());
        component.m_respawnEffect = GetEffects(flora.m_treeBase.GetRespawnEffects());
    }

    private static void AddTreeLogData(TreeLogFlora log)
    {
        if (!log.Prefab.TryGetComponent(out TreeLog component)) return;
        component.m_destroyedEffect = GetEffects(log.GetDestroyedEffects());
        component.m_hitEffect = GetEffects(log.GetHitEffects());
        component.m_dropWhenDestroyed = GetDropTable(GetFloraDrops(log.config.Drops.Value), log.GetDropTableData());
        if (log.Prefab.TryGetComponent(out ImpactEffect impactEffect))
        {
            impactEffect.m_hitEffect = GetEffects(log.GetHitImpactEffects());
            impactEffect.m_destroyEffect = GetEffects(log.GetHitDestroyEffects());
        }

        if (log.Prefab.TryGetComponent(out Floating floating))
        {
            floating.m_impactEffects = GetEffects(log.GetFloatingImpactEffects());
        }
    }

    private static void AddDestructibleData(Stub destructible)
    {
        if (!destructible.Prefab.TryGetComponent(out Destructible component)) return;
        component.m_destroyedEffect = GetEffects(destructible.GetDestroyedEffects());
        component.m_hitEffect = GetEffects(destructible.GetHitEffects());
    }

    private static void AddDropOnDestroyedData(Stub destructible)
    {
        if (!destructible.Prefab.TryGetComponent(out DropOnDestroyed component)) return;
        component.m_dropWhenDestroyed = GetDropTable(destructible.GetDrops(), destructible.GetDropTableData());
    }

    private static DropTable GetDropTable(List<FloraDrop> list, DropTableData dropTableData)
    {
        if (!ZNetScene.instance) return new DropTable();
        List<DropTable.DropData> drops = new();
        foreach (var data in list)
        {
            if (ZNetScene.instance.GetPrefab(data.m_itemName) is { } prefab)
            {
                drops.Add(new DropTable.DropData()
                {
                    m_item = prefab,
                    m_stackMin = data.m_min,
                    m_stackMax = data.m_max,
                    m_weight = data.m_weight,
                    m_dontScale = data.m_scale
                });
            }
        }

        return new DropTable()
        {
            m_drops = drops,
            m_dropMin = dropTableData.m_min,
            m_dropMax = dropTableData.m_max,
            m_oneOfEach = dropTableData.m_oneOfEach,
            m_dropChance = dropTableData.m_chance
        };
    }

    private static EffectList GetEffects(List<string> list)
    {
        List<EffectList.EffectData> data = new();
        foreach (var prefabName in list)
        {
            if (ZNetScene.instance.GetPrefab(prefabName) is { } prefab)
            {
                data.Add(new EffectList.EffectData()
                {
                    m_prefab = prefab,
                    m_enabled = true,
                });
            }
        }

        return new EffectList()
        {
            m_effectPrefabs = data.ToArray()
        };
    }

    internal static void Patch_ZoneSystem_SetupLocations_Patch(ZoneSystem __instance)
    {
        __instance.m_vegetation.AddRange(m_vegetation.Select(flora => flora.GetZoneVegetation()).ToList());
    }

    private static void Register(GameObject prefab)
    {
        if (!ZNetScene.instance.m_prefabs.Contains(prefab)) ZNetScene.instance.m_prefabs.Add(prefab);
        if (!ZNetScene.instance.m_namedPrefabs.ContainsKey(prefab.name.GetStableHashCode()))
        {
            ZNetScene.instance.m_namedPrefabs[prefab.name.GetStableHashCode()] = prefab;
        }
    }

    public class Vegetation
    {
        public readonly GameObject Prefab = null!;
        private readonly List<string> m_pickEffects = new();
        public void AddPickEffect(string prefab) => m_pickEffects.Add(prefab);
        public List<string> GetPickEffects() => m_pickEffects;
        public readonly TreeBaseFlora m_treeBase = new();
        
        private LocalizeKey? _name;

        public LocalizeKey Name
        {
            get
            {
                if (_name is { } name)
                {
                    return name;
                }

                var data = Prefab.GetComponent<HoverText>();
                if (data.m_text.StartsWith("$"))
                {
                    _name = new LocalizeKey(data.m_text);
                }
                else
                {
                    string key = "$prop_" + Prefab.name.Replace(" ", "_");
                    _name = new LocalizeKey(key).English(data.m_text);
                    data.m_text = key;
                }
                return _name;
            }
        }

        public class TreeBaseFlora
        {
            private readonly List<string> m_destroyedEffects = new();
            private readonly List<string> m_hitEffects = new();
            private readonly List<string> m_respawnEffects = new();

            public void AddDestroyedEffect(string prefab) => m_destroyedEffects.Add(prefab);
            public void AddHitEffect(string prefab) => m_hitEffects.Add(prefab);
            public void AddRespawnEffect(string prefab) => m_respawnEffects.Add(prefab);

            public List<string> GetDestroyedEffects() => m_destroyedEffects;
            public List<string> GetHitEffects() => m_hitEffects;
            public List<string> GetRespawnEffects() => m_respawnEffects;
        }
        
        // Data
        public NorthernAssetsPlugin.Toggle Enabled = NorthernAssetsPlugin.Toggle.On;
        public readonly string m_name = null!;
        public float MinSpawn = 1f;
        public float MaxSpawn = 1f;
        private bool ForcePlacement = false;
        private float ScaleMin = 1f;
        private float ScaleMax = 1f;
        private float RandomTilt = 0f;
        private float ChanceToTilt = 0f;
        public Heightmap.Biome Biome = Heightmap.Biome.None;
        public Heightmap.BiomeArea BiomeArea = Heightmap.BiomeArea.Everything;
        private bool BlockCheck = false;
        private bool SnapToStaticSolid = false;
        public float MinAltitude = 0f;
        public float MaxAltitude = 1000f;
        private float MinVegetation = 0f;
        private float MaxVegetation = 100f;
        private bool SurroundCheckVegetation = false;
        private float SurroundCheckDistance = 20f;
        private int SurroundCheckLayers = 2;
        private float SurroundBetterThanAverage = 0f;
        private float MinOceanDepth = 0f;
        private float MaxOceanDepth = 0f;
        private float MinTilt = 0f;
        private float MaxTilt = 50f;
        private float TerrainDeltaRadius = 4f;
        private float MaxTerrainDelta = 75f;
        private bool SnapToWater = false;
        private float GroundOffset = 0f;
        private int MinGroupSize = 1;
        private int MaxGroupSize = 1;
        private float GroupRadius = 0f;
        private bool InForest = false;
        private float MinForestThreshold = 0f;
        private float MaxForestThreshold = 0f;
        private readonly bool Foldout = false;

        public readonly string ConfigGroup = null!;
        internal readonly VegetationConfig config = new();
        public class VegetationConfig
        {
            public ConfigEntry<NorthernAssetsPlugin.Toggle> Enabled = null!;
            public ConfigEntry<Heightmap.Biome> Biome = null!;
            public ConfigEntry<Heightmap.BiomeArea> BiomeArea = null!;
            public ConfigEntry<float> MinAltitude = null!;
            public ConfigEntry<float> MaxAltitude = null!;
            public ConfigEntry<float> MinSpawn = null!;
            public ConfigEntry<float> MaxSpawn = null!;
        }

        public Vegetation(AssetBundle assetBundle, string prefabName, string configGroup)
        {
            if (assetBundle.LoadAsset<GameObject>(prefabName) is { } prefab)
            {
                ConfigGroup = configGroup;
                Prefab = prefab;
                m_name = prefabName;
                m_vegetation.Add(this);
            }
            else
            {
                Debug.LogWarning(prefabName + " is null");
            }
        }
        public void SetMinMaxSpawn(float min, float max)
        {
            MinSpawn = min;
            MaxSpawn = max;
        }
        
        public void SetEnabled(NorthernAssetsPlugin.Toggle enabled) => Enabled = NorthernAssetsPlugin.Toggle.On;
        public void SetSpawn(int min, int max)
        {
            MinSpawn = min;
            MaxSpawn = max;
        }
        public void SetForcePlacement(bool enable) => ForcePlacement = enable;
        
        public void SetScale(float min, float max)
        {
            ScaleMin = min;
            ScaleMax = max;
        }
        public void SetTilt(float chance, float randomTilt = 0f, float min = 0f, float max = 50f)
        {
            RandomTilt = randomTilt;
            ChanceToTilt = chance;
            MinTilt = min;
            MaxTilt = max;
        }
        public void SetBiome(Heightmap.Biome biome) => Biome = biome;
        public void SetBiomeArea(Heightmap.BiomeArea area) => BiomeArea = area;
        public void SetBlockCheck(bool enable) => BlockCheck = enable;
        public void SetSnapToStaticSolid(bool enable) => SnapToStaticSolid = enable;
        public void SetAltitude(float min = 0f, float max = 1000f)
        {
            MinAltitude = min;
            MaxAltitude = max;
        }
        public void SetVegetationCheck(bool enable, float min = 0f, float max = 1000f)
        {
            SurroundCheckVegetation = enable;
            MinVegetation = min;
            MaxVegetation = max;
        }
        public void SetSurroundCheck(float distance, int layers, float betterThanAverage = 0f)
        {
            SurroundCheckDistance = distance;
            SurroundCheckLayers = layers;
            SurroundBetterThanAverage = betterThanAverage;
        }
        public void SetOceanDepth(float min, float max)
        {
            MinOceanDepth = min;
            MaxOceanDepth = max;
        }
        public void SetTerrainDelta(float radius, float max)
        {
            TerrainDeltaRadius = radius;
            MaxTerrainDelta = max;
        }
        public void SetSnapToWater(bool enable) => SnapToWater = enable;
        public void SetGroundOffset(float offset) => GroundOffset = offset;
        
        public void SetGroupSize(int min, int max, float radius)
        {
            MinGroupSize = min;
            MaxGroupSize = max;
            GroupRadius = radius;
        }
        public void SetInForestOnly(bool enable, float min, float max)
        {
            InForest = enable;
            MinForestThreshold = min;
            MaxForestThreshold = max;
        }

        public ZoneSystem.ZoneVegetation? GetZoneVegetation()
        {
            return new()
            {
                m_name = m_name,
                m_prefab = Prefab,
                m_enable = config.Enabled.Value is NorthernAssetsPlugin.Toggle.On,
                m_min = config.MinSpawn.Value,
                m_max = config.MaxSpawn.Value,
                m_forcePlacement = ForcePlacement,
                m_scaleMin = ScaleMin,
                m_scaleMax = ScaleMax,
                m_randTilt = RandomTilt,
                m_chanceToUseGroundTilt = ChanceToTilt,
                m_biome = config.Biome.Value,
                m_biomeArea = config.BiomeArea.Value,
                m_blockCheck = BlockCheck,
                m_snapToStaticSolid = SnapToStaticSolid,
                m_minAltitude = config.MinAltitude.Value,
                m_maxAltitude = config.MaxAltitude.Value,
                m_minVegetation = MinVegetation,
                m_maxVegetation = MaxVegetation,
                m_surroundCheckVegetation = SurroundCheckVegetation,
                m_surroundCheckDistance = SurroundCheckDistance,
                m_surroundCheckLayers = SurroundCheckLayers,
                m_surroundBetterThanAverage = SurroundBetterThanAverage,
                m_minOceanDepth = MinOceanDepth,
                m_maxOceanDepth = MaxOceanDepth,
                m_minTilt = MinTilt,
                m_maxTilt = MaxTilt,
                m_terrainDeltaRadius = TerrainDeltaRadius,
                m_maxTerrainDelta = MaxTerrainDelta,
                m_snapToWater = SnapToWater,
                m_groundOffset = GroundOffset,
                m_groupSizeMin = MinGroupSize,
                m_groupSizeMax = MaxGroupSize,
                m_groupRadius = GroupRadius,
                m_inForest = InForest,
                m_forestTresholdMin = MinForestThreshold,
                m_forestTresholdMax = MaxForestThreshold,
                m_foldout = Foldout
            };
        }
    }

    public class Stub
    {
        public readonly string ConfigGroup;
        public readonly GameObject Prefab;
        private readonly List<string> m_destroyedEffects = new();
        private readonly List<string> m_hitEffects = new();
        private readonly DropTableData m_dropTableData = new();
        private readonly List<FloraDrop> m_drops = new();
        public class DestructibleConfig
        {
            public ConfigEntry<string> Drops = null!;
        }

        public readonly DestructibleConfig config = new();

        public void AddDestroyedEffect(string prefab) => m_destroyedEffects.Add(prefab);
        public void AddHitEffect(string prefab) => m_hitEffects.Add(prefab);
        public void AddDrop(string name, int min = 1, int max = 1, float weight = 1f, bool scale = false) => m_drops.Add(new FloraDrop(name, min, max, weight, scale));
        public void SetDropTable(int min = 1, int max = 4, float chance = 1f, bool oneOfEach = false)
        {
            m_dropTableData.m_min = min;
            m_dropTableData.m_max = max;
            m_dropTableData.m_chance = chance;
            m_dropTableData.m_oneOfEach = oneOfEach;
        }
        public List<string> GetDestroyedEffects() => m_destroyedEffects;
        public List<string> GetHitEffects() => m_hitEffects;
        public List<FloraDrop> GetDrops() => m_drops;
        public DropTableData GetDropTableData() => m_dropTableData;
        public Stub(AssetBundle assetBundle, string prefabName, string group)
        {
            Prefab = assetBundle.LoadAsset<GameObject>(prefabName);
            ConfigGroup = group;
            m_destructibleFlora.Add(this);
        }
    }

    public class TreeLogFlora
    {
        public readonly GameObject Prefab;
        private readonly List<string> m_destroyedEffects = new();
        private readonly List<string> m_hitEffects = new();
        private readonly List<string> m_impactHitEffects = new();
        private readonly List<string> m_impactDestroyEffects = new();
        private readonly List<string> m_floatingImpactEffects = new();
        private readonly DropTableData m_dropTableData = new();
        private readonly List<FloraDrop> m_drops = new();
        public readonly string ConfigGroup;

        public class TreeConfig
        {
            public ConfigEntry<string> Drops = null!;
        }

        public readonly TreeConfig config = new();
        
        public void AddDestroyedEffect(string prefab) => m_destroyedEffects.Add(prefab);
        public void AddHitEffect(string prefab) => m_hitEffects.Add(prefab);
        public void AddHitImpactEffect(string prefab) => m_impactHitEffects.Add(prefab);
        public void AddDestroyImpactEffect(string prefab) => m_impactDestroyEffects.Add(prefab);
        public void AddFloatingImpactEffect(string prefab) => m_floatingImpactEffects.Add(prefab);
        public void AddDrop(string name, int min = 1, int max = 1, float weight = 1f, bool scale = false) => m_drops.Add(new FloraDrop(name, min, max, weight, scale));
        public DropTableData GetDropTableData() => m_dropTableData;
        public void SetDropTable(int min = 1, int max = 4, float chance = 1f, bool oneOfEach = false)
        {
            m_dropTableData.m_min = min;
            m_dropTableData.m_max = max;
            m_dropTableData.m_chance = chance;
            m_dropTableData.m_oneOfEach = oneOfEach;

        }
        public List<string> GetDestroyedEffects() => m_destroyedEffects;
        public List<string> GetHitEffects() => m_hitEffects;
        public List<string> GetHitImpactEffects() => m_impactHitEffects;
        public List<string> GetHitDestroyEffects() => m_impactDestroyEffects;
        public List<string> GetFloatingImpactEffects() => m_floatingImpactEffects;
        public List<FloraDrop> GetDrops() => m_drops;

        public TreeLogFlora(AssetBundle assetBundle, string prefabName, string group)
        {
            Prefab = assetBundle.LoadAsset<GameObject>(prefabName);
            ConfigGroup = group;
            m_treeLogFlora.Add(this);
        }
    }
    
    public class FloraDrop
    {
        public readonly string m_itemName;
        public readonly int m_min;
        public readonly int m_max;
        public readonly float m_weight;
        public readonly bool m_scale = false;

        public FloraDrop(string name, int min = 1, int max = 1, float weight = 1f, bool scale = false)
        {
            m_itemName = name;
            m_min = min;
            m_max = max;
            m_weight = weight;
            m_scale = scale;
        }
    }

    public class DropTableData
    {
        public int m_min = 1;
        public int m_max = 4;
        [Range(0.0f, 1f)]
        public float m_chance = 1f;
        public bool m_oneOfEach = false;
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx.Configuration;
using HarmonyLib;
using ItemManager;
using UnityEngine;

namespace NorthernAssets.Managers;

public class VegetationManager
{
    private static readonly Dictionary<string, Vegetation> m_vegetation = new();
    private static readonly Dictionary<string, MineRock> m_rocks = new();

    static VegetationManager()
    {
        Harmony harmony = new("org.bepinex.helpers.VegetationManager");
        harmony.Patch(AccessTools.DeclaredMethod(typeof(ZNetScene), nameof(ZNetScene.Awake)),
            postfix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(VegetationManager), nameof(ZNetScene_Awake_Patch))));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(ZoneSystem), nameof(ZoneSystem.SetupLocations)),
            postfix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(VegetationManager), nameof(ZoneSystem_SetupLocations_Patch))));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(FejdStartup), nameof(FejdStartup.Awake)), new HarmonyMethod(AccessTools.DeclaredMethod(typeof(VegetationManager), nameof(Patch_FejdStartup))));
    }

    internal static void Patch_FejdStartup()
    {
        foreach (var rock in m_rocks.Values)
        {
            rock.config.HealthConfig = NorthernAssetsPlugin._Plugin.config(rock.ConfigGroup, "Health", rock.Health, "Define health of mine rock");
            rock.config.ToolTierConfig = NorthernAssetsPlugin._Plugin.config(rock.ConfigGroup, "Tool Tier", rock.ToolTier, "Define mininum tool tier to pick rock");
            rock.config.DropConfig = NorthernAssetsPlugin._Plugin.config(rock.ConfigGroup, "Drops", rock.DropTableData.GetDropConfigString(), "Define drops, [prefabName]:[min]:[max]:[weight]");
        }
    }

    internal static void ZNetScene_Awake_Patch()
    {
        foreach (var vegetation in m_vegetation.Values)
        {
            Register(vegetation.Prefab);
            if (vegetation.Prefab.TryGetComponent(out Destructible destructible) && destructible.m_spawnWhenDestroyed is { } spawnWhenDestroyed && spawnWhenDestroyed.GetComponent<ZNetView>())
            {
                destructible.m_minToolTier = vegetation.MinToolTier;
                if (spawnWhenDestroyed.TryGetComponent(out MineRock5 component))
                {
                    Helpers.UpdateEffects(vegetation.DestroyedEffects, ref component.m_destroyedEffect);
                    Helpers.UpdateEffects(vegetation.HitEffects, ref component.m_hitEffect);
                    component.m_minToolTier = vegetation.MinToolTier;
                    foreach (var drop in vegetation.m_drops)
                    {
                        if (ZNetScene.instance.GetPrefab(drop.m_itemName) is { } item)
                        {
                            component.m_dropItems.m_drops.Add(new DropTable.DropData()
                            {
                                m_item = item,
                                m_stackMin = drop.m_min,
                                m_stackMax = drop.m_max,
                                m_weight = drop.m_weight,
                                m_dontScale = drop.m_scale
                            });
                        }
                    }
                }
                Register(spawnWhenDestroyed);
            }
        }

        foreach (MineRock? rock in m_rocks.Values)
        {
            rock.AddDestructibleData();
            rock.AddDropOnDestroyedData();
            rock.AddHoverText();
            Register(rock.Prefab);
        }
        
        NorthernAssetsPlugin._Plugin.SetupWatcher();
    }
    
    internal static void ZoneSystem_SetupLocations_Patch(ZoneSystem __instance)
    {
        __instance.m_vegetation.AddRange(m_vegetation.Select(vegetation => vegetation.Value.GetZoneVegetation()).ToList());
        __instance.m_vegetation.AddRange(m_rocks.Select(rock => rock.Value.GetZoneVegetation()).ToList());
    }

    private static void Register(GameObject prefab)
    {
        if (!ZNetScene.instance.m_prefabs.Contains(prefab)) ZNetScene.instance.m_prefabs.Add(prefab);
        if (!ZNetScene.instance.m_namedPrefabs.ContainsKey(prefab.name.GetStableHashCode()))
        {
            ZNetScene.instance.m_namedPrefabs[prefab.name.GetStableHashCode()] = prefab;
        }
    }
    
    private static EffectList GetEffectList(List<string> list)
    {
        List<EffectList.EffectData> data = new();
        foreach (var prefabName in list)
        {
            if (ZNetScene.instance.GetPrefab(prefabName) is not { } prefab) continue;
            data.Add(new EffectList.EffectData() { m_prefab = prefab });
        }

        return new EffectList()
        {
            m_effectPrefabs = data.ToArray()
        };
    }
    
    public class MineRock
    {
        public readonly string ConfigGroup = "";
        public readonly GameObject Prefab = null!;
        public readonly List<string> DestroyedEffects = new();
        public readonly List<string> HitEffects = new();
        public readonly DropTableData DropTableData = new();
        public readonly MineRockConfigs config = new();

        public bool Enabled = true;
        public float MinSpawn = 1f;
        public float MaxSpawn = 1f;
        public bool ForcePlacement = false;
        public float ScaleMin = 1f;
        public float ScaleMax = 1f;
        public float RandomTilt = 0f;
        public float ChanceToTilt = 0f;
        public Heightmap.Biome Biome = Heightmap.Biome.None;
        public Heightmap.BiomeArea BiomeArea = Heightmap.BiomeArea.Everything;
        public bool BlockCheck = false;
        public bool SnapToStaticSolid = false;
        public float MinAltitude = 0f;
        public float MaxAltitude = 1000f;
        public float MinVegetation = 0f;
        public float MaxVegetation = 100f;
        public bool SurroundCheckVegetation = false;
        public float SurroundCheckDistance = 20f;
        public int SurroundCheckLayers = 2;
        public float SurroundBetterThanAverage = 0f;
        public float MinOceanDepth = 0f;
        public float MaxOceanDepth = 0f;
        public float MinTilt = 0f;
        public float MaxTilt = 50f;
        public float TerrainDeltaRadius = 4f;
        public float MaxTerrainDelta = 75f;
        public bool SnapToWater = false;
        public float GroundOffset = 0f;
        public int MinGroupSize = 1;
        public int MaxGroupSize = 1;
        public float GroupRadius = 0f;
        public bool InForest = false;
        public float MinForestThreshold = 0f;
        public float MaxForestThreshold = 0f;
        public bool Foldout = false;

        public float Health;
        public int ToolTier;
        public class MineRockConfigs
        {
            public ConfigEntry<float> HealthConfig = null!;
            public ConfigEntry<int> ToolTierConfig = null!;
            public ConfigEntry<string> DropConfig = null!;
            
            public List<RockDrop> GetRockDrops()
            {
                string[] drops = DropConfig.Value.Split(',');
                List<RockDrop> output = new();
                foreach (var drop in drops)
                {
                    string[] values = drop.Split(':');
                    if (values.Length < 4) continue;
                    var name = values[0];
                    if (!int.TryParse(values[1], out int min)) continue;
                    if (!int.TryParse(values[2], out int max)) continue;
                    if (!float.TryParse(values[3], out float weight)) continue;
                    output.Add(new RockDrop(name, min, max, weight));
                }

                return output;
            }
        }
        public void SetDropTable(int min = 1, int max = 4, float chance = 1f, bool oneOfEach = false)
        {
            DropTableData.m_min = min;
            DropTableData.m_max = max;
            DropTableData.m_chance = chance;
            DropTableData.m_oneOfEach = oneOfEach;
        }
        public MineRock(AssetBundle assetBundle, string prefabName, string group)
        {
            if (assetBundle.LoadAsset<GameObject>(prefabName) is { } prefab)
            {
                Prefab = prefab;
                ConfigGroup = group;
                if (!prefab.TryGetComponent(out Destructible component)) return;
                Health = component.m_health;
                ToolTier = component.m_minToolTier;
                m_rocks[prefabName] = this;
            }
            else
            {
                Debug.LogWarning(prefabName + " is null");
            }
        }
        
        
        public void AddDestructibleData()
        {
            if (!Prefab.TryGetComponent(out Destructible component)) return;
            component.m_destroyedEffect = GetEffectList(DestroyedEffects);
            component.m_hitEffect = GetEffectList(HitEffects);

            void SettingsChanged()
            {
                component.m_health = config.HealthConfig.Value;
                component.m_minToolTier = config.ToolTierConfig.Value;
            }

            component.m_health = config.HealthConfig.Value;
            component.m_minToolTier = config.ToolTierConfig.Value;
            config.HealthConfig.SettingChanged += (_, _) => SettingsChanged();
            config.ToolTierConfig.SettingChanged += (_, _) => SettingsChanged();
        }

        public void AddHoverText()
        {
            if (!Prefab.TryGetComponent(out HoverText component)) return;
            component.m_text = "$" + Name.Key;
        }
        
        public void AddDropOnDestroyedData()
        {
            if (!Prefab.TryGetComponent(out DropOnDestroyed component)) return;
            component.m_dropWhenDestroyed = DropTableData.GetDropTable();
            void ConfigChanged(object o, EventArgs e)
            {
                DropTableData.Drops = config.GetRockDrops();
                component.m_dropWhenDestroyed = DropTableData.GetDropTable();
            }
            config.DropConfig.SettingChanged += ConfigChanged;
        }
        
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
        
        public ZoneSystem.ZoneVegetation GetZoneVegetation()
        {
            return new()
            {
                m_name = Prefab.name,
                m_prefab = Prefab,
                m_enable = Enabled,
                m_min = MinSpawn,
                m_max = MaxSpawn,
                m_forcePlacement = ForcePlacement,
                m_scaleMin = ScaleMin,
                m_scaleMax = ScaleMax,
                m_randTilt = RandomTilt,
                m_chanceToUseGroundTilt = ChanceToTilt,
                m_biome = Biome,
                m_biomeArea = BiomeArea,
                m_blockCheck = BlockCheck,
                m_snapToStaticSolid = SnapToStaticSolid,
                m_minAltitude = MinAltitude,
                m_maxAltitude = MaxAltitude,
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
    
    public class RockDrop
    {
        public readonly string m_itemName;
        public readonly int m_min;
        public readonly int m_max;
        public readonly float m_weight;
        public readonly bool m_scale = false;

        public RockDrop(string name, int min = 1, int max = 1, float weight = 1f, bool scale = false)
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
        public List<RockDrop> Drops = new();
        public int m_min = 1;
        public int m_max = 4;
        [Range(0.0f, 1f)]
        public float m_chance = 1f;
        public bool m_oneOfEach = false;

        public void Add(string name, int min = 1, int max = 1, float weight = 1f, bool scale = false) => Drops.Add(new RockDrop(name, min, max, weight, scale));
        
        public string GetDropConfigString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (var index = 0; index < Drops.Count; index++)
            {
                var drop = Drops[index];
                stringBuilder.AppendFormat("{0}:{1}:{2}:{3}", drop.m_itemName, drop.m_min, drop.m_max, drop.m_weight);
                if (index < Drops.Count - 1) stringBuilder.Append(",");
            }

            return stringBuilder.ToString();
        }
        
        public DropTable GetDropTable()
        {
            if (!ZNetScene.instance) return new DropTable();
            List<DropTable.DropData> drops = new();
            foreach (RockDrop? data in Drops)
            {
                if (ZNetScene.instance.GetPrefab(data.m_itemName) is not { } prefab) continue;
                drops.Add(new DropTable.DropData()
                {
                    m_item = prefab,
                    m_stackMin = data.m_min,
                    m_stackMax = data.m_max,
                    m_weight = data.m_weight,
                    m_dontScale = data.m_scale
                });
            }

            return new DropTable()
            {
                m_drops = drops,
                m_dropMin = m_min,
                m_dropMax = m_max,
                m_oneOfEach = m_oneOfEach,
                m_dropChance = m_chance
            };
        }
    }
    
    public class Vegetation
    {
        public readonly GameObject Prefab = null!;
        // Data
        public bool Enabled = true;
        public float MinSpawn = 1f;
        public float MaxSpawn = 1f;
        public bool ForcePlacement = false;
        public float ScaleMin = 1f;
        public float ScaleMax = 1f;
        public float RandomTilt = 0f;
        public float ChanceToTilt = 0f;
        public Heightmap.Biome Biome = Heightmap.Biome.None;
        public Heightmap.BiomeArea BiomeArea = Heightmap.BiomeArea.Everything;
        public bool BlockCheck = false;
        public bool SnapToStaticSolid = false;
        public float MinAltitude = 0f;
        public float MaxAltitude = 1000f;
        public float MinVegetation = 0f;
        public float MaxVegetation = 100f;
        public bool SurroundCheckVegetation = false;
        public float SurroundCheckDistance = 20f;
        public int SurroundCheckLayers = 2;
        public float SurroundBetterThanAverage = 0f;
        public float MinOceanDepth = 0f;
        public float MaxOceanDepth = 0f;
        public float MinTilt = 0f;
        public float MaxTilt = 50f;
        public float TerrainDeltaRadius = 4f;
        public float MaxTerrainDelta = 75f;
        public bool SnapToWater = false;
        public float GroundOffset = 0f;
        public int MinGroupSize = 1;
        public int MaxGroupSize = 1;
        public float GroupRadius = 0f;
        public bool InForest = false;
        public float MinForestThreshold = 0f;
        public float MaxForestThreshold = 0f;
        public bool Foldout = false;

        public readonly List<string> DestroyedEffects = new();
        public readonly List<string> HitEffects = new();
        public readonly List<RockDrop> m_drops = new();
        public int MinToolTier = 0;

        public Vegetation(AssetBundle assetBundle, string prefabName)
        {
            if (assetBundle.LoadAsset<GameObject>(prefabName) is { } prefab)
            {
                Prefab = prefab;
                m_vegetation[prefabName] = this;
            }
            else
            {
                Debug.LogWarning(prefabName + " is null");
            }
        }

        public ZoneSystem.ZoneVegetation GetZoneVegetation()
        {
             return new()
             {
                 m_name = Prefab.name,
                 m_prefab = Prefab,
                 m_enable = Enabled,
                 m_min = MinSpawn,
                 m_max = MaxSpawn,
                 m_forcePlacement = ForcePlacement,
                 m_scaleMin = ScaleMin,
                 m_scaleMax = ScaleMax,
                 m_randTilt = RandomTilt,
                 m_chanceToUseGroundTilt = ChanceToTilt,
                 m_biome = Biome,
                 m_biomeArea = BiomeArea,
                 m_blockCheck = BlockCheck,
                 m_snapToStaticSolid = SnapToStaticSolid,
                 m_minAltitude = MinAltitude,
                 m_maxAltitude = MaxAltitude,
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
}